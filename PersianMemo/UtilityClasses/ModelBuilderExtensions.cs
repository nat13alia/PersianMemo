using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersianMemo.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Word>().HasData(
                 new Word
                 {
                     Id = 1,
                     PersianWord = "خونه",
                     Translation = "Dom",
                     Difficulty = Difficulty.Beginner,
                     PhotoPath = null 
                 },
                 new Word
                 {
                     Id = 2,
                     PersianWord = "ایران",
                     Translation = "Iran",
                     Difficulty = Difficulty.Beginner,
                     PhotoPath = null
                 });
        }
    }
}
