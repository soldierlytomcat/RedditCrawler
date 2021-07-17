import praw
import pprint
import pandas as pd
import datetime as dt


def get_date(created):
    return dt.datetime.fromtimestamp(created)

reddit = praw.Reddit(client_id='SbxILTZt4J3j_w', \
                     client_secret='yI93RzUFonzN1VBAd3fT2_CspIqJlA', \
                     user_agent='post_crawler', \
                     username='redditdev123', \
                     password='redditdev@123')

subreddit = reddit.subreddit('all')
posts = []

for post in subreddit.new(limit=10):
    posts.append([post.title, post.score, post.id, post.subreddit, \
    post.url, post.num_comments, post.selftext, post.created, \
    post.upvote_ratio, post.ups, post.downs])


posts = pd.DataFrame(posts,columns=['title', 'score', 'id', 'subreddit', 'url', 'num_comments', 'body', 'created', 'upvote_ratio', 'ups', 'downs'])

# column formating
_timestamp = posts["created"].apply(get_date)
posts = posts.assign(timestamp = _timestamp)

print(posts)

# top_subreddit = subreddit.top(limit=1)

# topics_dict = { "title":[], \
#                 "score":[], \
#                 "id":[], \
#                 "url":[], \ 
#                 "comms_num": [], \
#                 "created": [], \
#                 "body":[]}


# for submission in top_subreddit:
#     topics_dict["title"].append(submission.title)
#     topics_dict["score"].append(submission.score)
#     topics_dict["id"].append(submission.id)
#     topics_dict["url"].append(submission.url)
#     topics_dict["comms_num"].append(submission.num_comments)
#     topics_dict["created"].append(submission.created)
#     topics_dict["body"].append(submission.selftext)

# for submission in subreddit.top(limit=1):
#     print(submission.title)  # to make it non-lazy
#     pprint.pprint(vars(submission))

# submission = reddit.submission(id="f2xuxt")
# print(submission.title)  # to make it non-lazy
# pprint.pprint(vars(submission))