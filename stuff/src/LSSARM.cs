/*
*   Bypasses the Lightspeed Smart Agent uninstaller's password requirement
*/

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Security.Principal;

namespace LSSARM
{
    class Program
    {
        private static List<string> keys = new List<string>() // Registry keys with program information according to Microsoft's guidelines
        {
            "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall",
            "SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall"
        };

        private static void getPrograms(RegistryKey rk, List<string> o)
        {
            foreach (string k in keys)
            {
                using (RegistryKey ork = rk.OpenSubKey(k))
                {
                    if (ork == null) // Sad
                    {
                        continue;
                    }

                    foreach (string name in ork.GetSubKeyNames())
                    {
                        using (RegistryKey osk = ork.OpenSubKey(name))
                        {
                            try
                            {
                                string cname = (string)osk.GetValue("DisplayName");

                                if (string.IsNullOrWhiteSpace(cname)) // Retarded programs
                                {
                                    continue;
                                }

                                string us = (string)osk.GetValue("UninstallString");

                                if (string.IsNullOrWhiteSpace(us))
                                {
                                    continue;
                                }

                                o.Add(cname);
                                o.Add(us);
                            }
                            catch (Exception) { }
                        }
                    }
                }
            }
        }

        private static bool isadmin()
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
            catch (Exception)
            {
                usr.Dispose(); // If getting information about the user failed or something broke, pretend they're not admin

                return false;
            }
        }

        private static void pausekill(string s)
        {
            Console.WriteLine(s + " Press any key to exit.");
            Console.ReadKey();

            Environment.Exit(-1);
        }

        private static void uninstall()
        {
            Console.WriteLine("Beginning uninstall process...");

            List<string> found = new List<string>();

            try
            {
                Console.WriteLine("Gathering programs...");

                bool lsfound = false;
                bool killed = false;

                getPrograms(RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64), found); // Get programs
                getPrograms(RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64), found);

                Console.WriteLine("Searching for Lightspeed...");

                for (int i = 0; i < found.Count; i++)
                {
                    if (killed)
                    {
                        break; // Stupid
                    }

                    string s = found[i].ToLower();

                    if (s.Contains("msiexec")) // Uninstall string!
                    {
                        continue;
                    }

                    if (s.ToLower().Contains("lightspeed")) // Lightspeed has been found!
                    {
                        Console.WriteLine("Lightspeed found, attempting uninstall...");

                        ProcessStartInfo psi = new ProcessStartInfo();
                        psi.FileName = "cmd.exe";
                        psi.Arguments = "/C " + found[i + 1];
                        psi.WindowStyle = ProcessWindowStyle.Hidden;
                        psi.UseShellExecute = true;
                        psi.Verb = "runas";

                        Process.Start(psi);

                        lsfound = true;
                        bool looping = true; // Used to break out of nested loop

                        while (looping)
                        {
                            foreach (Process p in Process.GetProcessesByName("msiexec"))
                            {
                                string title = p.MainWindowTitle.ToLower();

                                if (title.Contains("lightspeed smart agent setup"))
                                {
                                    p.Kill();

                                    killed = true;
                                    looping = false;

                                    break;
                                }
                            }
                        }
                    }
                }

                if (lsfound)
                {
                    if (killed)
                    {
                        pausekill("Uninstall succeeded. Computer should reboot soon."); // :D
                    }
                    else
                    {
                        pausekill("Uninstall failed. :("); // D:
                    }
                } else
                {
                    pausekill("Lightspeed not found."); // D:!!!
                }
            }
            catch (Exception ex)
            {
                pausekill("Critical Error. Aborting uninstall.\nError: " + ex.ToString());
            }
        }

        static void Main(string[] args)
        {
            Console.Clear();
            Console.ResetColor();

            if (isadmin())
            {
                uninstall(); // Let the fun begin
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
                    pausekill("Failed to get Admin.");
                }
            }
        }
    }
}
