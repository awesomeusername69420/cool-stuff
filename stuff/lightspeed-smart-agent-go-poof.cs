/*
*   Bypasses the Lightspeed Smart Agent uninstaller's password requirement
*/

using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Threading;

namespace ConsoleApp1
{
    class Program
    {
        private static void die(string rs)
        {
            ConsoleColor c = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine("");
            Console.WriteLine(rs);

            Console.ForegroundColor = c;

            Console.ReadKey();
            Environment.Exit(-1);
        }

        private static bool pStart(string path, string args = "", bool admin = false, bool vis = true)
        {
            ProcessStartInfo psi = new ProcessStartInfo(); // Creates new thing that will become a process
            psi.FileName = path; // Sets exe file to run

            if (!String.IsNullOrEmpty(args))
            {
                psi.Arguments = args; // Supplies arguments
            }

            if (admin)
            {
                psi.UseShellExecute = true;
                psi.Verb = "runas"; // Makes it run as admin
            }

            if (!vis)
            {
                psi.WindowStyle = ProcessWindowStyle.Hidden; // Hides the window
            }

            try
            {
                Process p = Process.Start(psi);
                p.Dispose();

                return true; // If the process started successfully, let them know
            }
            catch (Exception)
            {
                return false;
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

                usr.Dispose(); // Dispose of the user because memory

                return r;
            }
            catch (Exception)
            {
                usr.Dispose(); // If getting information about the user failed, pretend they're not admin
                return false;
            }
        }

        static void Main(string[] args)
        {
            Console.Clear();
            Console.ResetColor(); // Prep the console

            if (!isadmin())
            {
                die("Admin is required. Press any key to exit."); // Don't let the program run without admin
            }

            if (Process.GetProcessesByName("wmic").Length > 0)
            {
                die("WMIC detected. Close any other installers."); // Don't let the program run if something is already being uninstalled / installed
            }

            Console.WriteLine("Attempting to start looking... (If stuck on looking, you're not. It takes a while.)");

            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "C:\\Windows\\System32\\cmd.exe";
            psi.Arguments = "/C wmic.exe product where \"name like 'Lightspeed Smart Agent'\" call uninstall"; // Calls wmic.exe to start an uninstall on a program with the name "Lighspeed Smart Agent"
            psi.WindowStyle = ProcessWindowStyle.Hidden; // Hide the cmd window

            try
            {
                Process p = Process.Start(psi); // Start cmd

                Console.WriteLine("Looking... (May take a while. The computer should reboot automatically if uninstall succeeds.)");

                p.Dispose();
            } catch (Exception)
            {
                die("Failed to start looking. Press any key to exit.");
            }

            bool loc = false;
            bool suc = false;
            bool wmics = false;

            while (!wmics)
            {
                wmics = false;

                if (Process.GetProcessesByName("wmic").Length > 0)
                {
                    wmics = true; // Once wmic launches, start searching for lightspeed's uninstaller
                    break;
                }

                Thread.Sleep(40); // Let the process run while not hogging up cpu, preventing a crash and letting the system know the program is still responsive
            }

            wmics = false;

            while (!loc)
            {
                wmics = false;

                if (Process.GetProcessesByName("wmic").Length > 0)
                {
                    wmics = true;
                    continue;
                }

                foreach (Process p in Process.GetProcesses()) // Loops through the processes
                {
                    string title = p.MainWindowTitle.ToLower(); // Get the title of the process

                    if ((title == "lightspeed smart agent setup" || title.Contains("lightspeed")) && p.Id != Process.GetCurrentProcess().Id) // Tests for the lightspeed uninstaller window
                    {
                        loc = true;

                        Console.WriteLine("Found! Trying to uninstall..."); // Let them know we found it

                        try
                        {
                            p.Kill();
                            suc = true;
                            break;
                        }
                        catch (Exception)
                        {
                            suc = false;
                        }
                    }
                }

                if (!wmics)
                {
                    loc = true;
                    break;
                }

                Thread.Sleep(40);
            }

            if (suc)
            {
                ConsoleColor c = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Cyan;

                Console.WriteLine("\nUninstall succeeded! Attempting to reboot...");
                Console.ReadKey();

                Environment.Exit(0);
            }
            else
            {
                die("Uninstall Failed. Press any key to exit."); // :(
            }
        }
    }
}
