using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class BranchRepository (DataContext context) : IBranchRepository
{
    public async Task<List<Branch?>> GetAll()
    {
        var branches = await context.Branches.Where(x=> x.IsDeleted == false).ToListAsync();
        return branches;
    }

    public async Task<Branch?> GetBranch(int id)
    {
        return await context.Branches.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
    }

    public async Task<int> CreateBranch(Branch? request)
    {
        await context.Branches.AddAsync(request);
        return await context.SaveChangesAsync();
    }

    public async Task<int> UpdateBranch(Branch? request)
    {
        context.Branches.Update(request);
        return await context.SaveChangesAsync();
    }

    public async Task<int> DeleteBranch(Branch request)
    {
        var r = await context.Branches.FirstOrDefaultAsync(
            x=> x.Id == request.Id && x.IsDeleted == false );
        if (r == null) return 0;
        r.IsDeleted = true;
        r.DeletedAt = DateTime.UtcNow;
        return await context.SaveChangesAsync();
    }
}