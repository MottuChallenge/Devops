namespace MottuChallenge.Application.DTOs.Request;

public class CreateEmployeeRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
    public Guid YardId { get; set;  }
    public string Password { get; set; }
    
}