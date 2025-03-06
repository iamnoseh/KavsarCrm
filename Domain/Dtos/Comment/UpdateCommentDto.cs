using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos.Comment
{
    public class UpdateCommentDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }
    }
}