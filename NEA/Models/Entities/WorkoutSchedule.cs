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
        public WorkoutSchedule(Workout[] workouts, DateTime date)
        {
            Workouts = workouts;
            Date = date;

        }
        public void UpdateWorkouts(Workout[] workouts)
        {
            Workouts = workouts;
        }
        
        public Workout[] GetWorkouts()
        {
            return Workouts;
        }
    }
}
