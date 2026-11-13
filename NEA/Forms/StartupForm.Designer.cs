namespace NEA
{
    partial class StartupForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblHeading = new System.Windows.Forms.Label();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnCreateAcount = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblHeading
            // 
            this.lblHeading.AutoSize = true;
            this.lblHeading.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeading.Location = new System.Drawing.Point(169, 23);
            this.lblHeading.Name = "lblHeading";
            this.lblHeading.Size = new System.Drawing.Size(444, 46);
            this.lblHeading.TabIndex = 0;
            this.lblHeading.Text = "WORKOUT PLANNER";
            // 
            // btnLogin
            // 
            this.btnLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F);
            this.btnLogin.Location = new System.Drawing.Point(273, 140);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(241, 76);
            this.btnLogin.TabIndex = 1;
            this.btnLogin.Text = "Log in";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnCreateAcount
            // 
            this.btnCreateAcount.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F);
            this.btnCreateAcount.Location = new System.Drawing.Point(273, 262);
            this.btnCreateAcount.Name = "btnCreateAcount";
            this.btnCreateAcount.Size = new System.Drawing.Size(241, 76);
            this.btnCreateAcount.TabIndex = 2;
            this.btnCreateAcount.Text = "Create an acount";
            this.btnCreateAcount.UseVisualStyleBackColor = true;
            this.btnCreateAcount.Click += new System.EventHandler(this.btnCreateAcount_Click);
            // 
            // StartupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnCreateAcount);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.lblHeading);
            this.Name = "StartupForm";
            this.Text = "StartupMenu";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblHeading;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnCreateAcount;
    }
}