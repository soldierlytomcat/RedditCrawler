using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RedditCrawler.DAL;
using RedditCrawler.Utility;
using RedditCrawler.Models;
using FileHelpers;

namespace RedditCrawler.Utility
{
    public class Indexer
    {
        static string INDEX_LOG_PATH = @"CurrentPostIndex.txt";
        public static void UpdateCurrentIndexId(int Id)
        {
            try
            {
                var path = Environment.CurrentDirectory;
                var filePath = $@"{path}/{INDEX_LOG_PATH}";
                File.WriteAllText(filePath, Id.ToString());
            }
            catch
            {
                throw new FileNotFoundException();
            }
        }

        public static void WriteToFile(string text, string filename)
        {
            try
            {
                var path = Environment.CurrentDirectory;
                var filePath = $@"{path}/{filename}";
                Console.WriteLine($"Writing to {filePath}...");
                File.WriteAllText(filePath, text);
            }
            catch
            {
                throw new FileNotFoundException();
            }
        }


        public static int GetCurrentIndexId()
        {
            try
            {
                var path = Environment.CurrentDirectory;
                var filePath = $@"{path}/{INDEX_LOG_PATH}";
                var fileData = string.Empty;
                var fileStream = File.Open(INDEX_LOG_PATH, File.Exists(filePath) ? FileMode.Open : FileMode.OpenOrCreate);
                using(var reader = new StreamReader(fileStream))
                {
                    fileData = reader.ReadLine();
                }
                fileStream.Close();
                return Convert.ToInt32(fileData);
            }
            catch{
                return -1;
            }
        }


        public static void GetDBCSV()
        {
            var posts = PostTable.GetPosts();
            var features = PostTable.GetFeatures();
            var phishposts = PostTable.GetPhishPost();

            var path = Environment.CurrentDirectory;
            var filePath = $@"{path}/{INDEX_LOG_PATH}";

            var feng = new FileHelperEngine<Post>();
            feng.HeaderText = feng.GetFileHeader();
            feng.WriteFile($@"{path}/reddit_posts.csv", posts);

            var phishfeng = new FileHelperEngine<PhishPost>();
            phishfeng.HeaderText = phishfeng.GetFileHeader();
            phishfeng.WriteFile($@"{path}/reddit_phishposts.csv", phishposts);

            var featurefeng = new FileHelperEngine<Feature>();
            featurefeng.HeaderText = featurefeng.GetFileHeader();
            featurefeng.WriteFile($@"{path}/reddit_post_features.csv", features);


            //WriteToFile(postscsv, "reddit_posts.csv");
            //WriteToFile(phishpostscsv, "reddit_phishposts.csv");
            //WriteToFile(featurescsv, "reddit_post_features.csv");

        }

        public static void IndexPhishPost()
        {
            var posts = PostTable.GetPosts();
            var features = PostTable.GetFeatures().Where(x => x.isVTPhish);

            Console.WriteLine($"{features.Count()} phish posts found");

            foreach (var feature in features)
            {
                var post = posts.FirstOrDefault(x => x.PostID == feature.PostID);
                if (post == null) continue;

                PostTable.AddPhishPost(post);
            }
        }

        public static async Task StartIndexer()
        {
            try
            {
                var currentIndex = GetCurrentIndexId();
                var posts = PostTable.GetPosts();

                Console.WriteLine($"{posts.Count()} posts");
                for (var i = currentIndex; i < posts.Count(); i++)
                {
                    Console.WriteLine($"Indexing {posts.ElementAt(i).Title}");
                    Console.WriteLine($"Title: {posts.ElementAt(i).Title}");
                    Console.WriteLine($"Author: {posts.ElementAt(i).Username}");
                    Console.WriteLine($"Subreddit: {posts.ElementAt(i).PostSource}");
                    Console.WriteLine($"Post Id: {posts.ElementAt(i).PostID}");


                    var urls = URLCheck.GetUrls(posts.ElementAt(i).Body);
                    if (urls.Count > 0)
                    {
                        var isPhish = URLCheck.IsPhish(await URLCheck.VirusTotalCheckCallBackAsync(urls[0]));

                        
                        if (isPhish)
                        {
                            PostTable.AddPhishPost(posts.ElementAt(i));
                        }

                        PostTable.AddFeature(posts.ElementAt(i), isPhish);
                    }


                    UpdateCurrentIndexId(i + 1);
                    Console.WriteLine($"Indexed {posts.ElementAt(i).PostID}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Something Went Wrong");
                Console.WriteLine("Error: " + e.Message);
            }

            
        }
    }
}


internal static class ThreadSafeRandom
{
    /// <summary>
    /// The local
    /// </summary>
    [ThreadStatic]
    private static Random _local;

    /// <summary>
    /// Gets the this threads random.
    /// </summary>
    /// <value>The this threads random.</value>
    public static Random ThisThreadsRandom
    {
        get { return _local ?? (_local = new Random(unchecked(Environment.TickCount * 31 + System.Threading.Thread.CurrentThread.ManagedThreadId))); }
    }
}
