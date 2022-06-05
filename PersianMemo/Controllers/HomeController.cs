using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PersianMemo.Models;
using PersianMemo.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PersianMemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly LanguageService _language;

        private readonly IWordRepository _wordRepository;
        private readonly string wwwrootDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

        public HomeController(IWordRepository wordRepository, ILogger<HomeController> logger, LanguageService language)
        {
            _wordRepository = wordRepository;
            _logger = logger;
            _language = language;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult GetAll()
        {
            var data = _wordRepository.GetAllWords();
            return Json(new { data = data });
        }

        [AllowAnonymous]
        public ViewResult Details(int? id)
        {
            Word word = _wordRepository.GetWord(id.Value);
            if(word == null)
            {
                Response.StatusCode = 404;
                _logger.LogWarning($"404 Error occured. Word with ID = {id.Value} cannot be found ");
                return View("WordNotFound", id.Value);
            }
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Word = _wordRepository.GetWord(id ?? _wordRepository.GetAllWords().FirstOrDefault().Id),
                PageTitle = _language.Getkey("wordDetails")

            };
            return View(homeDetailsViewModel);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(WordCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniquePhotoFileName = ProcessUploadedPhotoFile(model);
                string uniquePronunciationFileName = ProcessUploadedAudioFile(model);

                Word newWord = new Word
                {
                    PersianWord = model.PersianWord,
                    Translation = model.Translation,
                    Difficulty = model.Difficulty,
                    PhotoPath = uniquePhotoFileName,
                    PronunciationPath = uniquePronunciationFileName
                };
                _wordRepository.Add(newWord);
                return RedirectToAction("details", new { id = newWord.Id });
            }
            return View();
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            Word word = _wordRepository.GetWord(id);
            if (word == null)
            {
                Response.StatusCode = 404;
                _logger.LogWarning($"404 Error occured. Word with ID = {id} cannot be found ");
                return View("WordNotFound", id);
            }
            WordEditViewModel wordEditViewModel = new WordEditViewModel
            {
                Id = word.Id,
                PersianWord = word.PersianWord,
                Translation = word.Translation,
                Difficulty = word.Difficulty,
                ExistingPhotoPath = word.PhotoPath,
                ExistingPronunciationPath = word.PronunciationPath

            };
            return View(wordEditViewModel);
        }

        [HttpPost]
        public IActionResult Edit(WordEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Word word = _wordRepository.GetWord(model.Id);
                word.PersianWord = model.PersianWord;
                word.Translation = model.Translation;
                word.Difficulty = model.Difficulty;
                if(model.Photo != null)
                {
                    if(model.ExistingPhotoPath != null)
                    {
                        string existingPhotoFilePath = Path.Combine(wwwrootDirectory, "images", model.ExistingPhotoPath);
                        System.IO.File.Delete(existingPhotoFilePath);
                    }
                    word.PhotoPath = ProcessUploadedPhotoFile(model);
                }
                if (model.Pronunciation != null)
                {
                    if (model.ExistingPronunciationPath != null)
                    {
                        string existingAudioFilePath = Path.Combine(wwwrootDirectory, "images", model.ExistingPronunciationPath);
                        System.IO.File.Delete(existingAudioFilePath);
                    }
                    word.PronunciationPath = ProcessUploadedAudioFile(model);
                }
                Word updatedWord = _wordRepository.Update(word);
                return RedirectToAction("index");
            }
            return View(model);
        }

        public IActionResult Delete(int? id)
        {
            if (ModelState.IsValid)
            {
                Word word = _wordRepository.GetWord(id.Value);
                if (word.PhotoPath != null)
                {
                    string existingPhotoFilePath = Path.Combine(wwwrootDirectory, "images", word.PhotoPath);
                    System.IO.File.Delete(existingPhotoFilePath);
                }
                if (word.PronunciationPath != null)
                {
                    string existingAudioFilePath = Path.Combine(wwwrootDirectory, "audio", word.PronunciationPath);
                    System.IO.File.Delete(existingAudioFilePath);
                }
                Word deletedWord = _wordRepository.Delete(id.Value);
            }
            return RedirectToAction(nameof(Index));
        }

        private string ProcessUploadedAudioFile(WordCreateViewModel model)
        {
            string uniquePronunciationFileName = null;

            if (model.Pronunciation != null)
            {
                // Pronunciation upload
                string audioUploadsFolder = Path.Combine(wwwrootDirectory, "audio");
                uniquePronunciationFileName = $"{Guid.NewGuid().ToString()}_{model.Pronunciation.FileName}";
                string pronunciationFilePath = Path.Combine(audioUploadsFolder, uniquePronunciationFileName);
                using (var fileStream = new FileStream(pronunciationFilePath, FileMode.Create))
                {
                    model.Pronunciation.CopyTo(fileStream);
                }
            }

            return uniquePronunciationFileName;
        }

        private string ProcessUploadedPhotoFile(WordCreateViewModel model)
        {
            string uniquePhotoFileName = null;

            if (model.Photo != null)
            {
                // Photo upload
                string imagesUploadsFolder = Path.Combine(wwwrootDirectory, "images");
                uniquePhotoFileName = $"{Guid.NewGuid().ToString()}_{model.Photo.FileName}";
                string photoFilePath = Path.Combine(imagesUploadsFolder, uniquePhotoFileName);
                using (var fileStream = new FileStream(photoFilePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }
            }

            return uniquePhotoFileName;
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
