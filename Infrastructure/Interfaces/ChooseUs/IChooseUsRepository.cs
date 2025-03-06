using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface IChooseUsRepository
{
    Task<List<ChooseUs>> GetAll();
    Task<ChooseUs> GetChooseUs(int id);
    Task<int> CreateChooseUs(ChooseUs? request);
    Task<int> UpdateChooseUs(ChooseUs? request);
    Task<int> DeleteChooseUs(ChooseUs request);
}