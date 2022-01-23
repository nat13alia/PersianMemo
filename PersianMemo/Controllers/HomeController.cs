﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PersianMemo.Models;
using PersianMemo.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PersianMemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IWordRepository _wordRepository;

        public HomeController(IWordRepository wordRepository, ILogger<HomeController> logger)
        {
            _wordRepository = wordRepository;
            _logger = logger;
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
                Word = _wordRepository.GetWord(id ?? 1),
                PageTitle = "Word Details"

            };
            return View(homeDetailsViewModel);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Word word)
        {
            if (ModelState.IsValid)
            {
                Word newWord = _wordRepository.Add(word);
            }
            return View();
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
