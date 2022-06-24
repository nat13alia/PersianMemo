using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersianMemo.Models
{
    public interface IExerciseRepository
    {
        Exercise GetExercise(int Id);
        IEnumerable<Exercise> GetAllExercises();
        Exercise Add(Exercise exercise);
        Exercise Update(Exercise exerciseChanges);
        Exercise Delete(int Id);
    }
}
