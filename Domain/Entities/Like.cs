using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Like
{
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int? NewsId { get; set; }
    public News? News { get; set; }

    public int? CommentId { get; set; }
    public Comment? Comment { get; set; }

    [NotMapped]
    public bool IsForNews => NewsId.HasValue && !CommentId.HasValue;

    [NotMapped]
    public bool IsForComment => CommentId.HasValue && !NewsId.HasValue;
}