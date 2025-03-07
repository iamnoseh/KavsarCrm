using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface ICommentRepository
{
    Task<List<Comment>> GetAll();
    Task<Comment> GetById(int id);
    Task<int> Create(Comment comment);
    Task<int> Update(Comment comment);
    Task<int> Delete(Comment comment);
}