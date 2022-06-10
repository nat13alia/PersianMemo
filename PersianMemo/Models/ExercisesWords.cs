using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace PersianMemo.Models
{
    public class ExercisesWords
    {
        [Key]
        public int Id { get; set; }
        public int ExerciseId { get; set; }
        [ForeignKey("ExerciseId")]
        public Exercise Exercise { get; set; }
        public int WordId { get; set; }
        [ForeignKey("WordId")]
        public Word Word { get; set; }
    }
}
