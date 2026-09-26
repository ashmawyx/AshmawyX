using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace AshmawyX
{
    public class MirroringEngine
    {
        private IntPtr mainWindow = IntPtr.Zero;
        private IntPtr mirrorWindow = IntPtr.Zero;

        private bool isRunning = false;

        // Chat block always ON
        private bool chatBlockAlwaysOn = true;

        // Delay system (random FROM → TO)
        private double delayFrom = 0.0;
        private double delayTo = 0.0;
        private Random rnd = new Random();

        // Dynamic mirrored keys list
        private readonly ArrayList mirroredKeys = new ArrayList();

        // Permanent blocked keys (never mirrored)
        // These keys are ALWAYS blocked
        private readonly int[] permanentBlockedKeys = new int[]
        {
            (int)Keys.W,
            (int)Keys.A,
            (int)Keys.S,
            (int)Keys.D,
            (int)Keys.X,
            (int)Keys.Z,
            (int)Keys.C,
            (int)Keys.Space,
            (int)Keys.OemQuestion, // slash '/'
            (int)Keys.Enter,
            (int)Keys.Tab
        };

        // WinAPI for sending keys
        [DllImport("user32.dll")]
        private static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        private const uint WM_KEYDOWN = 0x0100;
        private const uint WM_KEYUP = 0x0101;

        public void SetWindows(IntPtr main, IntPtr mirror)
        {
            mainWindow = main;
            mirrorWindow = mirror;
        }

        public void Start()
        {
            if (mainWindow == IntPtr.Zero || mirrorWindow == IntPtr.Zero)
                return;

            isRunning = true;
        }

        public void Stop()
        {
            isRunning = false;
        }

        public void Panic()
        {
            isRunning = false;
        }

        // Update mirrored keys list from MainForm
        public void UpdateMirroredKeys(ListBox.ObjectCollection items)
        {
            mirroredKeys.Clear();

            foreach (var item in items)
            {
                string key = item.ToString().Trim().ToUpper();
                if (!string.IsNullOrWhiteSpace(key))
                    mirroredKeys.Add(key);
            }
        }

        // Random delay FROM → TO
        public void SetRandomDelay(double from, double to)
        {
            delayFrom = from;
            delayTo = to;
        }

        private void ApplyDelay()
        {
            if (delayTo <= 0.0)
                return;

            double min = Math.Min(delayFrom, delayTo);
            double max = Math.Max(delayFrom, delayTo);

            double randomDelay = min + rnd.NextDouble() * (max - min);
            Thread.Sleep((int)(randomDelay * 1000));
        }

        public void ProcessKey(int vkCode)
        {
            if (!isRunning)
                return;

            // Permanent blocked keys
            foreach (int blocked in permanentBlockedKeys)
            {
                if (vkCode == blocked)
                    return;
            }

            // Chat block always ON
            if (chatBlockAlwaysOn)
            {
                if (vkCode == (int)Keys.Enter ||
                    vkCode == (int)Keys.OemPeriod ||
                    vkCode == (int)Keys.Oemcomma)
                {
                    return;
                }
            }

            // Dynamic mirrored keys list
            bool allowed = false;

            foreach (var item in mirroredKeys)
            {
                string key = item.ToString().ToUpper();

                // Convert key name to VK code
                if (key.Length == 1)
                {
                    int ascii = (int)key[0];
                    if (vkCode == ascii)
                    {
                        allowed = true;
                        break;
                    }
                }
                else
                {
                    // Special keys like F1, F2, etc.
                    if (key.StartsWith("F"))
                    {
                        int num;
                        if (int.TryParse(key.Substring(1), out num))
                        {
                            int fKeyCode = (int)Keys.F1 + (num - 1);
                            if (vkCode == fKeyCode)
                            {
                                allowed = true;
                                break;
                            }
                        }
                    }
                }
            }

            if (!allowed)
                return;

            // Delay
            ApplyDelay();

            // Send key to mirror window
            SendKeyToMirror(vkCode);
        }

        private void SendKeyToMirror(int vkCode)
        {
            if (mirrorWindow == IntPtr.Zero)
                return;

            PostMessage(mirrorWindow, WM_KEYDOWN, (IntPtr)vkCode, IntPtr.Zero);
            PostMessage(mirrorWindow, WM_KEYUP, (IntPtr)vkCode, IntPtr.Zero);
        }
    }
}
