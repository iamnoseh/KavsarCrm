using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ChooseUsRepository (DataContext context) : IChooseUsRepository
{
    public async Task<List<ChooseUs>> GetAll()
    {
        var choose = await context.ChooseUss.Where(x => x.IsDeleted == false).ToListAsync();
        return choose;
    }

    public async Task<ChooseUs> GetChooseUs(int id)
    {
        var choose = await context.ChooseUss.Where(x => x.Id == id).FirstOrDefaultAsync();
        return choose;
    }

    public async Task<int> CreateChooseUs(ChooseUs request)
    {
        await context.ChooseUss.AddAsync(request);
        return await context.SaveChangesAsync();
    }

    public async Task<int> UpdateChooseUs(ChooseUs? request)
    {
        context.ChooseUss.Update(request);
        return await context.SaveChangesAsync();
    }

    public async Task<int> DeleteChooseUs(ChooseUs request)
    {
        var r = await context.ChooseUss.FirstOrDefaultAsync(
            x=> x.Id == request.Id && x.IsDeleted == false );
        if (r == null) return 0;
        r.IsDeleted = true;
        return await context.SaveChangesAsync();
    }
}