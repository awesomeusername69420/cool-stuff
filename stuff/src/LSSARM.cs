/*
*   Bypasses the Lightspeed Smart Agent uninstaller's password requirement
*/

using System;
using System.Diagnostics;
using System.Reflection;
using System.Security.Principal;
using System.Threading;

namespace LSSARM
{
    class Program
    {
        private static bool isadmin()
        {
            WindowsIdentity usr = null;

            try
            {
                usr = WindowsIdentity.GetCurrent(); // Get the current user
                WindowsPrincipal p = new WindowsPrincipal(usr);

                bool r = p.IsInRole(WindowsBuiltInRole.Administrator); // Test if user is an admin

                usr.Dispose(); // Dispose of the user because memory

                return r;
            }
            catch (Exception)
            {
                usr.Dispose(); // If getting information about the user failed, pretend they're not admin
                return false;
            }
        }

        private static void pausekill(string s)
        {
            Console.WriteLine(s + ". Press any key to exit.");
            Console.ReadKey();

            Environment.Exit(-1);
        }

        private static bool ask(string q)
        {
            Console.WriteLine(q + " (Y/N)");
            string s = Console.ReadLine();

            return s.ToLower() == "y";
        }

        private static void uninstall()
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = "C:\\Windows\\System32\\cmd.exe";
                psi.Arguments = "/C wmic.exe product where \"name like 'Lightspeed Smart Agent'\" call uninstall"; // Makes wmic try to uninstall
                psi.WindowStyle = ProcessWindowStyle.Hidden;

                Process wmic = Process.Start(psi);

                Console.WriteLine("Trying to start windows installer...\nComputer will automatically reboot upon success.\nThis may take a while.");

                Thread.Sleep(300);

                bool found = false;
                bool killed = false;

                while (!found)
                {
                    foreach (Process p in Process.GetProcesses())
                    {
                        string title = p.MainWindowTitle.ToLower();

                        if (title.Contains("lightspeed smart agent setup") && p.ProcessName.ToLower() == "msiexec")
                        {
                            try
                            {
                                p.Kill();

                                killed = true;
                            }
                            catch (Exception) { }

                            found = true;

                            break;
                        }
                    }

                    Thread.Sleep(40);
                }

                if (killed)
                {
                    pausekill("Uninstall succeeded. Computer should reboot soon.");
                } else
                {
                    pausekill("Uninstall failed. :(");
                }
            }
            catch (Exception)
            {
                pausekill("Critical Error. Stopping uninstall");
            }
        }

        static void Main(string[] args)
        {
            Console.Clear();
            Console.ResetColor(); // Prep the console

            if (isadmin())
            {
                if (Process.GetProcessesByName("wmic").Length > 0) // Avoid problems with Windows (fuck msiexec check)
                {
                    if (ask("Another uninstall process is active. Continue?"))
                    {
                        uninstall();
                    } else
                    {
                        pausekill("Cancelled by user");
                    }
                } else
                {
                    uninstall();
                }
            } else
            {
                if (ask("Admin is required. Would you like to run as admin?"))
                {
                    try
                    {
                        ProcessStartInfo psi = new ProcessStartInfo(); // Creates a thing that can become a process
                        psi.FileName = Assembly.GetExecutingAssembly().Location; // Set process location to location of the current process
                        psi.UseShellExecute = true; // This is retarded (but needed)
                        psi.Verb = "runas"; // I don't get how this works but this makes it run as admin

                        Process p = Process.Start(psi); // Start the thing so it becomes a process

                        p.Dispose(); // Memory

                        Environment.Exit(1337); // Close current process if creating a new admin process succeeded (user click yes on uac)
                    }
                    catch (Exception)
                    {
                        pausekill("Failed to get Admin");
                    }
                } else
                {
                    pausekill("Admin is required");
                }
            }
        }
    }
}
