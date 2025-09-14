using TextTranslator.Service.Model;

namespace TextTranslator.Service.Contracts;

public interface ITranslatorService
{
    Task<TranslationResult?> GetTranslationResult(Guid workId);
    Task<TranslationResult?> TranslateAsync(string text, string targetLanguage = "es");
    Guid TranslateJob(string? text, string targetLanguage = "es");
}