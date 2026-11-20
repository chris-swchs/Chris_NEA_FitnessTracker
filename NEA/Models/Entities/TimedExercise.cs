using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA.Models.Entities
{
    public class TimedExercise : Exercise
    {
        public int Duration { get; set; }
        public double Weight { get; set; }
        public double Speed { get; set; }
        public TimedExercise(string exerciseName, string exerciseDescription, ExerciseType exerciseType, MovementPattern movementPattern, MuscleGroup targetMuscle, int restDuration, double weight, double speed, int duration)
            : base(exerciseName, exerciseDescription, exerciseType, movementPattern, targetMuscle, restDuration)
        {
            Duration = duration;
            Weight = weight;
            Speed = speed;
        }

        public override double GetEstimatedDuration()
        {
            return Duration + RestDuration;
        }
    }
}
