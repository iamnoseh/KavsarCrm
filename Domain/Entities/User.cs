using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class User : IdentityUser<int>
{
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    public int Age { get; set; }
    public string Address { get; set; }

    public DateTime BirthDate { get; set; }
    public bool IsActive { get; set; }
    public DateTime RegistrationDate { get; set; } 
    [NotMapped]
    public IFormFile? ProfileImage { get; set; }
    public string? ProfileImagePath { get; set; }
    
    public List<Like> Likes { get; set; } = new();
    public List<Comment> Comments { get; set; } = new();
    public List<News> News { get; set; } = new();
    public List<Feedback> Feedbacks { get; set; } = new();
}