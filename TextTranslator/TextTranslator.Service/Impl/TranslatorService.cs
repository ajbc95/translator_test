using Hangfire;
using TextTranslator.Common.Helpers;
using TextTranslator.Repository.Contracts;
using TextTranslator.Service.Contracts;
using TextTranslator.Service.Model;

namespace TextTranslator.Service.Impl;

public abstract class TranslatorService(IJobResultsRepository resultsRepository) : ITranslatorService
{
    public abstract Task<TranslationResult?> TranslateAsync(string text, string targetLanguage = "es");

    public Guid TranslateJob(string? text, string targetLanguage = "es")
    {
        var workId = Guid.NewGuid();
        BackgroundJob.Enqueue(() => TranslateAndSaveResultAsync(workId, text, targetLanguage));
        return workId;
    }

    public async Task<TranslationResult?> GetTranslationResult(Guid workId)
    {
        var serializedResult = await resultsRepository.GetResultAsync(workId);

        if (!string.IsNullOrWhiteSpace(serializedResult))
            return System.Text.Json.JsonSerializer.Deserialize<TranslationResult>(serializedResult);

        return null;
    }

    public async Task TranslateAndSaveResultAsync(Guid workId, string? text, string targetLanguage)
    {
        try
        {
            var translation = new TranslationResult();

            if (!string.IsNullOrWhiteSpace(text))
                translation = await RetryHelper.RetryWithDelayAsync(() => TranslateAsync(text, targetLanguage));

            var serializedResult = System.Text.Json.JsonSerializer.Serialize(translation);

            await resultsRepository.SaveResultAsync(workId, serializedResult);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during translation: {ex.Message}");
        }
    }
}
