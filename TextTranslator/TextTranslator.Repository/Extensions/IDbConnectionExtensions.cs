using Dapper;
using System.Data;
using TextTranslator.Common.Helpers;

namespace TextTranslator.Repository.Extensions;

public static class IDbConnectionExtensions
{
    public static Task<T?> QuerySingleOrDefaultWithRetriesAsync<T>(this IDbConnection dbConnection, string sql, object? param = null)
        => RetryHelper.RetryWithDelayAsync(() => dbConnection.QuerySingleOrDefaultAsync<T>(sql, param));

    public static Task<int> ExecuteWithRetriesAsync(this IDbConnection dbConnection, string sql, object? param = null)
        => RetryHelper.RetryWithDelayAsync(() => dbConnection.ExecuteAsync(sql, param));
}
