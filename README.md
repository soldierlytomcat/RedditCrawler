# RedditCrawler
Reddit crawler and exporter to sql database 


>📋  A template README.md for code accompanying a Machine Learning paper

# RedditCrawler
Reddit post crawler and phish identification implemenation 
This repository is the official implementation of [My Paper Title](https://google.com). 

>📋  Optional: include a graphic explaining your approach/main result, bibtex entry, link to demos, blog posts and tutorials

## Requirements

Visual Studio 2015 or higher
Microsoft SQL Server

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

```train
python train.py --input-data <path_to_data> --alpha 10 --beta 20
```

>📋  Describe how to train the models, with example commands on how to train the models in your paper, including the full training procedure and appropriate hyperparameters.

## Evaluation

To evaluate my model on ImageNet, run:

```eval
python eval.py --model-file mymodel.pth --benchmark imagenet
```

>📋  Describe how to evaluate the trained models on benchmarks reported in the paper, give commands that produce the results (section below).

## Pre-trained Models

You can download pretrained models here:

- [My awesome model](https://drive.google.com/mymodel.pth) trained on ImageNet using parameters x,y,z. 

>📋  Give a link to where/how the pretrained models can be downloaded and how they were trained (if applicable).  Alternatively you can have an additional column in your results table with a link to the models.

## Results

Our model achieves the following performance on :

### [Image Classification on ImageNet](https://paperswithcode.com/sota/image-classification-on-imagenet)

| Model name         | Top 1 Accuracy  | Top 5 Accuracy |
| ------------------ |---------------- | -------------- |
| My awesome model   |     85%         |      95%       |

>📋  Include a table of results from your paper, and link back to the leaderboard for clarity and context. If your main result is a figure, include that figure and link to the command or notebook to reproduce it. 


## Contributing

>📋  Pick a licence and describe how to contribute to your code repository. 
