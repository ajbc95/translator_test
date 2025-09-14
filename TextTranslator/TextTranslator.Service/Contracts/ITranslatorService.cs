using TextTranslator.Service.Model;

namespace TextTranslator.Service.Contracts;

public interface ITranslatorService
{
    Task<TranslationResult?> TranslateAsync(string text, string targetLanguage = "es");
}