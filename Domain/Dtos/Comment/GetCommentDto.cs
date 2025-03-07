using System;

namespace Domain.Dtos;

public class GetCommentDto
{
    public int Id { get; set; }
    public string Content { get; set; }
    public int UserId { get; set; }
    public int LikeCount { get; set; }
    public int? PatternCommentId { get; set; }
    public int NewsId { get; set; }
    public List<GetCommentDto> SubComments { get; set; } = new ();
}