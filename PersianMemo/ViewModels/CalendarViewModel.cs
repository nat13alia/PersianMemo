using Microsoft.AspNetCore.Http;
using PersianMemo.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PersianMemo.ViewModels
{
    public class CalendarViewModel
    {
        public IEnumerable<Word> Words { get; set; }
    }
}
