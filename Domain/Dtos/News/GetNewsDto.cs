using System;

namespace Domain.Dtos
{
    public class GetNewsDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string MediaUrl { get; set; }

        public int LikeCount { get; set; }

        public int UserId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}