using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
namespace Domain.Dtos;

public class CreateCourseDto
{
    [Required]
    public string NameTj { get; set; }
    public string NameRu { get; set; }
    public string NameEn { get; set; }
    public string DescriptionTj { get; set; }
    public string DescriptionRu { get; set; }
    public string DescriptionEn { get; set; }
    public decimal Price { get; set; }
    public int Duration { get; set; }
    
    [NotMapped]
    public IFormFile Image { get; set; }
}