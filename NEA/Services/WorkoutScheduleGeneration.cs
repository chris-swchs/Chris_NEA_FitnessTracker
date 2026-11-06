using NEA.Models.Entities;
using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Windows.Forms;
using System.Xml.Schema;

namespace NEA.Services
{
    public static class WorkoutScheduleGeneration  
    {
        private static DatabaseManager db;

        public static void SendDatabase(DatabaseManager DB)
        {
            db = DB;
        }

        private static Dictionary<DateTime, WorkoutType> CalculateTimes(WorkoutSchedule workoutSchedule, Dictionary<DateTime, WorkoutType> availableTimes)
        {
            DateTime latestDate = workoutSchedule.GetLatestCompletedWorkout();

            if (latestDate != DateTime.MinValue)
            {
                foreach (var timeSlot in availableTimes)
                {
                    if (timeSlot.Key <= latestDate)
                    {

                    }
                }
            }
        }

        public static void GenerateWorkoutSchedule(User user, Dictionary<DateTime, WorkoutType> availableTimes)
        {
            // TO DO: Implement function which determines which available days should have a workout placed in it
            Dictionary<DateTime, WorkoutType> workoutSlots = availableTimes;

            WorkoutSchedule currentWorkoutSchedule = user.CurrentWorkoutSchedule != null ? user.CurrentWorkoutSchedule : null; ;
            Dictionary<DateTime, WorkoutType> completedWorkoutSlots;

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
            workoutSlots = WorkoutTypeSelection.RunAlgorithm(user, workoutSlots, completedWorkoutSlots);
        }
    }
}
