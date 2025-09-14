using Hangfire;
using TextTranslator.Api.Extensions;

namespace TextTranslator.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services
                .AddProblemDetails()
                .AddSwagger()
                .AddAzureServices(builder.Configuration) // Register Azure services
                .AddRepositories(builder.Configuration) // Register repositories
                .AddBackgroundTasksManager(builder.Configuration) // Register Hangfire services
                .AddServices() // Register application services
                .AddControllers();

            var app = builder.Build();

            app.UseExceptionHandler()
                .UseStatusCodePages()
                .UseSwagger()
                .UseSwaggerUI()
                .UseHangfireDashboard("/hangfire") // Jobs dashboard
                .UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
