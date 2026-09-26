using System;
using System.Drawing;
using System.Windows.Forms;

namespace AshmawyX
{
    public class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeForm();
            InitializeControls();
        }

        private void InitializeForm()
        {
            this.Text = "About AshmawyX";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.ClientSize = new Size(280, 240);
            this.BackColor = Color.FromArgb(18, 18, 22); // matte-dark
        }

        private void InitializeControls()
        {
            Label lblTitle = new Label
            {
                Text = "AshmawyX",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 200, 255),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 10),
                Size = new Size(280, 30)
            };

            Label lblVersion = new Label
            {
                Text = "Version 1.0.0",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(180, 180, 190),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 40),
                Size = new Size(280, 20)
            };

            Label lblDev = new Label
            {
                Text = "Developer: Ahmed Ashmawy\nEmail: ashmawyx@gmail.com\nDiscord: AshmawyX",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.White,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 70),
                Size = new Size(280, 60)
            };

            Label lblDesc = new Label
            {
                Text = "Compact dual-account controller\nNeon-dark theme\nAO-only focus\nProfessional-grade usability",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(180, 180, 190),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 140),
                Size = new Size(280, 60)
            };

            Button btnClose = new Button
            {
                Text = "Close",
                Location = new Point(100, 205),
                Size = new Size(80, 26),
                BackColor = Color.FromArgb(40, 120, 220),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => this.Close();

            Controls.Add(lblTitle);
            Controls.Add(lblVersion);
            Controls.Add(lblDev);
            Controls.Add(lblDesc);
            Controls.Add(btnClose);
        }
    }
}
