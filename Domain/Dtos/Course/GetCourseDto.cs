namespace Domain.Dtos;

public class GetCourseDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Duration { get; set; }
    public string ImagePath { get; set; }
}