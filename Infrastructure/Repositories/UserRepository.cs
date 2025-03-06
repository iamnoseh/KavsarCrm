using Domain.Entities;
using Domain.Filters;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Repositories;

public class UserRepository(DataContext context, UserManager<User> _userManager) : IUserRepository
{
    public async Task AddAsync(User user)
    {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(User user)
    {
        context.Users.Remove(user);
        await context.SaveChangesAsync();
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await context.Users.ToListAsync();
    }

    public async Task<List<User>> GetAllAsync(BaseFilter filter)
    {
        return await context.Users
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();
    }

    public async Task<int> CountAsync(BaseFilter filter)
    {
        return await context.Users.CountAsync();
    }

    public async Task<User> GetByIdAsync(int id)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task UpdateAsync(User user)
    {
        context.Users.Update(user);
        await context.SaveChangesAsync();
    }
    
    public async Task<List<User>> GetUsersByRoleAsync(string role, BaseFilter filter)
    {
        var users = await context.Users
            .Where(u => context.UserRoles.Any(ur => ur.UserId == u.Id && 
                                                     context.Roles.Any(r => r.Id == ur.RoleId && r.Name == role)))
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return users;
    }

    public async Task<int> CountUsersByRoleAsync(string role)
    {
        return await context.Users
            .CountAsync(u => context.UserRoles.
                Any(ur => ur.UserId == u.Id &&
                          context.Roles.Any(r => r.Id == ur.RoleId && r.Name == role)));
    }
    
    public async Task<User?> GetUserByIdAsync(int id, string role)
    {
        var user = await context.Users
            .Where(u => u.Id == id)
            .FirstOrDefaultAsync();

        if (user == null)
            return null;

        var roles = await _userManager.GetRolesAsync(user);
        if (!roles.Contains(role))
            return null;

        return user;
    }

}