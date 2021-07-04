using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Configuration;

namespace video_downloader
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("name of the input file (press enter for default value: input.txt)");
            string inputfile = Console.ReadLine();
            if (String.IsNullOrEmpty(inputfile))
            {
                inputfile = "input.txt";
            }
            else
            {
                inputfile = inputfile.Trim();
            }

            Console.WriteLine("name of the facebook cookie file (press enter for default value: facebook.com_cookies.txt)");
            string cookieFile = Console.ReadLine();
            if (String.IsNullOrEmpty(cookieFile))
            {
                cookieFile = "facebook.com_cookies.txt";
            }
            else
            {
                cookieFile = cookieFile.Trim();
            }

            Console.WriteLine("min wait time per download (press enter for default value: 50 sec)");
            string minWait = Console.ReadLine();
            int minWaitSec = 50;
            if (!String.IsNullOrEmpty(minWait))
            {
                if (!int.TryParse(minWait, out minWaitSec))
                {
                    Console.WriteLine("wait time must be digit");
                    Console.ReadLine();
                    return;
                }
            }
            else
            {
                minWaitSec = 50;
            }
            Console.WriteLine("max wait time per download (press enter for default value: 70 sec)");
            string maxWait = Console.ReadLine();
            int maxWaitSec = 70;
            if (!String.IsNullOrEmpty(maxWait))
            {
                if (!int.TryParse(maxWait, out maxWaitSec))
                {
                    Console.WriteLine("wait time must be digit");
                    Console.ReadLine();
                    return;
                }
            }
            else
            {
                maxWaitSec = 70;
            }

            string line;

            System.IO.StreamReader file = new System.IO.StreamReader(String.Format("{0}", inputfile));
            var output = new System.IO.StreamWriter("cache.txt", append: false);

            var expressions = new ArrayList(new string[]
            {
                @"^https:\/\/(www\.)*facebook",
                @"^https:\/\/(www\.)*instagram.com\/tv",
            });

            while ((line = file.ReadLine()) != null)
            {
                foreach(string expression in expressions)
                {
                    Match mc = Regex.Match(line.Trim(), expression);
                    if (mc.Success)
                    {
                        var myLine = line.Trim();
                        output.WriteLine(myLine);
                        break;
                    }
                }
            }
            output.Close();
            file.Close();


            var arg = String.Format("--batch-file cache.txt --cookies {0} --user-agent \"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.90 Safari/537.36\" --sleep-interval {1}  --max-sleep-interval {2} --no-overwrites --restrict-filenames --ignore-errors --output /archive/[%(upload_date)s]-%(id)s.%(ext)s", cookieFile, minWaitSec, maxWaitSec);

            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "youtube-dl.exe",
                    Arguments = arg,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };
            proc.Start();
            while (!proc.StandardOutput.EndOfStream)
            {
                string _line = proc.StandardOutput.ReadLine();
                Console.WriteLine(_line);
            }
            Console.WriteLine(proc.StandardError.ReadToEnd());
            Console.WriteLine("Download Job Finished, press anything to close process");
            Console.ReadLine();
        }
    }
}
