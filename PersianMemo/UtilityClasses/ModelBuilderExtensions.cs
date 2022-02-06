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
                     PersianWord = "ایران",
                     Translation = "Iran",
                     Difficulty = Difficulty.Beginner,
                     PhotoPath = "bac074b4 - 83ad - 4ec5 - 8b8a - e498fe02e048_Iran.png",
                     PronunciationPath = "d4838478-2fba-4c1c-864d-21c8cc6f941e_pronunciation_fa_ایران.mp3"
                 });
        }
    }
}
