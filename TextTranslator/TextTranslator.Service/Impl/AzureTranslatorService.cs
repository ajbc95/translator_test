using Azure.AI.Translation.Text;
using TextTranslator.Repository.Contracts;
using TextTranslator.Service.Model;

namespace TextTranslator.Service.Impl;

public class AzureTranslatorService(TextTranslationClient _translatorClient, IJobResultsRepository resultsRepository) 
    : TranslatorService(resultsRepository)
{
    public override async Task<TranslationResult?> TranslateAsync(string text, string targetLanguage = "es")
    {
        if (string.IsNullOrWhiteSpace(text))
            return null;

        var response = await _translatorClient.TranslateAsync(targetLanguage, text);
        var translation = response.Value.FirstOrDefault();
        var translatedText = string.Join(Environment.NewLine,
            translation?.Translations?.Select(_ => _.Text)?.AsEnumerable() ?? []) ?? string.Empty;

        await Task.Delay(10000); // Simulate some delay

        return new TranslationResult(
            SourceText: text,
            TranslatedText: translatedText,
            DetectedLanguage: translation?.DetectedLanguage?.Language ?? "unknown",
            TargetLanguage: targetLanguage,
            ConfidencePercentaje: (translation?.DetectedLanguage?.Confidence ?? 0f) * 100
        );
    }
}
