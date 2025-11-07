using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using MottuChallenge.Api.Extensions;
using MottuChallenge.Application;
using MottuChallenge.Application.Configurations;
using MottuChallenge.Infrastructure;

namespace MottuChallenge.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configs = builder.Configuration.Get<Settings>();
            
            builder.Services.AddInfrastructure(configs);    
            builder.Services.AddUseCases();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwagger(configs.Swagger);
            builder.Services.AddHealthServices(configs.ConnectionStrings);
            builder.Services.AddVersioning();

            var app = builder.Build();
            
            app.UseAuthentication();
            app.UseAuthorization();
            
            

            app.UseSwagger();
            app.UseSwaggerUI(ui =>
                {
                    ui.SwaggerEndpoint("/swagger/v1/swagger.json",  "MottuGrid.API v1");
                    ui.SwaggerEndpoint("/swagger/v2/swagger.json",  "MottuGrid.API v2");
                }
            );

            app.UseHttpsRedirection();

            app.UseAuthorization();
            
            app.MapControllers();
            
            app.MapHealthChecks("/api/health-check", new HealthCheckOptions
            {
                ResponseWriter = HealthCheckExtensions.WriteResponse
            });

            app.Run();
        }
    }
}
