using System.Security.Cryptography;
using System.Text;
using MottuChallenge.Domain.Validations;

namespace MottuChallenge.Domain.Entities;

public class Employee
{
    public Guid Id { get; private set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public Yard Yard { get; set; }
    public Guid YardId { get; set;  }
    public string PasswordHash { get; private set; } 
    public string PasswordSalt { get; private set; }

    protected Employee()
    {
        
    }
    public Employee(string name, string email, Yard yard ,string password)
    {
        Guard.AgainstNullOrWhitespace(name, nameof(name), nameof(Employee));
        Guard.AgainstNullOrWhitespace(email, nameof(email), nameof(Employee));
        Guard.AgainstNullOrWhitespace(password, nameof(password), nameof(Employee));
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        Yard = yard;
        YardId = yard.Id;
        CreatePasswordHash(password);
    }
    
    private void CreatePasswordHash(string password) 
    {
        using var hmac = new HMACSHA512();
        PasswordSalt = Convert.ToBase64String(hmac.Key);
        PasswordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
    }
    
    public bool VerifyPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            return false;

        using var hmac = new HMACSHA512(Convert.FromBase64String(PasswordSalt));
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        var computedHashString = Convert.ToBase64String(computedHash);

        return computedHashString == PasswordHash;
    }
    
    public void UpdatePassword(string newPassword)
    {
        Guard.AgainstNullOrWhitespace(newPassword, nameof(newPassword), nameof(Employee));
        CreatePasswordHash(newPassword);
    }

    
}