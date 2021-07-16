using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Data.SqlClient;
using RedditCrawler.Models;
using RedditCrawler.Utility;
using Dapper;
using System.Collections.Specialized;

namespace RedditCrawler.DAL
{
    public abstract class PostTable
    {
        

        public static List<Post> GetPosts()
        {

            var connString = $@"Data Source={Constants.datasource};DataBase={Constants.database};Integrated Security=SSPI;";

            //create instanace of database connection
            using(SqlConnection conn = new SqlConnection(connString))
            {
                return conn.Query<Post>("Select * From Post").ToList();
            }

        
        }

        public static List<Feature> GetFeatures()
        {
            var connString = $@"Data Source={Constants.datasource};DataBase={Constants.database};Integrated Security=SSPI;";

            //create instanace of database connection
            using (SqlConnection conn = new SqlConnection(connString))
            {
                return conn.Query<Feature>("Select * From Feature").ToList();
            }


        }


        public static List<PhishPost> GetPhishPost()
        {
            var connString = $@"Data Source={Constants.datasource};DataBase={Constants.database};Integrated Security=SSPI;";

            //create instanace of database connection
            using (SqlConnection conn = new SqlConnection(connString))
            {
                return conn.Query<PhishPost >("Select * From PhishPost").ToList();
            }


        }

        public static void AddFeature(Post post, bool isVTPhish)
        {
            var connString = $@"Data Source={Constants.datasource};DataBase={Constants.database};Integrated Security=SSPI;";

            //create instanace of database connection
            SqlConnection conn = new SqlConnection(connString);


            try
            {

                //open connection
                conn.Open();


                string processQuery = @"INSERT INTO Feature VALUES (@PostID, @URL, @LinkLength, @IsSecured, @NumDots, @NumSigns, @NumDigits, 
@LenUsername, @IsFile, @NumComments, @UserAge, @Score, @LenTitle, @LenUsernameLetters, @LenUsernameDigits, @LenUsernameSigns, @Username,
@isPhish, @isVTPhish, @isHam)";
                Console.WriteLine("Adding new Feature record");

                conn.Execute(processQuery, new Feature
                {
                    PostID = post.PostID,
                    URL = post.URL,
                    LinkLength = URLCheck.GetUrls(post.Body)[0].Length,
                    IsSecured = URLCheck.GetUrls(post.Body)[0].Contains("https"),
                    NumDots = URLCheck.GetUrls(post.Body)[0].Count(x => x == '.'),
                    NumSigns = URLCheck.GetUrls(post.Body)[0].Count(x => !char.IsLetterOrDigit(x)),
                    NumDigits = URLCheck.GetUrls(post.Body)[0].Count(x => !char.IsDigit(x)),
                    LenUsername = post.Username.Length,
                    IsFile = URLCheck.IsFileExtension(URLCheck.GetUrls(post.Body)[0]),
                    NumComments = post.NumOfComms,
                    //UserAge = (DateTime.Now - DateTime.Parse(post.Date)).Days,
                    UserAge = ThreadSafeRandom.ThisThreadsRandom.Next(16, 38),
                    Score = post.Score,
                    LenTitle = post.Title.Length,
                    LenUsernameLetters = post.Username.Count(x => char.IsLetter(x)),
                    LenUsernameDigits = post.Username.Count(x => char.IsDigit(x)),
                    Username = post.Username,
                    isPhish = isVTPhish,
                    isVTPhish = isVTPhish,
                    isHam = !isVTPhish
                }); 

                Console.WriteLine("Completed!");

            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
            finally
            {
                conn.Close();
            }

            Console.Read();
        }


        public static void AddPhishPost(Post post)
        {
            var connString = $@"Data Source={Constants.datasource};DataBase={Constants.database};Integrated Security=SSPI;";

            //create instanace of database connection
            SqlConnection conn = new SqlConnection(connString);


            try
            {
               
                //open connection
                conn.Open();


                string processQuery = "INSERT INTO PhishPost VALUES (@PostID, @URL, @Title, @PostSource, @NumOfComms, @Date, @Score)";
                Console.WriteLine("Adding new PhishPost record");
                conn.Execute(processQuery, new PhishPost
                {
                    PostID = post.PostID, 
                    URL = post.URL,
                    Title = post.Title,
                    PostSource = post.PostSource,
                    NumOfComms = post.NumOfComms,
                    Date = post.Date,
                    Score = post.Score
                });

                Console.WriteLine("Completed!");

            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
            finally
            {
                conn.Close();
            }

            Console.Read();
        }




        public static void InsertPosts(IEnumerable<Post> allPosts)
        {
            Console.WriteLine("Getting Connection ...");
            string connString = $@"Data Source={Constants.datasource};DataBase={Constants.database};Integrated Security=SSPI;";

            //create instanace of database connection
            SqlConnection conn = new SqlConnection(connString);


            try
            {
                Console.WriteLine("Openning Connection ...");

                //open connection
                conn.Open();
                Console.WriteLine("Connection successful!");
                var count = 0;
                var savedPosts = GetPosts();

                string processQuery = "INSERT INTO Post VALUES (@PostID, @URL, @Title, @Body, @PostSource, @NumOfComms, @Date, @Score, @Username)";
                Console.WriteLine("Inserting Values");
                
                Console.WriteLine("Adding posts to DB ...");

                if(allPosts.Count() == savedPosts.Count)
                {
                    Console.WriteLine($"Completed! Inserted 0 new posts");
                    return;
                }

                var newPosts = allPosts.Where(x => !savedPosts.Exists(y => y.PostID == x.PostID)).ToList();
                conn.Execute(processQuery, newPosts);
                //foreach (var post in allPosts)
                //{
                //    if(!savedPosts.Exists(x => post.PostID == x.PostID))
                //    {
                //        conn.Execute(processQuery, post);
                //        count += 1;
                //    }
                //}

                Console.WriteLine($"Completed! Inserted {newPosts.Count()} new posts");

            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
            finally
            {
                conn.Close();
            }

            Console.Read();
        }
    }
       
}
