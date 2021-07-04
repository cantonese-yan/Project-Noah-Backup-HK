#!/usr/bin/env python3
from __future__ import unicode_literals
import re
import youtube_dl
import json

f = open('config.json')
config = json.load(f)

class Logger(object):
    def debug(self, msg):
        print(msg)
    def warning(self, msg):
        pass
    def error(self, msg):
        print(msg)

def progressHook(d):
    if d['status'] == 'finished':
        print('Done downloading, now converting ...')


regexList = [
    "^https:\/\/(www\.)*facebook.com\/", 
    "^https:\/\/(www\.)*instagram.com\/tv"
    ]

links = []
with open(config["input_file"], encoding="utf8") as fp:
   line = fp.readline()
   cnt = 1
   while line:
       result = False
       for regex in regexList:
           if re.search(regex, line.strip()):
               result = True
               break               
       if result == True:
           cnt+=1
           print(line.strip())
           links.append(line.strip())
       line = fp.readline()

ydl_opts = {
    'simulate' : False,
    'nooverwrites' : True,
    'restrictfilenames' : True,
    'cookiefile': config["cookie_file"],
    'outtmpl' : "./archive/[%(upload_date)s]-%(id)s.%(ext)s",
    'ignoreerrors' : True,
    'sleep_interval': config["sleep_interval"],
    'max_sleep_interval' : config["max_sleep_interval"],
    'logger': Logger(),
    'progress_hooks': [progressHook],
}

with youtube_dl.YoutubeDL(ydl_opts) as ydl:
    ydl.download(links)
    


