using NEA.Models.LinkedLists;
using NEA.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NEA.Models.Entities
{  
    // Workout automatically set as completed at end of day - otherwise when user marks it as completed themselves
    public class Workout
    {
        public WorkoutType Workout_WorkoutType { get; private set; }
        public DateTime Date { get; private set; }
        public CustomLinkedList<Exercise> Exercises { get; private set; }
        public bool Completed { get; private set; }
        public Workout(WorkoutType workoutType, DateTime date, CustomLinkedList<Exercise> exercises, bool completed)
        {
            Workout_WorkoutType = workoutType;
            Date = date;
            Exercises = exercises;
            Completed = completed;
        }
    }
}
