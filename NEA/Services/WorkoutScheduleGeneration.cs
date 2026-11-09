using NEA.Models.Entities;
using NEA.Properties;
using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Windows.Forms;
using System.Xml.Schema;
using System.Linq;
using static NEA.Services.ApplicationClock;

namespace NEA.Services
{
    public static class WorkoutScheduleGeneration  
    {
        private static DatabaseManager db;

        public static void SendDatabase(DatabaseManager DB)
        {
            db = DB;
        }

        private static readonly Func<int, double> CalculateMaximumVariation = regenerationCount => Math.Min(1.5, 0.5 + regenerationCount * 0.25);

        private static Dictionary<DateTime, WorkoutType> CalculateTimes(Dictionary<DateTime, WorkoutType> availableTimes)
        {
            Dictionary<DateTime, WorkoutType> NewTimes = new Dictionary<DateTime, WorkoutType>(availableTimes);

            foreach (var timeSlot in availableTimes)
            {
                if (timeSlot.Key <= DateTime.Now)
                {
                    NewTimes.Remove(timeSlot.Key);
                }
            }

            return NewTimes;
        }

        public static void GenerateWorkoutSchedule(User user, Dictionary<DateTime, WorkoutType> availableTimes)
        {
            UserSettings userSettings = user.GetSettings();
            double maximumVariance = CalculateMaximumVariation(userSettings.RegenerationCount);

            Dictionary<DateTime, WorkoutType> workoutSlots = CalculateTimes(availableTimes);

            WorkoutSchedule currentWorkoutSchedule = user.CurrentWorkoutSchedule != null ? user.CurrentWorkoutSchedule : null; ;
            Dictionary<DateTime, WorkoutType> completedWorkoutSlots =  user.CurrentWorkoutSchedule != null ? user.CurrentWorkoutSchedule.ConvertToWorkoutSlots()  : null;
  
            if (currentWorkoutSchedule != null)
            {
                currentWorkoutSchedule = user.CurrentWorkoutSchedule;
                completedWorkoutSlots = new Dictionary<DateTime, WorkoutType>();
                          
                foreach (var workout in currentWorkoutSchedule.Workouts)
                {
                    if (workout.Completed)
                    {
                        completedWorkoutSlots[workout.Date] = workout.Workout_WorkoutType;
                    }
                }
            }
            workoutSlots = WorkoutTypeSelection.RunAlgorithm(user, workoutSlots, completedWorkoutSlots, maximumVariance);
            userSettings.RegenerationCount += 1;
            user.UpdateSettings(userSettings);
            Workout[] workouts = new Workout[workoutSlots.Count];

            foreach (var (slot, index) in workoutSlots.Select((value, i) => (value, i)))
            {
                workouts[i].
            }

            WorkoutSchedule workoutSchedule = new WorkoutSchedule();
            foreach (var workout in workoutSlots)
            {
                db.AddWorkout(user.UserID, GetCurrentDate(), false, );
            }
        }
    }
}
