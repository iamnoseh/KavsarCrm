using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Domain.Dtos;
public class UpdateNewsDto
{
    public int Id { get; set; }
    [Required, MaxLength(100)]
    public string TitleTj { get; set; }
    [Required, MaxLength(100)]
    public string TitleRu { get; set; }
    [Required, MaxLength(100)]
    public string TitleEn { get; set; }

    [Required]
    public string ContentTj { get; set; }
    [Required]
    public string ContentEn { get; set; }
    [Required]
    public string ContentRu { get; set; }
    
    public IFormFile Media { get; set; }
}