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
        public static void GenerateWorkoutSchedule(User user, Dictionary<DateTime, WorkoutType> availableTimes)
        {  
            // TO DO: Implement function which determines which available days should have a workout placed in it
            Dictionary<DateTime, WorkoutType> workoutSlots = availableTimes;

            WorkoutSchedule currentWorkoutSchedule = user.CurrentWorkoutSchedule != null ? user.CurrentWorkoutSchedule : null; ;

            if (currentWorkoutSchedule != null)
            {
                currentWorkoutSchedule = user.CurrentWorkoutSchedule;
                foreach(var workout in currentWorkoutSchedule.Workouts)
                {
                    if (workout.Completed)
                    {

                    }
                }
            }
            workoutSlots = WorkoutTypeSelection.RunAlgorithm(user, workoutSlots);
        }
    }
}
