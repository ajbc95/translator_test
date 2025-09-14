namespace TextTranslator.Service.Model;

public record TranslationResult(
    string SourceText = "",
    string TranslatedText = "",
    string DetectedLanguage = "",
    string TargetLanguage = "",
    float ConfidencePercentaje = 0f
);
