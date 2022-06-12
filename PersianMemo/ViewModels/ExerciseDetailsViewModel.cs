using PersianMemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersianMemo.ViewModels
{
    public class ExerciseDetailsViewModel
    {
        public int ExerciseId { get; set; }
        public IEnumerable<Word> WordsList { get; set; }
        public Word CurrentWord { get; set; }
    }
}
