using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface IRequestRepository
{
    Task<List<Request>> GetAll();
    Task<Request?> GetRequest(int id);
    Task<int> CreateRequest(Request request);
    Task<int> UpdateRequest(Request request);
    Task<int> DeleteRequest(Request request);
}