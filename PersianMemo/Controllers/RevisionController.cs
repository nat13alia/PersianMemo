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
    public class RevisionController : Controller
    {
        //    if(previousWord.RevisionsCount == 0)
        //            {
        //                Word wordChanges = new Word
        //                {
        //                    Id = previousWord.Id,
        //                    PersianWord = previousWord.PersianWord,
        //                    Translation = previousWord.Translation,
        //                    Difficulty = previousWord.Difficulty,
        //                    PhotoPath = previousWord.PhotoPath,
        //                    PronunciationPath = previousWord.PronunciationPath,
        //                    EF = previousWord.EF,
        //                    Status = WordStatus.InProgress,
        //                    RevisionsCount = previousWord.RevisionsCount,
        //                    NextRevision = DateTime.Today.AddDays(1)
        //                };
        //}
        //private readonly LanguageService _language;

        //    private readonly IExerciseRepository _exerciseRepository;
        //    private readonly IWordRepository _wordRepository;
        //    private readonly UserManager<IdentityUser> _userManager;
        //    private readonly IExercisesWordsRepository _exercisesWordsRepository;

        //    public ExerciseController(IExerciseRepository exerciseRepository, LanguageService language, IWordRepository wordRepository, UserManager<IdentityUser> userManager, IExercisesWordsRepository exercisesWordsRepository)
        //    {
        //        _exerciseRepository = exerciseRepository;
        //        _wordRepository = wordRepository;
        //        _userManager = userManager;
        //        _exercisesWordsRepository = exercisesWordsRepository;
        //        _language = language;
        //    }

        //    [HttpGet]
        //    public IActionResult Details(int id)
        //    {
        //        var model = new ExerciseDetailsViewModel
        //        {
        //            WordsList = _exercisesWordsRepository.GetWords(id),
        //            ExerciseId = id            
        //        };

        //        return View(model);
        //    }

        //    [HttpGet]
        //    public IActionResult LearnListen(int id, int wordId)
        //    {
        //        LearnListenViewModel model = new LearnListenViewModel
        //        {
        //            Exercise = _exerciseRepository.GetExercise(id),
        //            Words = _exercisesWordsRepository.GetWords(id),
        //            CurrentWordId = wordId,
        //            CurrentExerciseId = id
        //        };
        //        return View(model);
        //    }

        //    [HttpPost]
        //    public IActionResult LearnListen(LearnListenViewModel model)
        //    {
        //        var previousExWordpair = _exercisesWordsRepository.GetPair(model.CurrentExerciseId, model.CurrentWordId);
        //        previousExWordpair.DidListen = true;
        //        _exercisesWordsRepository.Update(previousExWordpair);


        //        var restOfWords = _exercisesWordsRepository.GetAllPairsForExercise(model.CurrentExerciseId).Where(p => p.DidListen == false).ToList();
        //        if(restOfWords.Count != 0)
        //        {
        //            return RedirectToAction("learnlisten", "exercise", new { id = model.CurrentExerciseId, wordId = restOfWords.FirstOrDefault().WordId });
        //        } else
        //        {
        //            return RedirectToAction("learnwrite", "exercise", new { id = model.CurrentExerciseId, wordId = _exercisesWordsRepository.GetAllPairsForExercise(model.CurrentExerciseId).FirstOrDefault().WordId });
        //        }
        //    }

        //    [HttpGet]
        //    public IActionResult LearnWrite(int id, int wordId)
        //    {
        //        LearnWriteViewModel model = new LearnWriteViewModel
        //        {
        //            Exercise = _exerciseRepository.GetExercise(id),
        //            Words = _exercisesWordsRepository.GetWords(id),
        //            CurrentWordId = wordId,
        //            CurrentExerciseId = id
        //        };
        //        return View(model);
        //    }

        //    [HttpPost]
        //    public IActionResult LearnWrite(LearnWriteViewModel model)
        //    {
        //        var previousExWordpair = _exercisesWordsRepository.GetPair(model.CurrentExerciseId, model.CurrentWordId);
        //        var previousWord = _wordRepository.GetWord(model.CurrentWordId);

        //        var correctAnswer = _wordRepository.GetWord(model.CurrentWordId).PersianWord;

        //        if (model.Answer == correctAnswer)
        //        {
        //            //Correct answer
        //            ExercisesWords pairChanges = new ExercisesWords
        //            {
        //                Id = previousExWordpair.Id,
        //                ExerciseId = previousExWordpair.ExerciseId,
        //                WordId = previousExWordpair.WordId,
        //                DidListen = previousExWordpair.DidListen,
        //                WriteAnswer = Answer.AnsweredCorrectly
        //            };
        //            _exercisesWordsRepository.Update(pairChanges);

        //            Word wordChanges = new Word
        //            {
        //                Id = previousExWordpair.Id,
        //                ExerciseId = previousExWordpair.ExerciseId,
        //                WordId = previousExWordpair.WordId,
        //                DidListen = previousExWordpair.DidListen,
        //                WriteAnswer = Answer.AnsweredCorrectly
        //            };

        //            var restOfWords = _exercisesWordsRepository.GetAllPairsForExercise(model.CurrentExerciseId).Where(p => p.WriteAnswer != Answer.AnsweredCorrectly).OrderBy(p => p.WriteAnswer).ToList();
        //            if (restOfWords.Count != 0)
        //            {
        //                return RedirectToAction("learnwrite", "exercise", new { id = model.CurrentExerciseId, wordId = restOfWords.FirstOrDefault().WordId });
        //            }
        //            else
        //            {
        //                return RedirectToAction("index", "home");
        //            }
        //        }
        //        else
        //        {
        //            //Incorrect answer
        //            ExercisesWords pairChanges = new ExercisesWords
        //            {
        //                Id = previousExWordpair.Id,
        //                ExerciseId = previousExWordpair.ExerciseId,
        //                WordId = previousExWordpair.WordId,
        //                DidListen = previousExWordpair.DidListen,
        //                WriteAnswer = Answer.AnsweredIncorrectly
        //            };

        //            _exercisesWordsRepository.Update(pairChanges);
        //            var restOfWords = _exercisesWordsRepository.GetAllPairsForExercise(model.CurrentExerciseId).Where(p => p.WriteAnswer != Answer.AnsweredCorrectly).OrderBy(p => p.WriteAnswer).ToList();
        //            if (restOfWords.Count != 0)
        //            {
        //                return RedirectToAction("learnwrite", "exercise", new { id = model.CurrentExerciseId, wordId = restOfWords.FirstOrDefault().WordId });
        //            }
        //            else
        //            {
        //                return RedirectToAction("index", "home");
        //            }
        //        }
        //    }
    }
}
