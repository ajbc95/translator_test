
using TextTranslator.Extensions;

namespace TextTranslator
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
                .AddControllers();

            var app = builder.Build();

            //if (app.Environment.IsDevelopment())
            //{
                
            //}

            app.UseExceptionHandler()
                .UseStatusCodePages()
                .UseSwagger()
                .UseSwaggerUI()
                .UseAuthorization();
            
            app.MapControllers();

            app.Run();
        }
    }
}
