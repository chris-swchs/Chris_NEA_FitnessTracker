using NEA.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static NEA.Services.ApplicationClock;

namespace NEA.Services
{
    public static class AcountManager
    {
        private static DatabaseManager db;

        public static void SendDatabase(DatabaseManager DB)
        {
            db = DB;
        }

        public static void CreateAcount(string username, string email, string password)
        {
           // TO DO: perform password text -> hash conversion

           db.AddUser(username, email, password, ApplicationClock.GetCurrentDate());
        }

        public static string isValidLoginDetails (string usernameOrEmail, string password)
        {
           bool valid = db.FindUsernameWithPassword(usernameOrEmail, password);
           valid = !valid ? db.FindEmailWithPassword(usernameOrEmail, password) : valid;

            return valid ? "Valid" : "Invalid details. Please try again";
        }
    }
}