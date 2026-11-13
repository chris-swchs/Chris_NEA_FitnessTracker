using NEA.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NEA
{
    public partial class StartupForm : Form
    {
        public StartupForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            using (LoginForm frm = new LoginForm())
            {
                this.Hide();

                try
                {
                    frm.ShowDialog(this);
                }
                finally
                {
                    this.Show();
                }
            }
        }

        private void btnCreateAcount_Click(object sender, EventArgs e)
        {
            using (SignupForm frm = new SignupForm())
            {
                this.Hide();

                try
                {
                    frm.ShowDialog(this);
                }
                finally
                {
                    this.Show();
                }
            }
        }
    }
}
