using NEA.Models.Entities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NEA.Services
{
    public static class WorkoutGeneration  
    {
        private static DatabaseManager db;

        public static void SendDatabase(DatabaseManager DB)
        {
            db = DB;
        }
        public static void RunAlgorithm(User user, Dictionary<DateTime, WorkoutType> WorkoutSlots)
        {
           
        }
    }
}
