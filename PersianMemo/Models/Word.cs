using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PersianMemo.Models
{
    public class Word
    {
        public Word()
        {
            Status = WordStatus.NotStarted;
            EF = 2.5;
        }
        public int Id { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Entered value cannot exceed 50 characters")]
        public string PersianWord { get; set; }
        [MaxLength(50, ErrorMessage = "Entered value cannot exceed 50 characters")]
        public string Translation { get; set; }
        [Required(ErrorMessage = "Please select the difficulty")]
        public Difficulty? Difficulty { get; set; }
        public string PhotoPath { get; set; }
        public string PronunciationPath { get; set; }

        public WordStatus Status { get; set; }
        public double EF { get; set; }
    }
}
