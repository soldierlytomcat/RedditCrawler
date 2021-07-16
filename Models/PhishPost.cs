using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHelpers;

namespace RedditCrawler.Models
{
    [DelimitedRecord(",")]
    public class PhishPost
    {
        public int Id { get; set; }
        public string PostID { get; set; }
        public string URL { get; set; }
        public string Title { get; set; }
        public string PostSource { get; set; }
        public int NumOfComms { get; set; }
        public string Date { get; set; }
        public int Score { get; set; }
    }
}
