using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RequestRepository(DataContext context) : IRequestRepository
{
    public async Task<List<Request>> GetAll()
    {
        return await context.Requests.Where(x => x.IsDeleted == false).ToListAsync();
    }

    public async Task<Request?> GetRequest(int id)
    {
        return await context.Requests.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
    }

    public async Task<int> CreateRequest(Request request)
    {
        await context.Requests.AddAsync(request);
        return await context.SaveChangesAsync();
    }

    public async Task<int> UpdateRequest(Request request)
    {
        context.Requests.Update(request);
        return await context.SaveChangesAsync();
    }

    public async Task<int> DeleteRequest(Request request)
    {
        var r = await context.Requests.FirstOrDefaultAsync(
            x=> x.Id == request.Id && x.IsDeleted == false );
        if (r == null) return 0;
        r.IsDeleted = true;
        r.DeletedAt = DateTime.UtcNow;
        return await context.SaveChangesAsync();
    }
}