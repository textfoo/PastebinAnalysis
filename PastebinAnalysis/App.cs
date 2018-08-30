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

        public App(IMongoDBService mongo, IPastebinService pastebin, IPastebinAnalyzerService pba)
        {
            _mongo = mongo;
            _pastebin = pastebin;
            _pba = pba;
            //test
        }

        public async Task Run()
        {

            await _pba.Initialize();
            var feed = await _pastebin.FetchFeed();
            var array = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<List<PastebinFeedObject>>(feed);

            //testing 
            Thread.Sleep(2000);
            var one = await _pastebin.FetchSingle(array[1].ScrapeUrl);
            await Console.Out.WriteLineAsync($"{Environment.NewLine}Data : {one}");

            var test = await _pba.ParallelTagAnalysis(one);
            await Console.Out.WriteLineAsync($"{Environment.NewLine}Tags : ");
            foreach (var tag in test)
                await Console.Out.WriteLineAsync(tag);
            Console.ReadLine();
        }

    }
}
