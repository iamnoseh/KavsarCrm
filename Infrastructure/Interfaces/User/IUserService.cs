using Domain.Dtos.User;
using Infrastructure.Responses;
using Domain.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces;

public interface IUserService
{
    Task<Response<GetUserDto>> GetByIdAsync(int id);
    Task<PaginationResponse<List<GetUserDto>>> GetUsersAsync(BaseFilter filter);
    Task<Response<string>> CreateUserAsync(CreateUserDto createUserDto);
    Task<Response<string>> UpdateUserAsync(UpdateUserDto updateUserDto);
    Task<Response<string>> DeleteUserAsync(int id);
}