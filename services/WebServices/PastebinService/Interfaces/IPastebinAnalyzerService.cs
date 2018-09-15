using System.Threading.Tasks;

namespace PastebinService.Interfaces
{
    public interface IPastebinAnalyzerService
    {
        string MD5Hash(string input);
        Task Initialize();
        Task<string[]> ParallelTagAnalysis(string input);
    }
}
