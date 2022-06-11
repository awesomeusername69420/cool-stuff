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
        private static string GetLatestRelease(string url, string filename="")
        {
            string response = "FAILED TO FETCH";

            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Method = "GET";
                request.AllowAutoRedirect = true;
                request.ContentType = "application/x-www-form-urlencoded";

                response = request.GetResponse().ResponseUri.ToString().Replace("tag", "download") + "/" + filename;
            }
            catch (Exception) { }

            return response;
        }

        private static string GetLatestVersion(string url)
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
            Console.WriteLine(GetLatestVersion("https://github.com/derrod/legendary/releases/latest/")); // As of 06/11/2022 this becomes "0.20.26"
            Console.WriteLine(GetLatestRelease("https://github.com/derrod/legendary/releases/latest/", "legendary.exe")); // As of 06/11/2022 this becomes "https://github.com/derrod/legendary/releases/download/0.20.26/legendary.exe
            Console.ReadLine();
        }
    }
}
