﻿using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDBService.Interfaces;
using PastebinService.Interfaces;
using PastebinService.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PastebinAnalysis
{
    public class App
    {
        private readonly IMongoDBService _mongo;
        private readonly IPastebinService _pastebin;
        private readonly IPastebinAnalyzerService _pba;
        private readonly ILogger<App> _log;

        private static HashSet<string> _keyCache { get; set; } = new HashSet<string>();
        private static HashSet<string> _hashCache { get; set; } = new HashSet<string>(); 

        private static List<PastebinObjectModel> modelCache { get; set; } = new List<PastebinObjectModel>(); 

        public App(IMongoDBService mongo, IPastebinService pastebin, IPastebinAnalyzerService pba, ILogger<App> log)
        {
            _mongo = mongo;
            _pastebin = pastebin;
            _pba = pba;
            _log = log; 
        }

        public async Task Run()
        {

            bool exit = false; 
            await Setup(); 

            while(!exit)
            {
                try
                {
                    var feed = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<List<PastebinFeedObject>>(await _pastebin.FetchFeed());
                    _log.LogInformation($"Fetched {feed.Count} items from pastebin feed.");
                    await Task.Delay(5000); 
                    foreach(var feedItem in feed)
                    {
                        if (!_keyCache.Contains(feedItem.Key))
                        {
                            var pbRaw = await _pastebin.FetchSingle(feedItem.ScrapeUrl);
                            var hash = _pba.MD5Hash(pbRaw);
                            if (!_hashCache.Contains(hash))
                            {
                                _log.LogInformation($"Conducting Analysis on {hash} : {feedItem.Title}, {feedItem.FullUrl}");
                                var tags = await _pba.ParallelTagAnalysis(pbRaw);

                                modelCache.Add(new PastebinObjectModel()
                                {
                                    Id = ObjectId.GenerateNewId(),
                                    Title = feedItem.Title,
                                    Key = hash,
                                    FullUrl = feedItem.FullUrl,
                                    ScrapeUrl = feedItem.ScrapeUrl,
                                    Size = feedItem.Size,
                                    Tags = tags,
                                    Contents = pbRaw
                                });
                                _keyCache.Add(feedItem.Key);
                                _hashCache.Add(hash); 
                            }
                            else
                                _log.LogInformation($"Ignoring {hash} : {feedItem.Title}, {feedItem.FullUrl}");
                            await Task.Delay(1500); 
                        }
                    }
                }
                catch(Exception ex)
                {
                    _log.LogCritical($"Exception thrown, exiting.{Environment.NewLine}{ex.Message}");
                    exit = true; 
                }
                finally
                {
                    _log.LogInformation("Caching...");
                    if (modelCache.Count > 0)
                        await _mongo.InsertMany("pastebin", "pastes", modelCache);
                    modelCache.Clear();
                    Console.Clear(); 
                }
            }
        }


        /// <summary>
        /// Since it's bad form to make database calls within constructors
        /// We need to manually initialize some components
        /// </summary>
        /// <returns></returns>
        public async Task Setup()
        {
            await _pba.Initialize();
            //need to add a function to instantiate the key cache
            await FetchPastHashes(); 
        }

        private async Task FetchPastHashes()
        {
            var col = _mongo.GetCollection<BsonDocument>("pastebin", "pastes");
            var hashes = await col.Find(Builders<BsonDocument>.Filter.Empty)
                .Project("{pb_key : 1}")
                .Sort(new BsonDocument("pb_key", 1))
                .ToListAsync();
            hashes.ForEach(x =>
            {
                _hashCache.Add((string)x["pb_key"]);
            }); 
        }
    }
}
