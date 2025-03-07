using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class NewsRepository(DataContext context) : INewsRepository
{
    public async Task<List<News>> GetAllNews()
    {
        var news = await context.News.ToListAsync();
        return news;
    }

    public async Task<News> GetNewsById(int id)
    {
        var news = await context.News.FirstOrDefaultAsync(x=> x.Id == id);
        return news;
    }

    public async Task<int> CreateNews(News news)
    {
        await context.News.AddAsync(news);
        return await context.SaveChangesAsync();
    }

    public async Task<int> UpdateNews(News news)
    {
        context.News.Update(news);
        return await context.SaveChangesAsync();
    }

    public async Task<int> DeleteNews(News news)
    {
        context.News.Remove(news);
        return await context.SaveChangesAsync();
    }
}