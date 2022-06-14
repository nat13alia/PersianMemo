using PersianMemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersianMemo.ViewModels
{
    public class RevisionDetailsViewModel
    {
        public IEnumerable<Word> WordsList { get; set; }
        public Word CurrentWord { get; set; }
    }
}
