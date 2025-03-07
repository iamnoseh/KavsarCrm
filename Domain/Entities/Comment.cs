using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Comment : BaseEntity
    {
        [Required]
        public string Content { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int LikeCount => Likes?.Count ?? 0;

        public int? PatternCommentId { get; set; }  
        [ForeignKey("PatternCommentId")]
        public Comment? PatternComment { get; set; }

        public List<Comment> PatternComments { get; set; } = new();  

        [Required]
        public int NewsId { get; set; }
        public News News { get; set; } = null!;

        public List<Like> Likes { get; set; } = new();
    }
}