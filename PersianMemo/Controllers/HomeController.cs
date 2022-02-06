﻿using Microsoft.AspNetCore.Hosting;
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

        public IActionResult Index()
        {
            var model = _wordRepository.GetAllWords();
            return View(model);
        }

        public ViewResult Details(int? id)
        {
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
                string uniquePhotoFileName = null;
                string uniquePronunciationFileName = null;
                if (model.Photo != null)
                {
                    // Photo upload
                    string imagesUploadsFolder = Path.Combine(wwwrootDirectory, "images");
                    uniquePhotoFileName = $"{Guid.NewGuid().ToString()}_{model.Photo.FileName}";
                    string photoFilePath = Path.Combine(imagesUploadsFolder, uniquePhotoFileName);
                    model.Photo.CopyTo(new FileStream(photoFilePath, FileMode.Create));
                }

                if (model.Pronunciation != null)
                {
                    // Pronunciation upload
                    string audioUploadsFolder = Path.Combine(wwwrootDirectory, "audio");
                    uniquePronunciationFileName = $"{Guid.NewGuid().ToString()}_{model.Pronunciation.FileName}";
                    string pronunciationFilePath = Path.Combine(audioUploadsFolder, uniquePronunciationFileName);
                    model.Pronunciation.CopyTo(new FileStream(pronunciationFilePath, FileMode.Create));
                }
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

        public IActionResult ChangeLanguage(string culture)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions() { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
