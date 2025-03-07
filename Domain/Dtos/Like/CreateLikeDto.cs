using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos;

public class CreateLikeDto
{
    [Required]
    public int UserId { get; set; }
    public int? NewsId { get; set; }
    public int? CommentId { get; set; }
}