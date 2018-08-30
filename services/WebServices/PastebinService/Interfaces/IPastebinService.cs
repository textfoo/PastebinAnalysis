using System;
using System.Threading.Tasks;

namespace PastebinService.Interfaces
{
    public interface IPastebinService
    {
        DateTime LastFetch();
        Task<string> FetchFeed();
        Task<string> FetchSingle(string url);
    }
}
