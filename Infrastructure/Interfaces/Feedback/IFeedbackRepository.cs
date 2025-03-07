using Domain.Entities;
using Domain.Filters;
using Infrastructure.Responses;

namespace Infrastructure.Interfaces;

public interface IFeedbackRepository
{
    Task<List<Feedback>> GetAll(BaseFilter filter);
    Task<Feedback> GetById(int id);
    Task<int> Create(Feedback feedback);
    Task<int> Update(Feedback feedback);
    Task<int> Delete(Feedback feedback);
    
}