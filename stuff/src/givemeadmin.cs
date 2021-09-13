/*
*   Asks for admin until it gets admin
*/

using System;
using System.Diagnostics;
using System.Reflection;
using System.Security.Principal;

namespace ConsoleApp1
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

        private static void getadmin()
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
                getadmin(); // They clicked no or something went wrong? Try again!
            }
        }

        static void Main(string[] args)
        {
            if (!isadmin())
            {
                try
                {
                    getadmin();
                }
                catch (Exception) { }
            } else
            {
                Console.WriteLine("Admin!"); // Yay!! Admin!!!!
                Console.ReadKey();
            }
        }
    }
}
