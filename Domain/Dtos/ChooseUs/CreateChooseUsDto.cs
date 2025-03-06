using Microsoft.AspNetCore.Http;

namespace Domain.Dtos;

public class CreateChooseUsDto
{
    public string TitleTj { get; set; }
    public string DescriptionTj { get; set; }
    
    public string TitleRu { get; set; }
    public string DescriptionRu { get; set; }
    
    public string TitleEn { get; set; }
    public string DescriptionEn { get; set; }
    
    public IFormFile Icon { get; set; }
}