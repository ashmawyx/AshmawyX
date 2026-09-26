using System;
using System.Drawing;
using System.Windows.Forms;

namespace AshmawyX
{
    public class SettingsForm : Form
    {
        private Label lblDelayFrom;
        private Label lblDelayTo;
        private NumericUpDown numDelayFrom;
        private NumericUpDown numDelayTo;

        private Button btnSave;
        private Button btnClose;

        private MirroringEngine engine;

        public SettingsForm(MirroringEngine eng)
        {
            engine = eng;

            InitializeForm();
            InitializeControls();
            WireEvents();
        }

        private void InitializeForm()
        {
            this.Text = "Settings";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.ClientSize = new Size(280, 240);
            this.BackColor = Color.FromArgb(18, 18, 22); // matte dark
        }

        private void InitializeControls()
        {
            lblDelayFrom = new Label
            {
                Text = "Random Delay From (sec):",
                Location = new Point(10, 20),
                Size = new Size(200, 20),
                ForeColor = Color.White
            };

            numDelayFrom = new NumericUpDown
            {
                Location = new Point(10, 45),
                Size = new Size(100, 22),
                DecimalPlaces = 2,
                Increment = 0.10M,
                Minimum = 0.00M,
                Maximum = 10.00M,
                Value = 0.20M,
                BackColor = Color.FromArgb(30, 30, 36),
                ForeColor = Color.White
            };

            lblDelayTo = new Label
            {
                Text = "Random Delay To (sec):",
                Location = new Point(10, 80),
                Size = new Size(200, 20),
                ForeColor = Color.White
            };

            numDelayTo = new NumericUpDown
            {
                Location = new Point(10, 105),
                Size = new Size(100, 22),
                DecimalPlaces = 2,
                Increment = 0.10M,
                Minimum = 0.00M,
                Maximum = 10.00M,
                Value = 0.80M,
                BackColor = Color.FromArgb(30, 30, 36),
                ForeColor = Color.White
            };

            btnSave = new Button
            {
                Text = "Save",
                Location = new Point(10, 150),
                Size = new Size(120, 30),
                BackColor = Color.FromArgb(40, 160, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSave.FlatAppearance.BorderSize = 0;

            btnClose = new Button
            {
                Text = "Close",
                Location = new Point(150, 150),
                Size = new Size(120, 30),
                BackColor = Color.FromArgb(180, 50, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnClose.FlatAppearance.BorderSize = 0;

            Controls.Add(lblDelayFrom);
            Controls.Add(numDelayFrom);
            Controls.Add(lblDelayTo);
            Controls.Add(numDelayTo);
            Controls.Add(btnSave);
            Controls.Add(btnClose);
        }

        private void WireEvents()
        {
            btnSave.Click += btnSave_Click;
            btnClose.Click += (s, e) => this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            double from = (double)numDelayFrom.Value;
            double to = (double)numDelayTo.Value;

            engine.SetRandomDelay(from, to);

            MessageBox.Show("Settings saved.", "AshmawyX", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
