using Azure;
using Azure.AI.Translation.Text;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Data.SqlClient;
using Microsoft.OpenApi.Models;
using TextTranslator.Common;
using TextTranslator.Repository.Contracts;
using TextTranslator.Repository.Repositories;
using TextTranslator.Service.Contracts;
using TextTranslator.Service.Impl;

namespace TextTranslator.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAzureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient(services =>
        {
            var apiKey = configuration[Constants.Configuration.AzureTranslatorApiKey];
            var region = configuration[Constants.Configuration.AzureTranslatorRegion];

            if (string.IsNullOrWhiteSpace(apiKey) || string.IsNullOrWhiteSpace(region))
                throw new InvalidDataException("Azure Translator API key or region are not configured");

            var credential = new AzureKeyCredential(apiKey);
            return new TextTranslationClient(credential, region);
        });

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        // DB connection
        var connectionString = configuration.GetConnectionString(Constants.Configuration.ConnectionStrings.Translator);

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidDataException("Translator connection string is not configured");

        services.AddScoped(_ => new SqlConnection(connectionString)
        {
            RetryLogicProvider = SqlConfigurableRetryFactory.CreateIncrementalRetryProvider(new()
            {
                NumberOfTries = 3,
                MinTimeInterval = TimeSpan.FromSeconds(1),
                DeltaTime = TimeSpan.FromSeconds(1),
                MaxTimeInterval = TimeSpan.FromSeconds(5),
                TransientErrors = null
            })
        }); 

        // Repositories
        services.AddScoped<IJobResultsRepository, JobResultsRepository>();

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
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
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "TextTranslator.Api",
                Version = "v1",
                Description = "API for text translation with background job processing. " +
                             "<br/><br/><a href='/hangfire' target='_blank'>📈 View Hangfire Dashboard</a>"
            });
        });

        return services;
    }

    public static IServiceCollection AddBackgroundTasksManager(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(Constants.Configuration.ConnectionStrings.Translator);

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidDataException("Translator connection string is not configured");

        services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(connectionString, new SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                DisableGlobalLocks = true
            }));

        // Configure Hangfire server with explicit options
        services.AddHangfireServer(options =>
        {
            options.ServerName = Environment.MachineName;
            options.WorkerCount = Math.Max(Environment.ProcessorCount, 20);
            options.ServerTimeout = TimeSpan.FromMinutes(4);
            options.SchedulePollingInterval = TimeSpan.FromSeconds(15);
            options.HeartbeatInterval = TimeSpan.FromSeconds(30);
            options.ServerCheckInterval = TimeSpan.FromMinutes(4);
            options.CancellationCheckInterval = TimeSpan.FromSeconds(5);
        });

        return services;
    }
}
