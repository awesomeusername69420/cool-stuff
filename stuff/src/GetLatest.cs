/*
 * Gets the latest version of a github repo release because github is dumb and
 * makes it difficult to just do it normally with one url
 * 
 * Input for GetLatestRelease:
 *      - "https://github.com/(reponame)/releases/latest"
 *      - Filename you want to download from the release (Ex: "whatever.exe") (OPTIONAL. You can add it yourself whenever you want)
 *      
 * Input for GetLatestVersion:
 *      - "https://github.com/(reponame)/releases/latest"
 */

using System;
using System.Net;

namespace GetLatest
{
    internal class Program
    {
        public static string GetLatestRelease(string url, string filename = "")
        {
            string pResponse = "FAILED TO FETCH";

            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Timeout = 5000;
                request.Method = "GET";
                request.AllowAutoRedirect = true;
                request.ContentType = "application/x-www-form-urlencoded";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                pResponse = response.ResponseUri.ToString().Replace("tag", "download") + "/" + filename;

                response.Close();
            }
            catch (Exception) { }

            return pResponse;
        }

        public static string GetLatestVersion(string url)
        {
            try
            {
                string final = GetLatestRelease(url, "");

                if (!final.Equals("FAILED TO FETCH"))
                {
                    string[] data = final.Split('/');

                    return data[data.Length - 2];
                }
            }
            catch (Exception) { }


            return string.Empty;
        }

        static void Main(string[] args)
        {
            string legendaryURL = "https://github.com/derrod/legendary/releases/latest/";
            string obsURL = "https://github.com/obsproject/obs-studio/releases/latest/";
            
            Console.WriteLine("Legendary:");
            
            Console.WriteLine(GetLatestVersion(legendaryURL)); // As of 06/11/2022 this becomes "0.20.26"
            Console.WriteLine(GetLatestRelease(legendaryURL, "legendary.exe")); // As of 06/11/2022 this becomes "https://github.com/derrod/legendary/releases/download/0.20.26/legendary.exe

            Console.WriteLine(Environment.NewLine + "OBS-Studio:");
            
            string latest = GetLatestRelease(obsURL);
            string latestVersion = GetLatestVersion(obsURL);
            
            Console.WriteLine(latestVersion);
            Console.WriteLine(latest + "OBS-Studio-" + latestVersion + "-Full-Installer-x64.exe");

            Console.ReadLine();
        }
    }
}
