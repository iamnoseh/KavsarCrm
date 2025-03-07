using Domain.Dtos;
using Domain.Dtos.Comment;
using Domain.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Responses;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class CommentService(ICommentRepository commentRepository, INewsRepository newsRepository) : ICommentService
{
    public async Task<Response<List<GetCommentDto>>> GetAllCommentsAsync()
    {
        var comments = await commentRepository.GetAll();
        var result = comments.Where(c => c.PatternCommentId == null).Select(MapCommentToDto).ToList();
        return new Response<List<GetCommentDto>>(result);
    }

    public async Task<Response<GetCommentDto>> GetCommentByIdAsync(int id)
    {
        var comment = await commentRepository.GetById(id);
        if (comment == null)
            return new Response<GetCommentDto>(System.Net.HttpStatusCode.NotFound, "Comment not found");

        var result = MapCommentToDto(comment);
        return new Response<GetCommentDto>(result);
    }

    public async Task<Response<string>> CreateCommentAsync(CreateCommentDto dto)
    {
        var existingNews = await newsRepository.GetNewsById(dto.NewsId);
        if (existingNews == null)
        {
            return new Response<string>(System.Net.HttpStatusCode.NotFound, "News not found");
        }

        if (dto.PatternCommentId.HasValue)
        {
            var parentComment = await commentRepository.GetById(dto.PatternCommentId.Value);
            if (parentComment == null)
            {
                return new Response<string>(System.Net.HttpStatusCode.NotFound, "Parent comment not found");
            }
        }

        var comment = new Comment
        {
            Content = dto.Content,
            UserId = dto.UserId,
            PatternCommentId = dto.PatternCommentId,
            NewsId = dto.NewsId
        };

        await commentRepository.Create(comment);
        return new Response<string>("Comment created successfully");
    }

    public async Task<Response<string>> UpdateCommentAsync(UpdateCommentDto dto)
    {
        var comment = await commentRepository.GetById(dto.Id);
        if (comment == null)
            return new Response<string>(System.Net.HttpStatusCode.NotFound, "Comment not found");

        comment.Content = dto.Content;
        await commentRepository.Update(comment);

        return new Response<string>("Comment updated successfully");
    }

    public async Task<Response<string>> DeleteCommentAsync(int id)
    {
        var comment = await commentRepository.GetById(id);
        if (comment == null)
            return new Response<string>(System.Net.HttpStatusCode.NotFound, "Comment not found");


        var subComments = await commentRepository.GetSubComments(id);
        foreach (var subComment in subComments)
        {
            await DeleteCommentAsync(subComment.Id); // Нест кардани зеркоммент
        }

        await commentRepository.Delete(comment);
        return new Response<string>("Comment and its sub-comments deleted successfully");
    }


    private GetCommentDto MapCommentToDto(Comment comment)
    {
        return new GetCommentDto
        {
            Id = comment.Id,
            Content = comment.Content,
            UserId = comment.UserId,
            LikeCount = comment.LikeCount,
            PatternCommentId = comment.PatternCommentId,
            NewsId = comment.NewsId,
            SubComments = comment.PatternComments.Select(MapCommentToDto).ToList()
        };
    }
}
