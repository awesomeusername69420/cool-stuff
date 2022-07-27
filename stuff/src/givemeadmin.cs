/*
*   Asks for admin until it gets admin
*/

using System;
using System.Diagnostics;
using System.Reflection;
using System.Security.Principal;

namespace givemeadmin
{
    class Program
    {
        private static bool IsAdmin()
        {
            try
            {
                using (WindowsIdentity user = WindowsIdentity.GetCurrent())
                    return new WindowsPrincipal(user).IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch (Exception) { }

            return false;
        }

        private static void UAC()
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = Assembly.GetExecutingAssembly().Location;
                psi.UseShellExecute = true;
                psi.Verb = "runas";

                Process.Start(psi);

                Environment.Exit(1337);
            }
            catch (Exception)
            {
                UAC();
            }
        }

        static void Main(string[] args)
        {
            if (!IsAdmin())
            {
                try
                {
                    UAC();
                }
                catch (Exception) { }
            }
            else
            {
                Console.WriteLine("Admin!"); // Yay!! Admin!!!!
                Console.ReadKey();
            }
        }
    }
}
