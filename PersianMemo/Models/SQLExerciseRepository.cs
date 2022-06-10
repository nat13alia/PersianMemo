using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersianMemo.Models
{
    public class SQLExerciseRepository : IExerciseRepository
    {
        private readonly AppDbContext context;
        public SQLExerciseRepository(AppDbContext context)
        {
            this.context = context;
        }
        public Exercise Add(Exercise exercise)
        {
            context.Exercises.Add(exercise);
            context.SaveChanges();
            return exercise;
        }

        public Exercise Delete(int Id)
        {
            Exercise exercise = context.Exercises.Find(Id);
            if (exercise != null)
            {
                context.Exercises.Remove(exercise);
                context.SaveChanges();
            }
            return exercise;
        }

        public IEnumerable<Exercise> GetAllExercises()
        {
            return context.Exercises;
        }

        public Exercise GetExercise(int Id)
        {
            return context.Exercises.Find(Id);
        }

        public Exercise Update(Exercise exerciseChanges)
        {
            var exercise = context.Exercises.Attach(exerciseChanges);
            exercise.State = EntityState.Modified;
            context.SaveChanges();
            return exerciseChanges;
        }
    }
}
