using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedditCrawler.Models;
using RedditCrawler.Utility;

namespace RedditCrawler
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //Initializes variables
            Constants.getConfigurations();

            //Gets reddit posts from new_all_posts.csv file and Inserts them into the Post table
            DAL.PostTable.InsertPosts(Utility.Parser.GetPosts());

            //Filters post if it contains at least one url, validates using VirusTotal, then insert to the Feature table
            await Indexer.StartIndexer();
            
            //Inserts into PhishPost table
            //Indexer.IndexPhishPost();


            //Indexer.GetDBCSV();

            Console.WriteLine("Done");

            string v = Console.ReadLine();
        }
    }
}
