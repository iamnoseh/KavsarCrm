using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Domain.Entities
{
    public class News : BaseEntity
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Title cannot be longer than 100 characters.")]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public string MediaUrl { get; set; }

        [NotMapped]
        public IFormFile? Media { get; set; }

        public virtual List<Comment> Comments { get; set; } = new();
        public virtual List<Like> Likes { get; set; } = new();


        [NotMapped]
        public int LikeCount => Likes?.Count ?? 0;
    }
}