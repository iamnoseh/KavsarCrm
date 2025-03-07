using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Feedback : BaseEntity
{
    [Required]
    [StringLength(500, MinimumLength = 5, ErrorMessage = "Feedback must be between 5 and 500 characters.")]
    public string TextTj { get; set; }
    public string TextRu { get; set; }
    public string TextEn { get; set; }
    
    [Required]
    [StringLength(80, MinimumLength = 3, ErrorMessage = "Fullname must be between 3 and 80 characters.")]
    public string FullName { get; set; }
    
    [Required]
    [Range(1,5, ErrorMessage = "Range must be between 1 and 5")]
    public int Grade { get; set; }
    
    public string? ProfileImagePath { get; set; }  // rasmi user agar doshta boshad
    
    public int? UserId { get; set; }          
    public User? User { get; set; }
}
