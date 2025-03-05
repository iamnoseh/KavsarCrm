using System.Linq.Expressions;
using Domain.Entities;
using Domain.Filters;
namespace Infrastructure.Interfaces;

public interface IBannerRepository
{
    Task<List<Banner>> GetAll(Expression<Func<Banner, bool>>? filter = null);
    Task<Banner?> GetBanner(int id);
    Task<int> CreateBanner(Banner banner);
    Task<int> UpdateBanner(Banner banner);
    Task<int> DeleteBanner(int id);
}