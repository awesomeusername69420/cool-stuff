/*
*   Changes all main active window titles to be awesome!!
*/

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace kaboom
{
    internal class Program
    {
        private delegate bool EnumWindowsProc(IntPtr hWnd, int lParam);

        [DllImport("user32.dll")]
        static extern bool SetWindowText(IntPtr hWnd, string text); // Thing used to change window text

        [DllImport("user32.DLL")]
        private static extern bool EnumWindows(EnumWindowsProc enumFunc, int lParam);

        [DllImport("user32.DLL")]
        private static extern int GetWindowText(IntPtr hWND, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.DLL")]
        private static extern int GetWindowTextLength(IntPtr hWND);

        [DllImport("user32.DLL")]
        private static extern bool IsWindowVisible(IntPtr hWND);

        [DllImport("user32.DLL")]
        private static extern IntPtr GetShellWindow();

        private static IDictionary<IntPtr, string> GetOpenWindows()
        {
            IntPtr shellWindow = GetShellWindow();
            Dictionary<IntPtr, string> windows = new Dictionary<IntPtr, string>();

            EnumWindows(delegate (IntPtr hWND, int lParam)
            {
                if (hWND == shellWindow) return true;
                if (!IsWindowVisible(hWND)) return true;

                int length = GetWindowTextLength(hWND);
                if (length == 0) return true;

                StringBuilder builder = new StringBuilder(length);
                GetWindowText(hWND, builder, length + 1);

                windows[hWND] = builder.ToString();
                return true;
            }, 0);

            return windows;
        }

        public static List<string> titles = new List<string>() {"HELLO", "HI", "HEY", "YOUR MOM IS FAT"};

        static void Main(string[] args)
        {
            int cur = 0;

            while (true)
            {
                foreach (KeyValuePair<IntPtr, string> window in GetOpenWindows()) // Loop through windows
                {
                    try
                    {
                        SetWindowText(window.Key, titles[cur]); // Try to change its text to this
                    }
                    catch (Exception) { } // Failed to set this window
                }

                cur = cur + 1;

                if (cur > titles.Count - 1)
                {
                    cur = 0;
                }

                Thread.Sleep(1500); // Wait 1.5 seconds before cycling to next title
            }
        }
    }
}
