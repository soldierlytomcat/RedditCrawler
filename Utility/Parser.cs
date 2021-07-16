using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using LumenWorks.Framework.IO.Csv;
using System.Data;
using System.Windows.Forms;
using RedditCrawler.Models;
using System.Text.RegularExpressions;

namespace RedditCrawler.Utility
{
    public abstract class Parser
    {

        public static IEnumerable<Post> GetPosts()
        {
            var path = Environment.CurrentDirectory;
            var csvPath = $@"{path}/new_all_posts.csv";
            var csvTable = new DataTable();
            var allPosts = new List<Post>();
            using (var csvReader = new CsvReader(new StreamReader(System.IO.File.OpenRead(csvPath)), true))
            {
                csvTable.Load(csvReader);
                //var date = new DateTime(1970, 1, 1).AddSeconds(Convert.ToDouble(csvTable.Rows[50000][10])).ToString("yyyy-MM-dd HH:mm:ss.fff");
                //MessageBox.Show(date);

                //selftext,title,id,sorted_by,num_comments,score,ups,downs,upvote_ratio,url,created,username,subreddit,created_utc

             

                for (int i = 0; i < csvTable.Rows.Count; i++)
                {
                    var post = new Post
                    {
                        //Id = i,
                        Body = csvTable.Rows[i][0].ToString(),
                        Title = csvTable.Rows[i][1].ToString(),
                        PostID = csvTable.Rows[i][2].ToString(),
                        PostSource = csvTable.Rows[i][12].ToString(),
                        NumOfComms = Convert.ToInt32(csvTable.Rows[i][4].ToString()),
                        Score = Convert.ToInt32(csvTable.Rows[i][5].ToString()),
                        URL = csvTable.Rows[i][9].ToString(),
                        Date = new DateTime(1970, 1, 1).AddSeconds(Convert.ToDouble(csvTable.Rows[i][10])).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        Username = csvTable.Rows[i][11].ToString(),
                    };

                    allPosts.Add(post);


                   

                }


            }

            Console.WriteLine($"Indexed {allPosts.Count} posts");
            return allPosts;
        }

    }
}
