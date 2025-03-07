using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos;
public class CreateCommentDto
{
    [Required]
    public string Content { get; set; }
    [Required]
    public int UserId { get; set; }
    public int? PatternCommentId { get; set; }
    [Required]
    public int NewsId { get; set; }
}