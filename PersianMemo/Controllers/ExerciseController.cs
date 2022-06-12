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

        public ExerciseController(IExerciseRepository exerciseRepository, LanguageService language, IWordRepository wordRepository, UserManager<IdentityUser> userManager, IExercisesWordsRepository exercisesWordsRepository)
        {
            _exerciseRepository = exerciseRepository;
            _wordRepository = wordRepository;
            _userManager = userManager;
            _exercisesWordsRepository = exercisesWordsRepository;
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
                return RedirectToAction("index", "home");
            }
        }

        [HttpGet]
        public IActionResult LearnWrite(int id, int wordId)
        {
            LearnWriteViewModel model = new LearnWriteViewModel
            {
                Exercise = _exerciseRepository.GetExercise(id),
                Words = _exercisesWordsRepository.GetWords(id),
                CurrentWordId = wordId,
                CurrentExerciseId = id
            };
            return View(model);
        }
        //[HttpPost]
        //public IActionResult LearnWrite(LearnWriteViewModel model)
        //{
        //    var wordExPair = _exercisesWordsRepository.GetPair(model.CurrentExerciseId, model.CurrentWordId);
        //    if (model.Answer == _wordRepository.GetWord(model.CurrentWordId).PersianWord)
        //    {
        //        //Correct answer
        //        _exercisesWordsRepository.Delete(wordExPair.Id);
        //        return RedirectToAction("details", "home", new { id = model.CurrentWordId });
        //    }
        //    else
        //    {
        //        //Incorrect answer
        //        wordExPair.ListenAnswer = Answer.AnsweredIncorrectly;
        //        _exercisesWordsRepository.Update(wordExPair);
        //        return RedirectToAction("details", "home", new { id = model.CurrentWordId });
        //    }
        //}
    }
}
