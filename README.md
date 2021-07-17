# RedditCrawler
Reddit crawler and exporter to sql database 


>📋  A template README.md for code accompanying a Machine Learning paper

# RedditCrawler
Reddit post crawler and phish identification implemenation 
This repository is the official implementation of [My Paper Title](https://google.com). 

>📋  Optional: include a graphic explaining your approach/main result, bibtex entry, link to demos, blog posts and tutorials

## Requirements

- Visual Studio 2015 or higher
- Microsoft SQL Server

### Nuget packages
- Dapper Micro-ORM
- FileHelpers
- LumenWorksCsvReader
- Microsoft.Data.SqlClient
- Newtonsoft.json
- VirusTotalNet

### Python packages
- pandas
- praw
- numpy

To install requirements:

- Move Virual environment folder 'Reddit Post Crawler' (containing /reddit-crawler-env) to the /bin/Debug folder
-


## Flow

- Create a [New reddit app](https://ssl.reddit.com/prefs/apps/)
- Open praw_scrapper.py file in the virual environment (Reddit Post Crawler) and add app credentials to initialize praw
- Activate virual environment and run 'praw_scrapper.py'
- After crawling for posts and saved to new_all_posts.csv (**new and **all are named to indicate the filter values of the scrapper, changeable in praw_scrapper.py)
- Add microsoft server connection credentials to App.config
- Build and Run the Visual Studio project to start indexing and validating posts   

## Results
Resulting datasets and processed dataset [here](https://drive.google.com/drive/folders/1H4hYrTOpoChgHIshN1Fb1FkbzR_N4v-H)

## Contributing

>📋  Pick a licence and describe how to contribute to your code repository. 
