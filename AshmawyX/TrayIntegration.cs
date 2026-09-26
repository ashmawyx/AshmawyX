using System;
using System.Drawing;
using System.Windows.Forms;

namespace AshmawyX
{
    public class TrayIntegration : IDisposable
    {
        private NotifyIcon trayIcon;
        private MainForm mainForm;

        public TrayIntegration(MainForm form)
        {
            mainForm = form;

            trayIcon = new NotifyIcon
            {
                Icon = SystemIcons.Application,
                Text = "AshmawyX",
                Visible = true
            };

            var menu = new ContextMenuStrip();

            // Show main window
            menu.Items.Add("Show", null, (s, e) => ShowMain());

            // Settings
            menu.Items.Add("Settings", null, (s, e) =>
            {
                new SettingsForm(mainForm.engine).ShowDialog();
            });

            // About
            menu.Items.Add("About", null, (s, e) =>
            {
                new AboutForm().ShowDialog();
            });

            // Exit
            menu.Items.Add("Exit", null, (s, e) =>
            {
                Application.Exit();
            });

            trayIcon.ContextMenuStrip = menu;

            trayIcon.DoubleClick += (s, e) => ShowMain();
        }

        private void ShowMain()
        {
            if (mainForm.WindowState == FormWindowState.Minimized)
                mainForm.WindowState = FormWindowState.Normal;

            mainForm.Show();
            mainForm.BringToFront();
        }

        public void Dispose()
        {
            trayIcon.Visible = false;
            trayIcon.Dispose();
        }
    }
}
