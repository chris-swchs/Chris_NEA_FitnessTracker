using NEA.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static NEA.Services.ApplicationClock;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace NEA.Services
{
    public static class AcountManager
    {
        private static DatabaseManager db;
        private static User user;
        private static bool loggingInWithUsername;

        private static int MinUsernameLength = 3;
        private static int MaxUsernameLength = 20;
        private static int MinPasswordLength = 3;
        private static int MaxPasswordLength = 20;
        private static int MaxEmailLength = 255;

        public static void SendDatabase(DatabaseManager DB)
        {
            db = DB;
        }

        public static void InitialiseLogin(string usernameOrEmail, string password)
        {
            string email = loggingInWithUsername ? db.GetEmailFromUsername(usernameOrEmail) : usernameOrEmail;
            string username = !loggingInWithUsername ? db.GetUsernameFromEmail(usernameOrEmail) : usernameOrEmail;

            user = new User(username, email, password, db.GetUserDateCreated(username));
            // TO DO: Create a UserSettings object for which its settings are retrieved from the database

            Console.WriteLine("Sucessfully logged in");
        }

        public static void CreateAcount(string username, string email, string password)
        {
           // TO DO: perform password text -> hash conversion

           db.AddUser(username, email, password, ApplicationClock.GetCurrentDate());
        }

        public static string isValidLoginDetails (string usernameOrEmail, string password)
        {
           bool valid = db.FindUsernameWithPassword(usernameOrEmail, password);
           loggingInWithUsername = valid;
           valid = !valid ? db.FindEmailWithPassword(usernameOrEmail, password) : valid;
           return valid ? "Valid" : "Invalid details. Please try again";
        }

        public static string UsernameInputValidation(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return "Please enter a username";
            }
            if (input.Length < MinUsernameLength || input.Length > MaxUsernameLength)
            {
                return string.Format("Username must be between {0}-{1} characters", MinUsernameLength, MaxUsernameLength);
            }
            // Also acounts for the rule "must not contain spaces"
            if (!Regex.IsMatch(input, @"^[a-zA-Z0-9_]+$"))
            {
                return "Username can only consist of letters, numbers, and underscores";
            }

            return "Valid";
        }

        public static string EmailInputValidation(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return "Please enter an email";
            }
            if (input.Length >= MaxEmailLength)
            {
                return string.Format("Email must be less than {0} characters", MaxEmailLength);
            }
            if (!Regex.IsMatch(input, @"^[^@\s]+@[^@\s.]+(\.[^@\s.]+)+$"))
            {
                return "Must enter a valid email";
            }
            return "Valid";
        }

        public static string PasswordInputValidation(string input, string username, string email)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return "Please enter a password";
            }
            if (input.Length < MinPasswordLength || input.Length > MaxPasswordLength)
            {
                return string.Format("Password must be between {0}-{1} characters", MinPasswordLength, MaxPasswordLength);
            }
            if (input == username || input == email)
            {
                return "Password must not be identical to username or email";
            }
            return "Valid";
        }

        public static string PerformValidation(string username,  string email, string password)
        {
            string CurrentValidation = UsernameInputValidation(username);

            if (CurrentValidation != "Valid")
            {
                return CurrentValidation;
            }
            CurrentValidation = EmailInputValidation(email);
            if (CurrentValidation != "Valid")
            {
                return CurrentValidation;
            }
            CurrentValidation = PasswordInputValidation(password, username, email);
            if (CurrentValidation != "Valid")
            {
                return CurrentValidation;
            }
            return CurrentValidation;
        }

    }
}