using Microsoft.AspNetCore.Http;
using PersianMemo.Models;
using System.ComponentModel.DataAnnotations;

namespace PersianMemo.ViewModels
{
    public class WordCreateViewModel
    {
        [Required(ErrorMessage = "Please insert the word")]
        [MaxLength(50, ErrorMessage = "Entered value cannot exceed 50 characters")]
        public string PersianWord { get; set; }
        [MaxLength(150, ErrorMessage = "Entered value cannot exceed 50 characters")]
        [Required(ErrorMessage = "Please insert translation")]
        public string Translation { get; set; }
        [MaxLength(150, ErrorMessage = "Entered value cannot exceed 50 characters")]
        public string PersianSynonym { get; set; }
        [Required(ErrorMessage = "Please select the difficulty")]
        public Difficulty? Difficulty { get; set; }
        public IFormFile Photo { get; set; }
        public IFormFile Pronunciation { get; set; }
        public WordStatus Status { get; set; }
        public double EF { get; set; }
    }
}
