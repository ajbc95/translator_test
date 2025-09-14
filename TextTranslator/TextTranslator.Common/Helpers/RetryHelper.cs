using Polly;
using TextTranslator.Common;

namespace TextTranslator.Common.Helpers;

public static class RetryHelper
{
    private static readonly Random random = new Random();

    public static Task<T> RetryWithDelayAsync<T>(Func<Task<T>> action)
        => Policy.Handle<Exception>()
            .WaitAndRetryAsync(Constants.Retry.Count, RetryIncremental)
            .WrapAsync(Policy.TimeoutAsync(60))
            .ExecuteAsync(action);

    private static TimeSpan RetryIncremental(int retryAttempt) => TimeSpan.FromSeconds(retryAttempt * Constants.Retry.DelaySeconds + random.Next(0, 5));
}
