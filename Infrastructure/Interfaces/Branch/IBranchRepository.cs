using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface IBranchRepository
{
    Task<List<Branch?>> GetAll();
    Task<Branch?> GetBranch(int id);
    Task<int> CreateBranch(Branch? request);
    Task<int> UpdateBranch(Branch? request);
    Task<int> DeleteBranch(Branch request);
}