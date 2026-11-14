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
        public SignupForm()
        {
            InitializeComponent();
            lblError.Visible = false;
        }

        private string username;
        private string email;
        private string password;


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
            string Validitity = AcountManager.PerformValidation(username, email, password);
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
