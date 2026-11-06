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
        public float Weight { get; set; }
        public float Speed { get; set; }
        public TimedExercise(string exerciseName, string exerciseDescription, ExerciseType exerciseType, MuscleGroup targetMuscle, int restDuration, float weight, float speed, float duration)
            : base(exerciseName, exerciseDescription, exerciseType, targetMuscle, restDuration)
        {
            Duration = duration;
            Weight = weight;
            Speed = speed;
        }

        public override float GetEstimatedDuration()
        {
            return Duration + RestDuration;
        }
    }
}
