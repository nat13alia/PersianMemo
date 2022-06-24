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
        private readonly IWordRepository _wordRepository;

        public SQLExercisesWordsRepository(AppDbContext context, IWordRepository wordRepository)
        {
            this.context = context;
            _wordRepository = wordRepository;
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
                    words.Add(_wordRepository.GetWord(pair.WordId));
                }
            }
            return words;
        }

        public ExercisesWords GetPair(int exerciseId, int wordId)
        {
            var pairs = context.ExercisesWords.ToList();
            return pairs.Find(pair => pair.ExerciseId == exerciseId && pair.WordId == wordId);
        }

        public IEnumerable<ExercisesWords> GetAllPairsForExercise(int exerciseId)
        {
            return context.ExercisesWords.Where(p => p.ExerciseId == exerciseId).ToList();
        }

        public ExercisesWords Update(ExercisesWords pairChanges)
        {
            context.ChangeTracker.Clear();
            var pair = context.ExercisesWords.Attach(pairChanges);
            pair.State = EntityState.Modified;
            context.SaveChanges();
            return pairChanges;
        }

        public ExercisesWords Delete(int Id)
        {
            ExercisesWords pair = context.ExercisesWords.Find(Id);
            if (pair != null)
            {
                context.ExercisesWords.Remove(pair);
                context.SaveChanges();
            }
            return pair;
        }
    }
}
