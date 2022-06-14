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
        public IActionResult Revise(int id)
        {
            var currentDate = DateTime.Now.Date;
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ReviseViewModel model = new ReviseViewModel
            {
                Words = _revisionRepository.GetWords(currentUserId, currentDate),
                CurrentWordId = _revisionRepository.GetWords(currentUserId, currentDate).FirstOrDefault().Id
            };
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
            Word wordChanges;

            if (model.Answer == correctAnswer)
            {
                //Correct answer
                if (previousWord.RevisionsCount == 0)
                {
                    wordChanges = new Word
                    {
                        Id = previousWord.Id,
                        PersianWord = previousWord.PersianWord,
                        Translation = previousWord.Translation,
                        Difficulty = previousWord.Difficulty,
                        PhotoPath = previousWord.PhotoPath,
                        PronunciationPath = previousWord.PronunciationPath,
                        EF = previousWord.EF,
                        Status = WordStatus.InProgress,
                        RevisionsCount = (previousWord.RevisionInterval + 1),
                        NextRevision = DateTime.Today.AddDays(3),
                        RevisionInterval = previousWord.RevisionInterval + 3
                    };
                    _wordRepository.Update(wordChanges);
                } else 
                {
                    double newEF = FactorsCalculator.CalculateEF(previousWord.EF, 5);
                    int newInterval = Convert.ToInt32(FactorsCalculator.CalculateInterval(previousWord.RevisionInterval, newEF));
                    wordChanges = new Word
                    {
                        Id = previousWord.Id,
                        PersianWord = previousWord.PersianWord,
                        Translation = previousWord.Translation,
                        Difficulty = previousWord.Difficulty,
                        PhotoPath = previousWord.PhotoPath,
                        PronunciationPath = previousWord.PronunciationPath,
                        EF = newEF,
                        Status = WordStatus.InProgress,
                        RevisionsCount = previousWord.RevisionsCount,
                        NextRevision = DateTime.Today.AddDays(newInterval),
                        RevisionInterval = newInterval
                    };
                    _wordRepository.Update(wordChanges);
                }

                Revision revisionChanges = new Revision
                {
                    Id = previousRevisionRow.Id,
                    UserId = previousRevisionRow.UserId,
                    WordId = previousRevisionRow.WordId,
                    WriteAnswer = Answer.AnsweredCorrectly,
                    RevisionDate = wordChanges.NextRevision
                };

                _revisionRepository.Update(revisionChanges);

                var restOfWords = _revisionRepository.GetAllRevisionsPerDay(currentDate).Where(p => p.WriteAnswer != Answer.AnsweredCorrectly).OrderBy(p => p.WriteAnswer).ToList();
                if (restOfWords.Count != 0)
                {
                    return RedirectToAction("revise", "revision", new { id = restOfWords.FirstOrDefault().WordId });
                }
                else
                {
                    return RedirectToAction("index", "home");
                }
            }
            else
            {
                //Incorrect answer
                if (previousWord.RevisionsCount == 0)
                {
                    wordChanges = new Word
                    {
                        Id = previousWord.Id,
                        PersianWord = previousWord.PersianWord,
                        Translation = previousWord.Translation,
                        Difficulty = previousWord.Difficulty,
                        PhotoPath = previousWord.PhotoPath,
                        PronunciationPath = previousWord.PronunciationPath,
                        EF = previousWord.EF,
                        Status = WordStatus.InProgress,
                        RevisionsCount = previousWord.RevisionsCount,
                        NextRevision = DateTime.Today.AddDays(3),
                        RevisionInterval = previousWord.RevisionInterval + 3
                    };
                    _wordRepository.Update(wordChanges);
                }
                else
                {
                    double newEF = FactorsCalculator.CalculateEF(previousWord.EF, 0);
                    int newInterval = Convert.ToInt32(FactorsCalculator.CalculateInterval(previousWord.RevisionInterval, newEF));
                    wordChanges = new Word
                    {
                        Id = previousWord.Id,
                        PersianWord = previousWord.PersianWord,
                        Translation = previousWord.Translation,
                        Difficulty = previousWord.Difficulty,
                        PhotoPath = previousWord.PhotoPath,
                        PronunciationPath = previousWord.PronunciationPath,
                        EF = newEF,
                        Status = WordStatus.InProgress,
                        RevisionsCount = previousWord.RevisionsCount,
                        NextRevision = DateTime.Today.AddDays(newInterval),
                        RevisionInterval = newInterval
                    };
                    _wordRepository.Update(wordChanges);
                }

                Revision revisionChanges = new Revision
                {
                    Id = previousRevisionRow.Id,
                    UserId = previousRevisionRow.UserId,
                    WordId = previousRevisionRow.WordId,
                    WriteAnswer = Answer.AnsweredIncorrectly,
                    RevisionDate = wordChanges.NextRevision
                };

                _revisionRepository.Update(revisionChanges);


                var restOfWords = _revisionRepository.GetAllRevisionsPerDay(currentDate).Where(p => p.WriteAnswer != Answer.AnsweredCorrectly).OrderBy(p => p.WriteAnswer).ToList();
                if (restOfWords.Count != 0)
                {
                    return RedirectToAction("revise", "revision", new { id = restOfWords.FirstOrDefault().WordId });
                }
                else
                {
                    return RedirectToAction("index", "home");
                }
            }
        }
    }
}

