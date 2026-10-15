using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA.Models.Entities
{
    public class Exercise
    {
        public string ExerciseName { get; set; }
        public string ExerciseDescription { get; set; }
        ExerciseType ExerciseType { get; set; }
        MuscleGroup TargetMuscle {  get; set; }
        public bool Completed { get; private set; }

        public Exercise(string exerciseName, string exerciseDescription, ExerciseType exerciseType, MuscleGroup targetMuscle)
        {
            ExerciseName = exerciseName;
            ExerciseDescription = exerciseDescription;
            ExerciseType = exerciseType;
            TargetMuscle = targetMuscle;
        }


        public void MarkCompleted()
        {
            Completed = true;
        }

        public virtual float GetEstimatedDuration()
        {
            return 0;
        }
    }
}
