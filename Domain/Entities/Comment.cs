using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Comment : BaseEntity
    {
        [Required]
        public string Content { get; set; }

        [Required]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        [Required]
        public int NewsId { get; set; }
        public virtual News News { get; set; }

        public virtual List<Like> Likes { get; set; } = new();
    }
}