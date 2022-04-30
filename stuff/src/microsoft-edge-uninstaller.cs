/*
*   Finds information about Edge via registry and uninstalls it
*/

using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;

namespace EdgeSucks
{
    class Program
    {
        static void Explode(string reason, int code=-1)
        {
            Console.WriteLine(reason);
            Console.ReadKey();
            Environment.Exit(code);
        }

        static void Main(string[] args)
        {
            string registry_key = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall"; // Location of installed programs that followed Microsoft's guidelines
            string edge_version = "";
            string edge_location = "";

            RegistryKey key = null;

            try
            {
                key = Registry.LocalMachine.OpenSubKey(registry_key); // Attempt to open the registry key containing the installed programs
            }
            catch (Exception) { }

            if (key == null) // This shouldn't happen but just in case
            {
                Explode("Failed to open registry key.");
            }

            try
            {
                foreach (string subkey_name in key.GetSubKeyNames())
                {
                    try
                    {
                        using (RegistryKey subkey = key.OpenSubKey(subkey_name))
                        {
                            string sname = (string)subkey.GetValue("DisplayName");

                            if (!string.IsNullOrEmpty(sname))
                            {
                                sname = sname.ToLower();

                                if (sname == "microsoft edge")
                                {
                                    string sver = (string)subkey.GetValue("DisplayVersion");
                                    string spath = (string)subkey.GetValue("InstallLocation"); // Attempt to get the information of Edge through the registry

                                    int found = 0;

                                    if (!string.IsNullOrEmpty(sver))
                                    {
                                        edge_version = sver;

                                        found = found + 1;
                                    }

                                    if (!string.IsNullOrEmpty(spath))
                                    {
                                        edge_location = spath;

                                        found = found + 1;
                                    }

                                    if (found == 2)
                                    {
                                        break; // If a version and location were found there's no need to go over the rest of the programs when we already found what we need
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error while enumerating over subkey. Error:" + Environment.NewLine + ex.ToString()); // No need to stop the program for these errors, just try the rest of the keys
                    }
                }
            }
            catch (Exception ex) // If something goes horribly wrong, catch it
            {
                Explode("Failed to enumerate over subkeys. Error:" + Environment.NewLine + ex.ToString(), -2);
            }

            if (string.IsNullOrEmpty(edge_version)) // :(
            {
                Explode("Failed to find Edge version.", -3);
            }

            if (string.IsNullOrEmpty(edge_location)) // :(
            {
                Explode("Failed to find Edge location.", -4);
            }

            string fullpath = edge_location + "\\" + edge_version + "\\Installer\\setup.exe";

            if (File.Exists(fullpath))
            {
                ProcessStartInfo psi = new ProcessStartInfo();

                psi.FileName = fullpath;
                psi.Arguments = "--uninstall --system-level --force-uninstall"; // Launch arguments needed to uninstall
                psi.Verb = "runas";
                psi.UseShellExecute = true; // This and Verb "runas" makes it run as Administrator

                try
                {
                    Process p = Process.Start(psi);

                    p.WaitForExit();

                    Explode("Uninstall succeeded. Press any key to exit.", 0); // :D
                }
                catch (Exception ex) // :(
                {
                    Explode("Uninstall failed. Error:" + Environment.NewLine + ex.ToString(), -5);
                }
            }
        }
    }
}
