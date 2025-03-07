namespace Domain.Dtos.Feedback;

public class FeedbackUpdateDto
{
    public int Id { get; set; }
    public string TextTj { get; set; }
    public string TextRu { get; set; }
    public string TextEn { get; set; }
    public int Grade { get; set; }
}
