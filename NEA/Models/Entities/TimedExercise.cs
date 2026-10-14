using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA.Models.Entities
{
    public class TimedExercise : Exercise
    {
        public float Duration { get; set; }
        public float PostWorkoutDuration { get; set; }
        public float Weight {  get; set; }
        public float Speed { get; set; }
        public TimedExercise(string exerciseName, string exerciseDescription, ExerciseType exerciseType, MuscleGroup targetMuscle, float postWorkoutDuration,  float  weight, float speed, float duration)
            : base(exerciseName, exerciseDescription, exerciseType, targetMuscle)
        {
            PostWorkoutDuration = postWorkoutDuration;
            Duration = duration;
            Weight = weight;    
            Speed = speed;
        }

        public override float GetEstimatedDuration()
        {
            return Duration + PostWorkoutDuration;
        }
    }
}
