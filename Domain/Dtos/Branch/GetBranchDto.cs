namespace Domain.Dtos;

public class GetBranchDto
{
    public int Id { get; set; }
    public string BranchName { get; set; }
    public string Address{ get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
}