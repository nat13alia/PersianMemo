using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersianMemo.Models
{
    public interface IExercisesWordsRepository
    {
        IEnumerable<Word> GetWords(int exerciseId);
        ExercisesWords Add(ExercisesWords exerciseWordPair);
    }
}
