using NEA.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NEA.Models.Entities
{
    public class User
    {
        public string UserID { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public DateTime DateCreated { get; private set; }
        public UserSettings Settings { get; private set; }
        public WorkoutSchedule CurrentWorkoutSchedule { get; private set; }
        public User(string userID, string email, string password, DateTime dateCreated)
        {
            UserID = userID;
            Email = email;
            Password = password;
            DateCreated = dateCreated;
        }

        public void UpdateSettings(UserSettings newSettings)
        {
            Settings = newSettings;
        }

        public void UpdateWorkoutSchedule(WorkoutSchedule newWorkoutSchedule)
        {
            CurrentWorkoutSchedule = newWorkoutSchedule;
        }

        public UserSettings GetSettings()
        {
            return Settings;
        }
    }
}
