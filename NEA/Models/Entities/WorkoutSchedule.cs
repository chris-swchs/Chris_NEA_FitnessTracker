using NEA.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NEA.Models.Entities
{
    public class WorkoutSchedule
    {
        public Workout[] Workouts { get; private set; }
        public DateTime DateCreated { get; private set; }
        public WorkoutSchedule(Workout[] workouts, DateTime dateCreated)
        {
            Workouts = workouts;
            DateCreated = dateCreated;

        }
        public void UpdateWorkouts(Workout[] workouts)
        {
            Workouts = workouts;
        }

        public Workout[] GetWorkouts()
        {
            return Workouts;
        }

        public DateTime GetLatestCompletedWorkout()
        {
            DateTime latestDate = DateTime.MinValue;
            foreach (var workout in Workouts)
            {
                if (workout.Date > latestDate || workout.Completed)
                {
                    latestDate = workout.Date;
                }
            }
            return latestDate;
        }

        public Dictionary<DateTime, WorkoutType> ConvertToWorkoutSlots()
        {
            Dictionary<DateTime, WorkoutType> completedWorkoutSlots = new Dictionary<DateTime, WorkoutType>();

            foreach (var workout in Workouts)
            {
                if (workout.Completed)
                {
                    completedWorkoutSlots[workout.Date] = workout.Workout_WorkoutType;
                }
            }

            return completedWorkoutSlots;
        }
    }
}
