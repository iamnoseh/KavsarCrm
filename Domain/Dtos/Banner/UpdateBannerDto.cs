using Microsoft.AspNetCore.Http;

namespace Domain.Dtos.BannerDto;
public class UpdateBannerDto
{
    public int Id { get; set; }
    public string? TitleTj { get; set; }
    public string? TitleRu { get; set; }
    public string? TitleEn { get; set; }
    public string? DescriptionTj { get; set; }
    public string? DescriptionRu { get; set; }
    public string? DescriptionEn { get; set; }
    public IFormFile? ImageFile { get; set; }
}