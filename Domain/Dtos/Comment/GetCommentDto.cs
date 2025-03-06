using System;

namespace Domain.Dtos.Comment
{
    public class GetCommentDto
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public int UserId { get; set; }

        public int LikeCount { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}