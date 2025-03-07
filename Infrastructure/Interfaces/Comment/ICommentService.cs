using Domain.Dtos;
using Domain.Dtos.Comment;
using Infrastructure.Responses;

namespace Infrastructure.Interfaces;

public interface ICommentService
{
    Task<Response<List<GetCommentDto>>> GetAllCommentsAsync();
    Task<Response<GetCommentDto>> GetCommentByIdAsync(int id);
    Task<Response<string>> CreateCommentAsync(CreateCommentDto comment);
    Task<Response<string>> UpdateCommentAsync(UpdateCommentDto comment);
    Task<Response<string>> DeleteCommentAsync(int id);
}