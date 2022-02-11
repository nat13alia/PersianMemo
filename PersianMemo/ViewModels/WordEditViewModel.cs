using Microsoft.AspNetCore.Http;
using PersianMemo.Models;
using System.ComponentModel.DataAnnotations;

namespace PersianMemo.ViewModels
{
    public class WordEditViewModel : WordCreateViewModel
    {
        public int Id { get; set; }
        public string ExistingPhotoPath { get; set; }
        public string ExistingPronunciationPath { get; set; }
    }
}
