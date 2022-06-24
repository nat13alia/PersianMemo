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

namespace PersianMemo.Controllers
{
    public class ExerciseController : Controller
    {

        private readonly LanguageService _language;

        private readonly IExerciseRepository _exerciseRepository;
        private readonly IWordRepository _wordRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IExercisesWordsRepository _exercisesWordsRepository;
        private readonly IRevisionRepository _revisionRepository;

        public ExerciseController(IExerciseRepository exerciseRepository, LanguageService language, IWordRepository wordRepository, UserManager<IdentityUser> userManager, IExercisesWordsRepository exercisesWordsRepository, IRevisionRepository revisionRepository)
        {
            _exerciseRepository = exerciseRepository;
            _wordRepository = wordRepository;
            _userManager = userManager;
            _exercisesWordsRepository = exercisesWordsRepository;
            _revisionRepository = revisionRepository;
            _language = language;
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var model = new ExerciseDetailsViewModel
            {
                WordsList = _exercisesWordsRepository.GetWords(id),
                ExerciseId = id            
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult LearnListen(int id, int wordId)
        {
            LearnListenViewModel model = new LearnListenViewModel
            {
                Exercise = _exerciseRepository.GetExercise(id),
                Words = _exercisesWordsRepository.GetWords(id),
                CurrentWordId = wordId,
                CurrentExerciseId = id
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult LearnListen(LearnListenViewModel model)
        {
            var previousExWordpair = _exercisesWordsRepository.GetPair(model.CurrentExerciseId, model.CurrentWordId);
            previousExWordpair.DidListen = true;
            _exercisesWordsRepository.Update(previousExWordpair);


            var restOfWords = _exercisesWordsRepository.GetAllPairsForExercise(model.CurrentExerciseId).Where(p => p.DidListen == false).ToList();
            if(restOfWords.Count != 0)
            {
                return RedirectToAction("learnlisten", "exercise", new { id = model.CurrentExerciseId, wordId = restOfWords.FirstOrDefault().WordId });
            } else
            {
                return RedirectToAction("learnwrite", "exercise", new { id = model.CurrentExerciseId, wordId = _exercisesWordsRepository.GetAllPairsForExercise(model.CurrentExerciseId).FirstOrDefault().WordId });
            }
        }

        [HttpGet]
        public IActionResult LearnWrite(int id, int wordId, string? jsToRun)
        {
            LearnWriteViewModel model = new LearnWriteViewModel
            {
                Exercise = _exerciseRepository.GetExercise(id),
                Words = _exercisesWordsRepository.GetWords(id),
                CurrentWordId = wordId,
                CurrentExerciseId = id,
                JavascriptToRun = jsToRun
            };
            if(TempData["audiopath"] != null)
            {
                ViewBag.AudioPath = TempData["audiopath"].ToString();
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LearnWrite(LearnWriteViewModel model)
        {
            var previousExWordpair = _exercisesWordsRepository.GetPair(model.CurrentExerciseId, model.CurrentWordId);
            var previousWord = _wordRepository.GetWord(model.CurrentWordId);
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var correctAnswer = _wordRepository.GetWord(model.CurrentWordId).PersianWord;

            if (model.Answer == correctAnswer)
            {
                //Correct answer
                ExercisesWords pairChanges = new ExercisesWords
                {
                    Id = previousExWordpair.Id,
                    ExerciseId = previousExWordpair.ExerciseId,
                    WordId = previousExWordpair.WordId,
                    DidListen = previousExWordpair.DidListen,
                    WriteAnswer = Answer.AnsweredCorrectly
                };
                _exercisesWordsRepository.Update(pairChanges);

                Word wordChanges = _wordRepository.GetWord(model.CurrentWordId);

                // check if revision exists for this word already from the past
                var existingRevision = _revisionRepository.GetAllRevisionsPerUser(currentUserId).Where(r => r.WordId == model.CurrentWordId).FirstOrDefault();
                if(existingRevision == null)
                {
                    wordChanges.Status = WordStatus.InProgress;
                    wordChanges.RevisionsCount = previousWord.RevisionsCount;
                    wordChanges.NextRevision = DateTime.Today.AddDays(1);
                    wordChanges.RevisionInterval = previousWord.RevisionInterval;

                    _wordRepository.Update(wordChanges);

                    Revision revision = new Revision
                    {
                        UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                        WordId = previousWord.Id,
                        WriteAnswer = Answer.NotAnsweredYet,
                        RevisionDate = wordChanges.NextRevision,
                        ModifiedDate = DateTime.Now
                    };
                    _revisionRepository.Add(revision);
                } else
                {
                    wordChanges.EF = 2.5;
                    wordChanges.Status = WordStatus.InProgress;
                    wordChanges.RevisionsCount = 0;
                    wordChanges.NextRevision = DateTime.Today.AddDays(1);
                    wordChanges.RevisionInterval = 1;

                    _wordRepository.Update(wordChanges);

                    existingRevision.WriteAnswer = Answer.NotAnsweredYet;
                    existingRevision.RevisionDate = wordChanges.NextRevision;
                    existingRevision.ModifiedDate = DateTime.Now;
                    _revisionRepository.Update(existingRevision);
                }

                var restOfWords = _exercisesWordsRepository.GetAllPairsForExercise(model.CurrentExerciseId).Where(p => p.WriteAnswer != Answer.AnsweredCorrectly).OrderBy(p => p.WriteAnswer).ToList();
                if (restOfWords.Count != 0)
                {
                    TempData["audiopath"] = $"/audio/{_wordRepository.GetWord(model.CurrentWordId).PronunciationPath}";
                    return RedirectToAction("learnwrite", "exercise", new { id = model.CurrentExerciseId, wordId = restOfWords.FirstOrDefault().WordId, jsToRun = "ShowSuccessPopup()" });
                }
                else
                {
                    var exWordPairsToRemove = _exercisesWordsRepository.GetAllPairsForExercise(model.CurrentExerciseId);
                    foreach(ExercisesWords pair in exWordPairsToRemove)
                    {
                        _exercisesWordsRepository.Delete(pair.Id);
                    }
                    var exerciseToRemove = _exerciseRepository.Delete(model.CurrentExerciseId);
                    TempData["audiopath"] = $"/audio/{_wordRepository.GetWord(model.CurrentWordId).PronunciationPath}";
                    return RedirectToAction("CompletedExercise");
                }
            }
            else
            {
                //Incorrect answer
                ExercisesWords pairChanges = new ExercisesWords
                {
                    Id = previousExWordpair.Id,
                    ExerciseId = previousExWordpair.ExerciseId,
                    WordId = previousExWordpair.WordId,
                    DidListen = previousExWordpair.DidListen,
                    WriteAnswer = Answer.AnsweredIncorrectly
                };
                _exercisesWordsRepository.Update(pairChanges);

                var restOfWords = _exercisesWordsRepository.GetAllPairsForExercise(model.CurrentExerciseId).Where(p => p.WriteAnswer != Answer.AnsweredCorrectly).OrderBy(p => p.WriteAnswer).ToList();
                TempData["audiopath"] = $"/audio/{_wordRepository.GetWord(model.CurrentWordId).PronunciationPath}";
                return RedirectToAction("learnwrite", "exercise", new { id = model.CurrentExerciseId, wordId = restOfWords.FirstOrDefault().WordId, jsToRun = "ShowErrorPopup()" });
            }
        }

        public IActionResult CompletedExercise()
        {
            if (TempData["audiopath"] != null)
            {
                ViewBag.AudioPath = TempData["audiopath"].ToString();
            }
            return View();
        }
    }
}
