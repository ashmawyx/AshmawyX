using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace AshmawyX
{
    public static class AOWindowScanner
    {
        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        public class AOWindowInfo
        {
            public string Title { get; set; }
            public IntPtr Handle { get; set; }

            public override string ToString()
            {
                return Title;
            }
        }

        public static List<AOWindowInfo> Scan()
        {
            List<AOWindowInfo> windows = new List<AOWindowInfo>();

            EnumWindows(delegate (IntPtr hWnd, IntPtr lParam)
            {
                if (!IsWindowVisible(hWnd))
                    return true;

                int length = GetWindowTextLength(hWnd);
                if (length == 0)
                    return true;

                StringBuilder builder = new StringBuilder(length + 1);
                GetWindowText(hWnd, builder, builder.Capacity);

                string title = builder.ToString();

                // AO window detection
                if (title.Contains("Anarchy Online") || title.Contains("AO"))
                {
                    windows.Add(new AOWindowInfo
                    {
                        Title = title,
                        Handle = hWnd
                    });
                }

                return true;
            }, IntPtr.Zero);

            return windows;
        }
    }
}
