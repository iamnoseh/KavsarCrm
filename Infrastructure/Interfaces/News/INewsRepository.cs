using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface INewsRepository
{
    Task<List<News>> GetAllNews();
    Task<News> GetNewsById(int id);
    Task<int> CreateNews(News news);
    Task<int> UpdateNews(News news);
    Task<int> DeleteNews(News news);
}