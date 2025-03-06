using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Domain.Dtos.Account;

public class RegisterDto
{
    [Required]
    [StringLength(50, MinimumLength = 4, ErrorMessage = "Firstname must be between 4 and 50 characters")]
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    [Required]
    [StringLength(50,MinimumLength = 4, ErrorMessage = "Username must be between 4 and 50 characters")]
    public string UserName { get; set; }
    [Required]
    public int Age { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [Required]
    [DataType(DataType.Password)]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }
    
    public IFormFile? ProfileImage { get; set; }
}