using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PersianMemo.Models;
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
        
        public IActionResult Index(int[] ids)
        {
            var model = new List<Word>();
            foreach (int id in ids)
            {
                var word = _wordRepository.GetWord(id);
                model.Add(word);
            }

            return View(model);
        }

        public IActionResult SaveExercise(int[] ids)
        {
            List<Word> words = new List<Word>();

            foreach (int id in ids)
            {
                var word = _wordRepository.GetWord(id);
                words.Add(word);
            }

            var exercise = new Exercise
            {
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            };

            _exerciseRepository.Add(exercise);

            foreach (Word w in words)
            {
                var exerciseWordPair = new ExercisesWords
                {
                    ExerciseId = exercise.Id,
                    WordId = w.Id
                };
                _exercisesWordsRepository.Add(exerciseWordPair);
            }

            return RedirectToAction("exercise", "index");
        }
    }
}
