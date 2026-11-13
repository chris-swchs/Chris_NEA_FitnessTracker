using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using NEA.Services;

namespace NEA
{
    public partial class SignupForm : Form
    {

        // TO CONSIDER - put the validation functionality into AcountManager, then call it from here

        int MinUsernameLength = 3;
        int MaxUsernameLength = 20;
        int MinPasswordLength = 3;
        int MaxPasswordLength = 20;
        int MaxEmailLength = 255;

        public SignupForm()
        {
            InitializeComponent();
            lblError.Visible = false;
        }

        private string username;
        private string email;
        private string password;

        private string UsernameInputValidation(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return "Please enter a username";
            }
            if (input.Length < MinUsernameLength || input.Length > MaxUsernameLength)
            {
                return string.Format("Username must be between {0}-{1} characters",  MinUsernameLength, MaxUsernameLength);
            }
            // Also acounts for the rule "must not contain spaces"
            if (!Regex.IsMatch(input, @"^[a-zA-Z0-9_]+$")) 
            {
                return "Username can only consist of letters, numbers, and underscores";
            }

            return "Valid";
        }

        private string EmailInputValidation(string input)
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

        private string PasswordInputValidation(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return "Please enter a password";
            }
            if (input.Length < MinPasswordLength || input.Length > MaxPasswordLength)
            {
                return string.Format("Password must be between {0}-{1} characters", MinPasswordLength, MaxPasswordLength);
            }
            if (input == username ||  input == email)
            {
                return "Password must not be identical to username or email";
            }
            return "Valid";
        }

        private string PerformValidation()
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
            CurrentValidation = PasswordInputValidation(password);
            if (CurrentValidation != "Valid")
            {
                return CurrentValidation;
            }
            return CurrentValidation;
        }

        private void SignupForm_Load(object sender, EventArgs e)
        {

        }
        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            username = txtUsername.Text;
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
           email  = txttEmail.Text;
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            password = txtPassword.Text;
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            string Validitity = PerformValidation();
            if (Validitity == "Valid")
            {
                AcountManager.CreateAcount(username, email, password);
                this.Close();
            }
            else
            {
                lblError.Text = Validitity;
                lblError.Visible = true;
            }
        }
    }
}
