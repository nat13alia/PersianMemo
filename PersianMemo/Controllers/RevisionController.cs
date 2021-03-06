using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PersianMemo.Models;
using PersianMemo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using PersianMemo.UtilityClasses;

namespace PersianMemo.Controllers
{
    public class RevisionController : Controller
    {
        private readonly LanguageService _language;
        private readonly IWordRepository _wordRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRevisionRepository _revisionRepository;

        public RevisionController(LanguageService language, IWordRepository wordRepository, UserManager<IdentityUser> userManager, IRevisionRepository revisionRepository)
        {
            _wordRepository = wordRepository;
            _userManager = userManager;
            _revisionRepository = revisionRepository;
            _language = language;
        }

        [HttpGet]
        public IActionResult Details()
        {
            var currentDate = DateTime.Now.Date;
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var model = new RevisionDetailsViewModel
            {
                WordsList = _revisionRepository.GetWords(currentUserId, currentDate)
            };

            if(model.WordsList.Count() > 0)
            {
                return View(model);
            } else
            {
                return View("NoRevisionsForToday");
            }
        }

        [HttpGet]
        public IActionResult Revise(int id, string? jsToRun)
        {
            var currentDate = DateTime.Now.Date;
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ReviseViewModel model = new ReviseViewModel
            {
                Words = _revisionRepository.GetWords(currentUserId, currentDate),
                CurrentWordId = id,
                JavascriptToRun = jsToRun
            };
            if (TempData["audiopath"] != null)
            {
                ViewBag.AudioPath = TempData["audiopath"].ToString();
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Revise(ReviseViewModel model)
        {
            var currentDate = DateTime.Now.Date;
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var previousWord = _wordRepository.GetWord(model.CurrentWordId);

            var previousRevisionRow = _revisionRepository.GetRevisionRow(currentUserId, previousWord.Id, currentDate);

            var correctAnswer = _wordRepository.GetWord(model.CurrentWordId).PersianWord;
            double EFholder = 2.9;

            if (model.Answer == correctAnswer)
            {
                Word wordChanges;
                double newEF;
                //Correct answer
                if (previousWord.RevisionsCount == 0)
                {
                    if (previousRevisionRow.WriteAnswer == Answer.AnsweredIncorrectly)
                    {
                        newEF = FactorsCalculator.CalculateEF(previousWord.EF, 0);
                    }
                    else
                    {
                        newEF = FactorsCalculator.CalculateEF(previousWord.EF, 5);
                    }
                    wordChanges = new Word
                    {
                        Id = previousWord.Id,
                        UserId = previousWord.UserId,
                        PersianWord = previousWord.PersianWord,
                        Translation = previousWord.Translation,
                        Difficulty = previousWord.Difficulty,
                        PhotoPath = previousWord.PhotoPath,
                        PronunciationPath = previousWord.PronunciationPath,
                        EF = newEF,
                        Status = WordStatus.InProgress,
                        RevisionsCount = (previousWord.RevisionsCount + 1),
                        NextRevision = DateTime.Today.AddDays(3),
                        RevisionInterval = previousWord.RevisionInterval + 3
                    };
                    _wordRepository.Update(wordChanges);
                } else 
                {
                    int newInterval;
                    if (previousRevisionRow.WriteAnswer == Answer.AnsweredIncorrectly)
                    {
                        newEF = FactorsCalculator.CalculateEF(previousWord.EF, 0);
                    }
                    else
                    {
                        newEF = FactorsCalculator.CalculateEF(previousWord.EF, 5);
                    }

                    if(newEF < 1.3)
                    {
                        newEF = 1.3;
                    }

                    newInterval = Convert.ToInt32(FactorsCalculator.CalculateInterval(previousWord.RevisionInterval, newEF));

                    wordChanges = new Word
                    {
                        Id = previousWord.Id,
                        UserId = previousWord.UserId,
                        PersianWord = previousWord.PersianWord,
                        Translation = previousWord.Translation,
                        Difficulty = previousWord.Difficulty,
                        PhotoPath = previousWord.PhotoPath,
                        PronunciationPath = previousWord.PronunciationPath,
                        EF = newEF,
                        Status = WordStatus.InProgress,
                        RevisionsCount = (previousWord.RevisionsCount + 1),
                        NextRevision = DateTime.Today.AddDays(newInterval),
                        RevisionInterval = newInterval
                    };

                    if (newEF >= 3.0)
                    {
                        EFholder = 3.0;
                        wordChanges.EF = 2.5;
                        wordChanges.NextRevision = DateTime.MaxValue;
                        wordChanges.RevisionInterval = 1;
                        wordChanges.RevisionsCount = 0;
                        wordChanges.Status = WordStatus.Learned;
                        _revisionRepository.Delete(previousRevisionRow.Id);
                    }
                    _wordRepository.Update(wordChanges);
                }

                Revision revisionChanges = new Revision
                {
                    Id = previousRevisionRow.Id,
                    UserId = previousRevisionRow.UserId,
                    WordId = previousRevisionRow.WordId,
                    WriteAnswer = Answer.AnsweredCorrectly,
                    RevisionDate = wordChanges.NextRevision,
                    ModifiedDate = DateTime.Now
                };

                if (EFholder >= 3.0)
                {
                    _revisionRepository.Delete(previousRevisionRow.Id);
                } else
                {
                    _revisionRepository.Update(revisionChanges);
                }

                var revisionsLeft = _revisionRepository.GetAllRevisionsPerDay(currentDate).Where(r => r.WriteAnswer != Answer.AnsweredCorrectly).OrderBy(r => r.ModifiedDate).ToList();

                if (revisionsLeft.Count != 0)
                {
                    TempData["audiopath"] = $"/audio/{_wordRepository.GetWord(model.CurrentWordId).PronunciationPath}";
                    return RedirectToAction("revise", "revision", new { id = revisionsLeft.FirstOrDefault().WordId, jsToRun = "ShowSuccessPopup()" });
                }
                else
                {
                    var allUserRevisions = _revisionRepository.GetAllRevisionsPerUser(currentUserId).ToList();
                    foreach(Revision revision in allUserRevisions)
                    {
                        Revision revChanges = new Revision
                        {
                            Id = revision.Id,
                            UserId = revision.UserId,
                            WordId = revision.WordId,
                            WriteAnswer = Answer.NotAnsweredYet,
                            RevisionDate = revision.RevisionDate,
                            ModifiedDate = DateTime.Now
                        };
                        _revisionRepository.Update(revChanges);
                    }

                    TempData["audiopath"] = $"/audio/{_wordRepository.GetWord(model.CurrentWordId).PronunciationPath}";
                    return View("CompletedAllRevisions");
                }
            }
            else
            {
                Revision revisionChanges = new Revision
                {
                    Id = previousRevisionRow.Id,
                    UserId = previousRevisionRow.UserId,
                    WordId = previousRevisionRow.WordId,
                    WriteAnswer = Answer.AnsweredIncorrectly,
                    RevisionDate = previousRevisionRow.RevisionDate,
                    ModifiedDate = DateTime.Now
                };

                _revisionRepository.Update(revisionChanges);

                var revisionsLeft = _revisionRepository.GetAllRevisionsPerDay(currentDate).Where(r => r.WriteAnswer != Answer.AnsweredCorrectly).OrderBy(r => r.ModifiedDate).ToList();

                TempData["audiopath"] = $"/audio/{_wordRepository.GetWord(model.CurrentWordId).PronunciationPath}";
                return RedirectToAction("revise", "revision", new { id = revisionsLeft.FirstOrDefault().WordId, jsToRun = "ShowErrorPopup()" });
            }
        }

        public IActionResult CompletedAllRevisions()
        {
            if (TempData["audiopath"] != null)
            {
                ViewBag.AudioPath = TempData["audiopath"].ToString();
            }
            return View();
        }
    }
}

