namespace MottuChallenge.Application.Configurations;

public class Settings
{
    public ConnectionSettings ConnectionStrings { get; set;  }
    public SwaggerSettings Swagger { get; set; }
    public JwtSettings Jwt { get; set; }
}