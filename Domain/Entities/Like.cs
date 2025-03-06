using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Like
{
    public int Id { get; set; }
    [Required]
    public int UserId { get; set; }
    public virtual User User { get; set; }
    
    public int? NewsId { get; set; }
    public virtual News? News { get; set; }

    public int? CommentId { get; set; }
    public virtual Comment? Comment { get; set; }
}