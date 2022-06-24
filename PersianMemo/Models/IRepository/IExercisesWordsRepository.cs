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
        ExercisesWords GetPair(int exerciseId, int wordId);
        IEnumerable<ExercisesWords> GetAllPairsForExercise(int exerciseId);
        ExercisesWords Update(ExercisesWords pairChanges);
        ExercisesWords Delete(int Id);

    }
}
