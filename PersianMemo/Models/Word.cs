using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PersianMemo.Models
{
    public class Word
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Słówko")]
        [MaxLength(50, ErrorMessage = "Entered value cannot exceed 50 characters")]
        public string PersianWord { get; set; }
        [Display(Name = "Tłumaczenie")]
        [MaxLength(50, ErrorMessage = "Entered value cannot exceed 50 characters")]
        public string Translation { get; set; }
        [Required(ErrorMessage = "Please select the difficulty")]
        [Display(Name = "Trudność")]
        public Difficulty? Difficulty { get; set; }
        public string PhotoPath { get; set; }
    }
}
