using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Domain.Entities;

public class ChooseUs
{
    public int Id { get; set; }
    [Required]
    [StringLength(100,MinimumLength = 2,ErrorMessage = "Title must be between 2 and 100 characters.")]
    public string TitleTj { get; set; }
    [Required]
    [StringLength(500,MinimumLength = 2,ErrorMessage = "Description must be between 2 and 500 characters.")]
    public string DescriptionTj { get; set; }
    
    [Required]
    [StringLength(100,MinimumLength = 2,ErrorMessage = "Title must be between 2 and 100 characters.")]
    public string TitleRu { get; set; }
    [Required]
    [StringLength(500,MinimumLength = 2,ErrorMessage = "Description must be between 2 and 500 characters.")]
    public string DescriptionRu { get; set; }
    
    [Required]
    [StringLength(100,MinimumLength = 2,ErrorMessage = "Title must be between 2 and 100 characters.")]
    public string TitleEn { get; set; }
    [Required]
    [StringLength(500,MinimumLength = 2,ErrorMessage = "Description must be between 2 and 500 characters.")]
    public string DescriptionEn { get; set; }
    public bool IsDeleted { get; set; } = false;
    
    public string? IconPath { get; set; }
    [NotMapped]
    public IFormFile? Icon { get; set; }
}