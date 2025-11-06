using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MottuChallenge.Application.Configurations;

namespace MottuChallenge.Api.Extensions;

public static class HealthCheckExtensions
{
    
    public static Task WriteResponse(HttpContext context, HealthReport report)
    {
        JsonSerializerOptions jsonSerializerOptions = new()
        {
            WriteIndented = false,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        var json = JsonSerializer.Serialize(
            new
            {
                Status = report.Status.ToString(),
                Duration = report.TotalDuration,
                Info = report.Entries.Select(entry =>
                    new
                    {
                        entry.Key,
                        entry.Value.Description,
                        entry.Value.Duration,
                        Status = Enum.GetName(typeof(HealthStatus), entry.Value.Status),
                        Error = entry.Value.Exception?.Message,
                        entry.Value.Data
                    }).ToList()
            }, jsonSerializerOptions);

        context.Response.ContentType = MediaTypeNames.Application.Json;

        return context.Response.WriteAsync(json);
    }
    
    public static IServiceCollection AddHealthServices(this IServiceCollection services, ConnectionSettings connectionSettings)
    {
        services
            .AddHealthChecks()
            .AddMySql(connectionSettings.MysqlConnection, name: "mysql connection")
            .AddUrlGroup(new Uri("https://viacep.com.br/"), name: "Via Cep API");
        
        return services;
    }
    
    
}