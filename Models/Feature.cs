using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHelpers;

namespace RedditCrawler.Models
{

    [DelimitedRecord(",")]
    public class Feature
    {
        public int Id { get; set; }
        public string PostID { get; set; }
        public string URL { get; set; }
        public int LinkLength { get; set; }
        public bool IsSecured { get; set; }
        public int NumDots { get; set; }
        public int NumSigns { get; set; }
        public int NumDigits { get; set; }
        public int LenUsername { get; set; }
        public bool IsFile { get; set; }
        public int NumComments { get; set; }
        public int UserAge { get; set; }
        public int Score { get; set; }
        public int LenTitle { get; set; }
        public int LenUsernameLetters { get; set; }
        public int LenUsernameDigits { get; set; }
        public int LenUsernameSigns { get; set; }
        public string Username { get; set; }
        public bool isPhish { get; set; }
        public bool isVTPhish { get; set; }
        public bool isHam { get; set; }
    }
}
