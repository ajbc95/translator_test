using Azure.AI.Translation.Text;
using TextTranslator.Service.Contracts;
using TextTranslator.Service.Model;

namespace TextTranslator.Service.Impl;

public class AzureTranslatorService(TextTranslationClient _translatorClient) : ITranslatorService
{
    public async Task<TranslationResult?> TranslateAsync(string text, string targetLanguage = "es")
    {
        if (string.IsNullOrWhiteSpace(text))
            return null;

        try
        {
            var response = await _translatorClient.TranslateAsync(targetLanguage, text);
            var translation = response.Value.FirstOrDefault();
            var translatedText = string.Join(Environment.NewLine,
                translation?.Translations?.Select(_ => _.Text)?.AsEnumerable() ?? []) ?? string.Empty;

            return new TranslationResult(
                SourceText: text,
                TranslatedText: translatedText,
                DetectedLanguage: translation?.DetectedLanguage?.Language ?? "unknown",
                TargetLanguage: targetLanguage,
                ConfidencePercentaje: (translation?.DetectedLanguage?.Confidence ?? 0f) * 100
            );
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Translation failed: {ex.Message}", ex);
        }
    }
}
