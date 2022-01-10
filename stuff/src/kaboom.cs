/*
*   Changes all main active window titles to be awesome!!
*/

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace kaboom
{
    internal class Program
    {
        [DllImport("user32.dll")]
        static extern bool SetWindowText(IntPtr hWnd, string text); // Thing used to change window text

        public static string[] titles = {"HELLO", "HI", "HEY", "YOUR MOM IS FAT"};

        static void Main(string[] args)
        {
            int cur = 0;

            while (true)
            {
                foreach (Process p in Process.GetProcesses()) // Loop through processes
                {
                    try
                    {
                        SetWindowText(p.MainWindowHandle, titles[cur]); // Try to change its text to this
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Failed setting text on " + Convert.ToString(p.Id)); // Says if one failed
                    }
                }

                cur = cur + 1;

                if (cur > titles.Length - 1)
                {
                    cur = 0;
                }

                Thread.Sleep(1500); // Wait 40 ms to prevent a crash while letting the system know the process is still alive
            }
        }
    }
}
