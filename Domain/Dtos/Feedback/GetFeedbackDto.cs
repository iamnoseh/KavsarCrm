namespace Domain.Dtos.Feedback;



public class FeedbackGetDto
{
    public int Id { get; set; }
    public string Text { get; set; }
    public int Grade { get; set; }
    public string FullName { get; set; }
    public string? ProfileImagePath { get; set; }
}

