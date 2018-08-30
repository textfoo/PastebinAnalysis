using System.Threading.Tasks;

namespace PastebinService.Interfaces
{
    public interface IPastebinAnalyzerService
    {
        Task Initialize();
        Task<string[]> ParallelTagAnalysis(string input);
    }
}
