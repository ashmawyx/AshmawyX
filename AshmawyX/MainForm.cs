using System;
using System.Drawing;
using System.Windows.Forms;

namespace AshmawyX
{
    public class MainForm : Form
    {
        // AO window selection
        private ComboBox comboMainWindow;
        private Button btnRefreshMain;
        private ComboBox comboMirrorWindow;
        private Button btnRefreshMirror;

        // Status
        private Label lblLinkedStatus;

        // Control panel
        private Button btnStart;
        private Button btnStop;
        private Button btnPanic;

        // Delay controls
        private Label lblDelayFrom;
        private Label lblDelayTo;
        private NumericUpDown numDelayFrom;
        private NumericUpDown numDelayTo;

        // Mirrored keys manager
        private Label lblMirrorKeys;
        private ListBox listMirrorKeys;
        private Button btnAddKey;
        private Button btnRemoveKey;

        // Footer
        private Label lblHotkeys;
        private Panel panelMirroringLed;
        private Label lblMirroringStatus;

        // Bottom buttons
        private Button btnSettings;
        private Button btnAbout;

        // Internal state
        private bool isMirroringActive = false;

        // Engine modules
        public MirroringEngine engine;
        private GlobalHotkeys hotkeys;
        private KeyCaptureHook keyHook;
        private TrayIntegration tray;

        public MainForm()
        {
            InitializeForm();
            InitializeControls();
            WireEvents();

            // Engine
            engine = new MirroringEngine();

            // Global hotkeys
            hotkeys = new GlobalHotkeys();
            hotkeys.OnStartHotkey += () => btnStart.PerformClick();
            hotkeys.OnStopHotkey += () => btnStop.PerformClick();
            hotkeys.OnPanicHotkey += () => btnPanic.PerformClick();

            // Key capture → mirroring engine
            keyHook = new KeyCaptureHook();
            keyHook.OnKeyCaptured += (vk) => engine.ProcessKey(vk);

            // Tray system
            tray = new TrayIntegration(this);
        }

        private void InitializeForm()
        {
            this.Text = "AshmawyX - Dual Account Controller";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = true;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new Size(280, 240);
            this.BackColor = Color.FromArgb(18, 18, 22); // matte dark
        }

        private void InitializeControls()
        {
            // AO Window selection
            comboMainWindow = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(10, 10),
                Size = new Size(180, 22),
                BackColor = Color.FromArgb(30, 30, 36),
                ForeColor = Color.White
            };

            btnRefreshMain = new Button
            {
                Text = "Refresh",
                Location = new Point(200, 10),
                Size = new Size(70, 22),
                BackColor = Color.FromArgb(40, 120, 220),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnRefreshMain.FlatAppearance.BorderSize = 0;

            comboMirrorWindow = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(10, 40),
                Size = new Size(180, 22),
                BackColor = Color.FromArgb(30, 30, 36),
                ForeColor = Color.White
            };

            btnRefreshMirror = new Button
            {
                Text = "Refresh",
                Location = new Point(200, 40),
                Size = new Size(70, 22),
                BackColor = Color.FromArgb(40, 120, 220),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnRefreshMirror.FlatAppearance.BorderSize = 0;

            lblLinkedStatus = new Label
            {
                Text = "Linked ✓",
                Location = new Point(10, 65),
                Size = new Size(80, 18),
                ForeColor = Color.FromArgb(60, 200, 90)
            };

            // Control panel
            btnStart = new Button
            {
                Text = "Start",
                Location = new Point(10, 90),
                Size = new Size(80, 26),
                BackColor = Color.FromArgb(40, 160, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnStart.FlatAppearance.BorderSize = 0;

            btnStop = new Button
            {
                Text = "Stop",
                Location = new Point(100, 90),
                Size = new Size(80, 26),
                BackColor = Color.FromArgb(180, 50, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnStop.FlatAppearance.BorderSize = 0;

            btnPanic = new Button
            {
                Text = "Panic",
                Location = new Point(190, 90),
                Size = new Size(80, 26),
                BackColor = Color.FromArgb(220, 140, 40),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnPanic.FlatAppearance.BorderSize = 0;

            // Delay controls
            lblDelayFrom = new Label
            {
                Text = "Delay From:",
                Location = new Point(10, 120),
                Size = new Size(80, 18),
                ForeColor = Color.White
            };

            numDelayFrom = new NumericUpDown
            {
                Location = new Point(90, 118),
                Size = new Size(60, 22),
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
                Text = "To:",
                Location = new Point(160, 120),
                Size = new Size(20, 18),
                ForeColor = Color.White
            };

            numDelayTo = new NumericUpDown
            {
                Location = new Point(185, 118),
                Size = new Size(60, 22),
                DecimalPlaces = 2,
                Increment = 0.10M,
                Minimum = 0.00M,
                Maximum = 10.00M,
                Value = 0.80M,
                BackColor = Color.FromArgb(30, 30, 36),
                ForeColor = Color.White
            };

            // Mirrored keys manager
            lblMirrorKeys = new Label
            {
                Text = "Mirrored Keys:",
                Location = new Point(10, 145),
                Size = new Size(100, 18),
                ForeColor = Color.White
            };

            listMirrorKeys = new ListBox
            {
                Location = new Point(10, 165),
                Size = new Size(120, 50),
                BackColor = Color.FromArgb(30, 30, 36),
                ForeColor = Color.White
            };

            btnAddKey = new Button
            {
                Text = "Add Key",
                Location = new Point(140, 165),
                Size = new Size(60, 22),
                BackColor = Color.FromArgb(40, 120, 220),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnAddKey.FlatAppearance.BorderSize = 0;

            btnRemoveKey = new Button
            {
                Text = "Remove",
                Location = new Point(140, 193),
                Size = new Size(60, 22),
                BackColor = Color.FromArgb(180, 50, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnRemoveKey.FlatAppearance.BorderSize = 0;

            // Footer
            lblHotkeys = new Label
            {
                Text = "Hotkeys: F7 / F8 / F9",
                Location = new Point(10, 220),
                Size = new Size(140, 18),
                ForeColor = Color.FromArgb(180, 180, 190)
            };

            panelMirroringLed = new Panel
            {
                Location = new Point(155, 222),
                Size = new Size(10, 10),
                BackColor = Color.FromArgb(80, 80, 80)
            };

            lblMirroringStatus = new Label
            {
                Text = "Mirroring Active",
                Location = new Point(170, 220),
                Size = new Size(100, 18),
                ForeColor = Color.FromArgb(150, 150, 160)
            };

            // Settings + About buttons
            btnSettings = new Button
            {
                Text = "Settings",
                Location = new Point(200, 145),
                Size = new Size(70, 22),
                BackColor = Color.FromArgb(80, 80, 140),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSettings.FlatAppearance.BorderSize = 0;

            btnAbout = new Button
            {
                Text = "About",
                Location = new Point(200, 170),
                Size = new Size(70, 22),
                BackColor = Color.FromArgb(80, 80, 140),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnAbout.FlatAppearance.BorderSize = 0;

            // Add controls
            Controls.Add(comboMainWindow);
            Controls.Add(btnRefreshMain);
            Controls.Add(comboMirrorWindow);
            Controls.Add(btnRefreshMirror);
            Controls.Add(lblLinkedStatus);

            Controls.Add(btnStart);
            Controls.Add(btnStop);
            Controls.Add(btnPanic);

            Controls.Add(lblDelayFrom);
            Controls.Add(numDelayFrom);
            Controls.Add(lblDelayTo);
            Controls.Add(numDelayTo);

            Controls.Add(lblMirrorKeys);
            Controls.Add(listMirrorKeys);
            Controls.Add(btnAddKey);
            Controls.Add(btnRemoveKey);

            Controls.Add(lblHotkeys);
            Controls.Add(panelMirroringLed);
            Controls.Add(lblMirroringStatus);

            Controls.Add(btnSettings);
            Controls.Add(btnAbout);
        }

        private void WireEvents()
        {
            btnRefreshMain.Click += btnRefreshMain_Click;
            btnRefreshMirror.Click += btnRefreshMirror_Click;

            btnStart.Click += btnStart_Click;
            btnStop.Click += btnStop_Click;
            btnPanic.Click += btnPanic_Click;

            btnAddKey.Click += btnAddKey_Click;
            btnRemoveKey.Click += btnRemoveKey_Click;

            btnSettings.Click += (s, e) => new SettingsForm(engine).ShowDialog();
            btnAbout.Click += (s, e) => new AboutForm().ShowDialog();

            numDelayFrom.ValueChanged += (s, e) => UpdateDelay();
            numDelayTo.ValueChanged += (s, e) => UpdateDelay();
        }

        private void btnRefreshMain_Click(object sender, EventArgs e)
        {
            comboMainWindow.Items.Clear();

            var windows = AOWindowScanner.Scan();
            foreach (var w in windows)
                comboMainWindow.Items.Add(w);

            if (comboMainWindow.Items.Count > 0)
                comboMainWindow.SelectedIndex = 0;
        }

        private void btnRefreshMirror_Click(object sender, EventArgs e)
        {
            comboMirrorWindow.Items.Clear();

            var windows = AOWindowScanner.Scan();
            foreach (var w in windows)
                comboMirrorWindow.Items.Add(w);

            if (comboMirrorWindow.Items.Count > 0)
                comboMirrorWindow.SelectedIndex = 0;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            var main = (AOWindowScanner.AOWindowInfo)comboMainWindow.SelectedItem;
            var mirror = (AOWindowScanner.AOWindowInfo)comboMirrorWindow.SelectedItem;

            engine.SetWindows(main.Handle, mirror.Handle);
            engine.Start();

            isMirroringActive = true;
            UpdateMirroringStatus();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            engine.Stop();
            isMirroringActive = false;
            UpdateMirroringStatus();
        }

        private void btnPanic_Click(object sender, EventArgs e)
        {
            engine.Panic();
            isMirroringActive = false;
            UpdateMirroringStatus();
        }

        private void btnAddKey_Click(object sender, EventArgs e)
        {
            string key = Prompt.ShowDialog("Enter key to mirror:", "Add Key");

            if (!string.IsNullOrWhiteSpace(key))
            {
                listMirrorKeys.Items.Add(key.ToUpper());
                engine.UpdateMirroredKeys(listMirrorKeys.Items);
            }
        }

        private void btnRemoveKey_Click(object sender, EventArgs e)
        {
            if (listMirrorKeys.SelectedItem != null)
            {
                listMirrorKeys.Items.Remove(listMirrorKeys.SelectedItem);
                engine.UpdateMirroredKeys(listMirrorKeys.Items);
            }
        }

        private void UpdateDelay()
        {
            engine.SetRandomDelay((double)numDelayFrom.Value, (double)numDelayTo.Value);
        }

        private void UpdateMirroringStatus()
        {
            if (isMirroringActive)
            {
                panelMirroringLed.BackColor = Color.FromArgb(60, 200, 90);
                lblMirroringStatus.ForeColor = Color.FromArgb(200, 200, 210);
            }
            else
            {
                panelMirroringLed.BackColor = Color.FromArgb(80, 80, 80);
                lblMirroringStatus.ForeColor = Color.FromArgb(150, 150, 160);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (WindowState == FormWindowState.Minimized)
                Hide();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            tray.Dispose();
            hotkeys.Dispose();
            keyHook.Dispose();
            base.OnFormClosing(e);
        }
    }

    // Simple input dialog for Add Key
    public static class Prompt
    {
        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 260,
                Height = 140,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterParent,
                BackColor = Color.FromArgb(18, 18, 22)
            };

            Label lblText = new Label() { Left = 10, Top = 10, Text = text, ForeColor = Color.White };
            TextBox txtInput = new TextBox() { Left = 10, Top = 35, Width = 220, BackColor = Color.FromArgb(30, 30, 36), ForeColor = Color.White };
            Button btnOk = new Button() { Text = "Add", Left = 150, Width = 80, Top = 70, BackColor = Color.FromArgb(40, 120, 220), ForeColor = Color.White };

            btnOk.Click += (sender, e) => { prompt.Close(); };

            prompt.Controls.Add(lblText);
            prompt.Controls.Add(txtInput);
            prompt.Controls.Add(btnOk);

            prompt.ShowDialog();

            return txtInput.Text;
        }
    }
}
