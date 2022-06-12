using Microsoft.AspNetCore.Http;
using PersianMemo.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PersianMemo.ViewModels
{
    public class LearnWriteViewModel
    {
        public Exercise Exercise { get; set; }
        public IEnumerable<Word> Words { get; set; }
        public string Answer { get; set; }
        public int CurrentWordId { get; set; }
        public int CurrentExerciseId { get; set; }
    }
}
