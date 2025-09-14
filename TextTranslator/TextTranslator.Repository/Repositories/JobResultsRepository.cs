using Microsoft.Data.SqlClient;
using TextTranslator.Repository.Contracts;
using TextTranslator.Repository.Extensions;

namespace TextTranslator.Repository.Repositories;

public class JobResultsRepository(SqlConnection connection) : IJobResultsRepository
{
    public Task SaveResultAsync(Guid workId, string result)
    {
        var sql = $@"
            INSERT INTO [Translator].[JobResults] ([workId], [result]) VALUES
            (@workId, @result)";

        return connection.ExecuteWithRetriesAsync(sql, new { workId, result });
    }

    public Task<string?> GetResultAsync(Guid workId)
    {
        var sql = $@"
            SELECT [result]
            FROM [Translator].[JobResults]
            WHERE [workId] = @workId";

        return connection.QuerySingleOrDefaultWithRetriesAsync<string>(sql, new { workId });
    }
}
