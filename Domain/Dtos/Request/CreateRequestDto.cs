namespace Domain.Dtos.Request;

public class CreateRequestDto
{
    public string Email { get; set; }
    public string Phone { get; set; }
    public string FullName { get; set; }
    public string Question { get; set; }
}