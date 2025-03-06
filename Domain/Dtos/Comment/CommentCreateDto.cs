using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos.Comment
{
    public class CommentCreateDto
    {
        [Required]
        public string Content { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int NewsId { get; set; }
    }
}