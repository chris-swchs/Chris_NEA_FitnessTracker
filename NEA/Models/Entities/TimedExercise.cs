using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA.Models.Entities
{
    public class TimedExercise : Exercise
    {
        public Set[] Sets { get; private set; }

        public TimedExercise(string exerciseName, string exerciseDescription, ExerciseType exerciseType, MuscleGroup targetMuscle, Set[] sets)
            : base(exerciseName, exerciseDescription, exerciseType, targetMuscle)
        {
            Sets = sets;
        }

        public int GetCompletedSetCount()
        {
            int completedCount = 0;
            foreach (Set set in Sets)
            {
                if (set.Completed == true)
                {
                    completedCount++;
                }
            }
            return completedCount;
        }

        public int GetEstimatedDuration()
        {
            float duration = 0;
            foreach (Set set in Sets)
            {
                set.Reps
            }
            return completedCount;
        }
    }
}
