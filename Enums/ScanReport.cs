using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirusTotalNet.Results;

namespace RedditCrawler.Enums
{
    public class ScanReport
    {
        public bool IsCompleted { get; set; }
        public IEnumerable<UrlReport> Reports { get; set; }

    }
}
