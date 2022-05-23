/*
*   Bypasses the Lightspeed Smart Agent uninstaller's password requirement
*/

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;

namespace LSSARM
{
    class Program
    {
        private delegate bool EnumWindowsProc(IntPtr hWND, int lParam);

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

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

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

        private static void GetPrograms(RegistryKey rk, List<string> o, List<string> keys)
        {
            foreach (string k in keys)
            {
                using (RegistryKey ork = rk.OpenSubKey(k))
                {
                    if (ork == null) continue; // Sad

                    foreach (string name in ork.GetSubKeyNames())
                    {
                        using (RegistryKey osk = ork.OpenSubKey(name))
                        {
                            try
                            {
                                string cname = (string)osk.GetValue("DisplayName");

                                if (string.IsNullOrWhiteSpace(cname)) continue; // Retarded programs

                                string us = (string)osk.GetValue("UninstallString");

                                if (string.IsNullOrWhiteSpace(us)) continue;

                                o.Add(cname);
                                o.Add(us);
                            }
                            catch (Exception) { }
                        }
                    }
                }
            }
        }

        private static bool IsAdmin()
        {
            WindowsIdentity usr = null;

            try
            {
                usr = WindowsIdentity.GetCurrent(); // Get the current user
                WindowsPrincipal p = new WindowsPrincipal(usr);

                bool r = p.IsInRole(WindowsBuiltInRole.Administrator); // Test if user is an admin

                usr.Dispose();

                return r;
            } 
            catch (Exception) // If getting information about the user failed or something broke, pretend they're not admin
            {
                usr.Dispose();

                return false;
            }
        }

        private static void PauseKill(string s)
        {
            Console.WriteLine(s + " Press any key to exit.");
            Console.ReadKey();

            Environment.Exit(-1);
        }

        private static void Uninstall()
        {
            List<string> found = new List<string>();

            try
            {
                Console.WriteLine("Gathering programs...");

                bool lsfound = false;
                bool killed = false;

                List<string> keys = new List<string>() // Registry keys with program information according to Microsoft's guidelines
                {
                    "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall",
                    "SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall"
                };

                GetPrograms(RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64), found, keys); // Get programs
                GetPrograms(RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64), found, keys); // Lightspeed isn't usually in CurrentUser but check just in case

                Console.WriteLine("Searching for Lightspeed in the registry...");

                for (int i = 0; i < found.Count; i++)
                {
                    if (killed) break; // Stupid

                    string s = found[i];

                    if (s.ToLower().Contains("msiexec")) continue; // Skip checking uninstall strings

                    if (s.Equals("Lightspeed Smart Agent")) // Lightspeed has been found!
                    {
                        Console.WriteLine("Lightspeed found, attempting uninstall...");

                        ProcessStartInfo psi = new ProcessStartInfo();
                        psi.FileName = "cmd.exe";
                        psi.Arguments = "/C " + found[i + 1] + " /q"; // '/q' removes the popup Yes/No dialog
                        psi.WindowStyle = ProcessWindowStyle.Hidden;
                        psi.UseShellExecute = true;
                        psi.Verb = "runas";

                        Process.Start(psi);

                        lsfound = true;
                        bool search = true; // Used to break out of nested loop

                        while (search)
                        {
                            foreach (KeyValuePair<IntPtr, string> window in GetOpenWindows())
                            {
                                if (window.Value.Equals("Lightspeed Smart Agent Setup"))
                                {
                                    search = false;
                                    
                                    uint pid;
                                    GetWindowThreadProcessId(window.Key, out pid);
                                    
                                    Process p = Process.GetProcessById((int)pid);
 
                                    if (p != null)
                                    {
                                        try
                                        {
                                            p.Kill();
                                            killed = true;
                                        }
                                        catch (Exception) { }
                                    }

                                    break;
                                }
                            }
                        }
                    }
                }

                if (lsfound)
                {
                    if (killed)
                        PauseKill("Uninstall succeeded. Computer should reboot soon."); // :D
                    else
                        PauseKill("Uninstall failed. :("); // D:
                }
                else
                {
                    PauseKill("Lightspeed not found."); // D:!!!
                }
            }
            catch (Exception ex)
            {
                PauseKill("Critical Error. Aborting uninstall.\nError: " + ex.ToString()); // wtflip
            }
        }

        static void Main(string[] args)
        {
            Console.Clear();
            Console.ResetColor();

            if (IsAdmin())
            {
                Uninstall(); // Let the fun begin
            }
            else
            {
                try
                {
                    ProcessStartInfo psi = new ProcessStartInfo(); // Restarts the process as admin
                    psi.FileName = Assembly.GetExecutingAssembly().Location;
                    psi.UseShellExecute = true;
                    psi.Verb = "runas";

                    Process p = Process.Start(psi);

                    p.Dispose();

                    Environment.Exit(1337);
                }
                catch (Exception)
                {
                    PauseKill("Failed to get Admin.");
                }
            }
        }
    }
}
