using System.ComponentModel.DataAnnotations;
namespace Domain.Dtos.Feedback;

public class FeedbackCreateDto
{

    public string TextTj { get; set; }
    public string TextRu { get; set; }
    public string TextEn { get; set; }
    public int Grade { get; set; }
    public string? FullName { get; set; }
}
