using Domain.Entities;
using Domain.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces;

public interface IUserRepository
{
    Task<User> GetByIdAsync(int id);
    Task<List<User>> GetAllAsync();
    Task<List<User>> GetAllAsync(BaseFilter filter);
    Task<int> CountAsync(BaseFilter filter);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(User user);
    
    Task<List<User>> GetUsersByRoleAsync(string role, BaseFilter filter);
    Task<int> CountUsersByRoleAsync(string role);
    Task<User?> GetUserByIdAsync(int id, string role);
}