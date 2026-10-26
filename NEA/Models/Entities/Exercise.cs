using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA.Models.Entities
{
    public class Exercise
    {
        public int ExerciseID { get; set; }
        public string ExerciseName { get; set; }
        public string ExerciseDescription { get; set; }
        ExerciseType ExerciseType { get; set; }
        MuscleGroup TargetMuscle {  get; set; }
        public bool Completed { get; private set; }
        public int RestDuration { get; set; }

        public Exercise(string exerciseName, string exerciseDescription, ExerciseType exerciseType, MuscleGroup targetMuscle, int restDuration)
        {
            ExerciseName = exerciseName;
            ExerciseDescription = exerciseDescription;
            ExerciseType = exerciseType;
            TargetMuscle = targetMuscle;
            RestDuration = restDuration;
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
