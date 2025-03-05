using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class User : IdentityUser<int>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public DateTime BirthDate { get; set; }
    public bool IsActive { get; set; }
    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
}