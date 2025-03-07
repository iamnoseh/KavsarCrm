using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CommentRepository (DataContext context) : ICommentRepository
{
    public async Task<List<Comment>> GetAll()
    {
        return await context.Comments.ToListAsync();
    }

    public async Task<Comment> GetById(int id)
    {
        return await context.Comments.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<int> Create(Comment comment)
    {
        await context.Comments.AddAsync(comment);
        return await context.SaveChangesAsync();
    }

    public async Task<int> Update(Comment comment)
    {
        context.Comments.Update(comment);
        return await context.SaveChangesAsync();
    }

    public async Task<int> Delete(Comment comment)
    {
        context.Comments.Remove(comment);
        return await context.SaveChangesAsync();
    }
    public async Task<List<Comment>> GetSubComments(int parentId)
    {
        return await context.Comments.Where(c => c.PatternCommentId == parentId).ToListAsync();
    }
    

}