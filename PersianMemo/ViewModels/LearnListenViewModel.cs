using Microsoft.AspNetCore.Http;
using PersianMemo.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PersianMemo.ViewModels
{
    public class LearnListenViewModel
    {
        public Exercise Exercise { get; set; }
        public IEnumerable<Word> Words { get; set; }
        public int CurrentWordId { get; set; }
        public int CurrentExerciseId { get; set; }
    }
}
