using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RedditCrawler
{
    public abstract class ScriptExecutor
    {
        public static void run_cmd(string cmd, string args)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            var path = Environment.CurrentDirectory;

            start.FileName = $@"{path}/Reddit Post Crawler/reddit-crawler-env/Scripts/python.exe";
            var file = $@"{path}/Reddit Post Crawler/reddit-crawler-env/Scripts/praw_scrapper.py";
            //start.Arguments = string.Format("\"{0}\" \"{1}\"", cmd, file);
            start.Arguments = string.Format("\"{0}\"", file);
            start.UseShellExecute = false;
            start.CreateNoWindow = true;
            start.RedirectStandardOutput = true;
            start.RedirectStandardError = true;

            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    Console.Write(result);
                }
            }

            Console.ReadLine();
        }
    }
}
