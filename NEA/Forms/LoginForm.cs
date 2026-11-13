using NEA.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace NEA.Forms
{
    public partial class LoginForm : Form
    {
        string usernameOrEmail;
        string password;

        public LoginForm()
        {
            InitializeComponent();
            lblError.Visible = false;
        }

        private void txtEmailUsername_TextChanged(object sender, EventArgs e)
        {
            usernameOrEmail = txtEmailUsername.Text;
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            password = txtPassword.Text;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string validity = AcountManager.isValidLoginDetails(usernameOrEmail, password);

            if (validity == "Valid")
            {
                // Successfully logged in
                Console.WriteLine("logged in");
                // Open up main menu (not been implemented yet)    
            }
            else
            {
                lblError.Visible = true;
                lblError.Text = validity;
            }
        }
    }
}
