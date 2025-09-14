using Dapper;
using Microsoft.Data.SqlClient;
using TextTranslator.Repository.Contracts;

namespace TextTranslator.Repository.Repositories;

public class JobResultsRepository(SqlConnection connection) : IJobResultsRepository
{
    public Task SaveResultAsync(Guid workId, string result)
    {
        var sql = $@"
            INSERT INTO [Translator].[JobResults] ([workId], [result]) VALUES
            (@workId, @result)";

        return connection.ExecuteAsync(sql, new { workId , result });
    }

    public Task<string?> GetResultAsync(Guid workId)
    {
        var sql = $@"
            SELECT [result]
            FROM [Translator].[JobResults]
            WHERE [workId] = @workId";

        return connection.QuerySingleOrDefaultAsync<string>(sql, new { workId });
    }
}
