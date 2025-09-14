using Azure;
using Azure.AI.Translation.Text;
using Microsoft.OpenApi.Models;
using TextTranslator.Service.Contracts;
using TextTranslator.Service.Impl;

namespace TextTranslator.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAzureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient(services =>
        {
            var apiKey = configuration["AzureTranslator:ApiKey"];
            var region = configuration["AzureTranslator:Region"];

            if (string.IsNullOrWhiteSpace(apiKey) || string.IsNullOrWhiteSpace(region))
                throw new InvalidDataException("Azure Translator API key or region are not configured");

            var credential = new AzureKeyCredential(apiKey);
            return new TextTranslationClient(credential, region);
        });

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<ITranslatorService, AzureTranslatorService>();

        return services;
    }

    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "TextTranslator.Api", Version = "v1" });
        });

        return services;
    }
}
