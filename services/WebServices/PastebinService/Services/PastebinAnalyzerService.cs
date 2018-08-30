using MongoDBService.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using PastebinService.Interfaces;
using PastebinService.Models;

namespace PastebinService.Services
{
    public class PastebinAnalyzerService : IPastebinAnalyzerService
    {
        private List<TagAnalysisTemplateModel> _tagTemplates;

        private readonly IMongoDBService _mongo;

        public PastebinAnalyzerService(IMongoDBService mongo)
        {
            _mongo = mongo;
        }

        public async Task<string[]> ParallelTagAnalysis(string input)
        {
            List<Task<string>> tasks = new List<Task<string>>();
            _tagTemplates.ForEach(x =>
            {
                switch (x.Action)
                {
                    case "contains":
                        tasks.Add(TagTemplateContainsAction(input, x));
                        break;
                }

            });
            var response = await Task.WhenAll(tasks);
            return response.Where(x => !string.IsNullOrEmpty(x)).ToArray();
        }

        public async Task Initialize()
        {
            await PopulateTagActions();
        }

        private async Task PopulateTagActions()
        {
            _tagTemplates =
                await _mongo.Fetch<TagAnalysisTemplateModel>("pastebin", "tag_analysis", new MongoDB.Bson.BsonDocument());
        }

        private async Task<string> TagTemplateContainsAction(string input, TagAnalysisTemplateModel tagModel)
        {
            string response = null;
            await Task.Run(() =>
            {
                for (int i = 0; i < tagModel.Keywords.Length; i++)
                {
                    if (!string.IsNullOrEmpty(response))
                        break;
                    if (input.ToLower().Contains(tagModel.Keywords[i]))
                        response = tagModel.Tag;
                }
            });
            return response;
        }
    }
}
