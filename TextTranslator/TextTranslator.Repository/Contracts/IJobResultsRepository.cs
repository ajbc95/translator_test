
namespace TextTranslator.Repository.Contracts;

public interface IJobResultsRepository
{
    Task<string?> GetResultAsync(Guid workId);
    Task SaveResultAsync(Guid workId, string result);
}