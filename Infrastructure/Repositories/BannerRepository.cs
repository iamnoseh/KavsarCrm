using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class BannerRepository(
    DataContext context,
    ILogger<BannerRepository> logger) : IBannerRepository
{
    public async Task<List<Banner>> GetAll(Expression<Func<Banner, bool>>? filter = null)
    {
        var query = context.Banners.AsQueryable();
        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await query.ToListAsync();
    }

    public async Task<Banner?> GetBanner(int id)
    {
        return await context.Banners.FindAsync(id);
    }

    public async Task<int> CreateBanner(Banner banner)
    {
        try
        {
            await context.Banners.AddAsync(banner);
            return await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error creating banner");
            return 0;
        }
    }

    public async Task<int> UpdateBanner(Banner banner)
    {
        try
        {
            context.Banners.Update(banner);
            return await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error updating banner");
            return 0;
        }
    }

    public async Task<int> DeleteBanner(int id)
    {
        try
        {
            var banner = await GetBanner(id);
            if (banner == null) return 0;
            context.Banners.Remove(banner);
            return await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error deleting banner");
            return 0;
        }
    }
}

