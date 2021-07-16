

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirusTotalNet;
using VirusTotalNet.Objects;
using VirusTotalNet.ResponseCodes;
using VirusTotalNet.Results;
using RedditCrawler.Enums;
using System.Text.RegularExpressions;

namespace RedditCrawler.Utility
{
    public class URLCheck
    {

        static int CALL_COUNT = 0;
   

        public static void CheckRequestCount()
        {
            if(CALL_COUNT != 0 && CALL_COUNT % 4 == 0)
            {
                Console.WriteLine("1 minute delay");
                System.Threading.Thread.Sleep(60000);
            }
        }


        public static List<string> GetUrls(string body)
        {
            var urls = new List<string>();
            //foreach (Match item in Regex.Matches(csvTable.Rows[i][0].ToString(), @"(http|ftp|https):\/\/([\w\-_]+(?:(?:\.[\w\-_]+)+))([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?"))
            foreach (Match item in Regex.Matches(body, @"((https?):((//)|(\\\\))+([\w\d:#@%/;$()~_?\+-=\\\.&](#!)?)*)"))
            {
                urls.Add(item.Value);
            }

            return urls;
        }

        public static bool IsFileExtension(string url)
        {
            try
            {
                var ext = url.Split('.').Last();
                var extensions = new List<string> { "pdf", "html", "php", "aspx", "exe", "json", "txt", "jpg", "jpeg", "png", "zip", "csv", "excel", "cs", "bat", "doc",
            "docx", "ppt", "pptx", "apk", "mp3", "mp4", "nupkg"};
                return extensions.Contains(ext);
            }
            catch
            {
                return false;
            }
        }

        public static async Task VirusTotalCheckAsync(string url)
        {
            VirusTotal virusTotal = new VirusTotal(Constants.apikey);

            //Use HTTPS instead of HTTP
            virusTotal.UseTLS = true;

            UrlReport urlReport = await virusTotal.GetUrlReportAsync(url);
           

            bool hasUrlBeenScannedBefore = urlReport.ResponseCode == UrlReportResponseCode.Present;
            Console.WriteLine("URL has been scanned before: " + (hasUrlBeenScannedBefore ? "Yes" : "No"));

            //If the url has been scanned before, the results are embedded inside the report.
            if (hasUrlBeenScannedBefore)
            {
                PrintScan(urlReport);
            }
            else
            {
                UrlScanResult urlResult = await virusTotal.ScanUrlAsync(url);
                PrintScan(urlResult);
            }
        }


        public static async Task<UrlReport> VirusTotalCheckCallBackAsync(string url)
        {
            try
            {
                VirusTotal virusTotal = new VirusTotal(Constants.apikey);

                //Use HTTPS instead of HTTP
                virusTotal.UseTLS = true;

                CheckRequestCount();
                Console.WriteLine("Scanning: " + url);
                UrlReport urlReport = await virusTotal.GetUrlReportAsync(url);
                CALL_COUNT += 1;

                bool hasUrlBeenScannedBefore = urlReport.ResponseCode == UrlReportResponseCode.Present;

                //If the url has been scanned before, the results are embedded inside the report.
                if (hasUrlBeenScannedBefore)
                {
                    //PrintScan(urlReport);
                }
                else
                {
                    CheckRequestCount();
                    UrlScanResult urlResult = await virusTotal.ScanUrlAsync(url);
                    CALL_COUNT += 1;
                    //PrintScan(urlResult);
                    Console.WriteLine("Sleeping");
                    System.Threading.Thread.Sleep(5000);
                    Console.WriteLine("Done Sleeping");
                    urlReport = await VirusTotalCheckCallBackAsync(url);

                }


                return urlReport;
            }
            catch(Exception e)
            {
                Console.WriteLine("Something Went Wrong");
                Console.WriteLine("Error: " + e.Message);
                Environment.Exit(0);
                return new UrlReport { ResponseCode = UrlReportResponseCode.NotPresent};
            }
        }

        public static async Task<IEnumerable<UrlReport>> VirusTotalChecksAsync(IEnumerable<string> urls)
        {
            VirusTotal virusTotal = new VirusTotal(Constants.apikey);
            
            //Use HTTPS instead of HTTP
            virusTotal.UseTLS = true;

            var reports = new List<UrlReport>();

            if(urls.Count() == 0)
            {
                return reports;
            }

            //IEnumerable<UrlReport> urlReports = await virusTotal.GetUrlReportsAsync(urls);
            var urlReports = new List<UrlReport>();
            foreach(var url in urls)
            {
                urlReports.Add(await virusTotal.GetUrlReportAsync(url));
            }

            if(urlReports.Where(x => x.ResponseCode == UrlReportResponseCode.NotPresent).Count() > 0)
            {
                Console.WriteLine($"{urlReports.Where(x => x.ResponseCode == UrlReportResponseCode.NotPresent).Count()} scans left");
                //foreach(var report in urlReports.Where(x => x.ResponseCode == UrlReportResponseCode.NotPresent))
                //{
                //    Console.WriteLine($"{report.URL}");

                //    UrlScanResult urlResult = await virusTotal.ScanUrlAsync(url);
                //    PrintScan(urlResult);
                //}

                foreach(var url in urlReports.Where(x => x.ResponseCode == UrlReportResponseCode.NotPresent))
                {
                    UrlScanResult urlResult = await virusTotal.ScanUrlAsync(url.URL);
                    PrintScan(urlResult);
                }

                
                System.Threading.Thread.Sleep(10000);
                reports.AddRange(await VirusTotalChecksAsync(urls));
            }
            else
            {
                reports.AddRange(urlReports);
            }

            return reports;
            
            
        }


        public static bool IsPhish(UrlReport urlReport)
        {
            var phishDetected = false;

            Console.WriteLine("Scan ID: " + urlReport.ScanId);
            Console.WriteLine("Message: " + urlReport.VerboseMsg);
            Console.WriteLine("Message: " + urlReport.URL);

            if (urlReport.ResponseCode == UrlReportResponseCode.Present)
            {
                if (urlReport.Scans != null)
                {
                    foreach (KeyValuePair<string, UrlScanEngine> scan in urlReport.Scans)
                    {
                        Console.WriteLine("{0,-25} Detected: {1}", scan.Key, scan.Value.Detected);
                        if (scan.Value.Detected) phishDetected = true;
                    }
                }
                else
                {
                    Console.WriteLine("Null scans detected");
                }
            }

            return phishDetected;
        }

        public static bool IsPhish(IEnumerable<UrlReport> urlReports)
        {
            var phishDetected = false;

            foreach(var urlReport in urlReports)
            {
                Console.WriteLine("Scan ID: " + urlReport.ScanId);
                Console.WriteLine("Message: " + urlReport.VerboseMsg);
                Console.WriteLine("Message: " + urlReport.URL);

                if (urlReport.ResponseCode == UrlReportResponseCode.Present)
                {
                    if(urlReport.Scans != null)
                    {
                        foreach (KeyValuePair<string, UrlScanEngine> scan in urlReport.Scans)
                        {
                            Console.WriteLine("{0,-25} Detected: {1}", scan.Key, scan.Value.Detected);
                            if (scan.Value.Detected) phishDetected = true;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Null scans detected");
                    }
                }
            }
            return phishDetected;
        } 

        private static void PrintScan(UrlScanResult scanResult)
        {
            Console.WriteLine("Scan ID: " + scanResult.ScanId);
            Console.WriteLine("Message: " + scanResult.VerboseMsg);
            Console.WriteLine();
        }

        private static void PrintScan(ScanResult scanResult)
        {
            Console.WriteLine("Scan ID: " + scanResult.ScanId);
            Console.WriteLine("Message: " + scanResult.VerboseMsg);
            Console.WriteLine();
        }

        private static void PrintScan(UrlReport urlReport)
        {
            Console.WriteLine("Scan ID: " + urlReport.ScanId);
            Console.WriteLine("Message: " + urlReport.VerboseMsg);

            if (urlReport.ResponseCode == UrlReportResponseCode.Present)
            {
                foreach (KeyValuePair<string, UrlScanEngine> scan in urlReport.Scans)
                {
                    Console.WriteLine("{0,-25} Detected: {1}", scan.Key, scan.Value.Detected);
                }
            }

            Console.WriteLine();
        }

    }
}
