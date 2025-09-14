namespace TextTranslator.Common;

public static class Constants
{
    public static class Retry
    {
        public const int Count = 3;
        public const int DelaySeconds = 2;
    }

    public static class Configuration
    {
        public const string AzureTranslatorApiKey = "AzureTranslator:ApiKey";
        public const string AzureTranslatorRegion = "AzureTranslator:Region";

        public static class ConnectionStrings
        {
            public const string Translator = "Translator";
        }
    }
}
