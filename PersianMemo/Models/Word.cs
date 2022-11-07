using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
            RevisionsCount = 0;
            RevisionInterval = 1;
        }
        public int Id { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public IdentityUser User { get; set; }
        [Required(ErrorMessage = "Please insert the word")]
        [MaxLength(50, ErrorMessage = "Entered value cannot exceed 50 characters")]
        public string PersianWord { get; set; }
        [MaxLength(150, ErrorMessage = "Entered value cannot exceed 50 characters")]
        public string PersianSynonym { get; set; }
        [MaxLength(150, ErrorMessage = "Entered value cannot exceed 50 characters")]
        public string Translation { get; set; }
        [Required(ErrorMessage = "Please select the difficulty")]
        public Difficulty? Difficulty { get; set; }
        public string PhotoPath { get; set; }
        public string PronunciationPath { get; set; }
        public WordStatus Status { get; set; }
        public double EF { get; set; }
        public int RevisionsCount { get; set; }
        public DateTime NextRevision { get; set; }
        public int RevisionInterval { get; set; }
    }
}
