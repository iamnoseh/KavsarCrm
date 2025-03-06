using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Request : BaseEntity
{
    [Required]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; }
    [Required]
    public string Phone { get; set; }
    [Required]
    [StringLength(50,MinimumLength = 3,ErrorMessage = "Fullname must be between 3 and 50 characters")]
    public string FullName { get; set; }
    [StringLength(200,MinimumLength = 5, ErrorMessage = "Question must be between 5 and 200 characters")]
    public string Question { get; set; }
}