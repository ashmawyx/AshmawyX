using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AshmawyX
{
    public class GlobalHotkeys : IDisposable
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;

        private IntPtr hookId = IntPtr.Zero;
        private LowLevelKeyboardProc proc;

        public event Action OnStartHotkey;   // F7
        public event Action OnStopHotkey;    // F8
        public event Action OnPanicHotkey;   // F9

        public GlobalHotkeys()
        {
            proc = HookCallback;
            hookId = SetHook(proc);
        }

        public void Dispose()
        {
            UnhookWindowsHookEx(hookId);
        }

        private IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(
                    WH_KEYBOARD_LL,
                    proc,
                    GetModuleHandle(curModule.ModuleName),
                    0);
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);

                switch (vkCode)
                {
                    case (int)Keys.F7:
                        OnStartHotkey?.Invoke();
                        break;

                    case (int)Keys.F8:
                        OnStopHotkey?.Invoke();
                        break;

                    case (int)Keys.F9:
                        OnPanicHotkey?.Invoke();
                        break;
                }
            }

            return CallNextHookEx(hookId, nCode, wParam, lParam);
        }

        // WinAPI
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk,
            int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}
