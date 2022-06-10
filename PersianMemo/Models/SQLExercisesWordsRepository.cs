using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersianMemo.Models
{
    public class SQLExercisesWordsRepository : IExercisesWordsRepository
    {
        private readonly AppDbContext context;
        public SQLExercisesWordsRepository(AppDbContext context)
        {
            this.context = context;
        }
        public ExercisesWords Add(ExercisesWords exerciseWordPair)
        {
            context.ExercisesWords.Add(exerciseWordPair);
            context.SaveChanges();
            return exerciseWordPair;
        }

        public IEnumerable<Word> GetWords(int exerciseId)
        {
            var pairs = context.ExercisesWords;
            var words = new List<Word>();
            foreach(ExercisesWords pair in pairs)
            {
                if(pair.ExerciseId == exerciseId)
                {
                    words.Add(pair.Word);
                }
            }
            return words;
        }
    }
}
