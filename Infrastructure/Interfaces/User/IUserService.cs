using Domain.Dtos.User;
using Infrastructure.Responses;
using Domain.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Interfaces;

public interface IUserService
{
    Task<Response<GetUserDto>> GetByIdAsync(int id);
    Task<PaginationResponse<List<GetUserDto>>> GetUsersAsync(BaseFilter filter);
    Task<Response<string>> CreateUserAsync(CreateUserDto createUserDto);
    Task<Response<string>> UpdateUserAsync(UpdateUserDto updateUserDto);
    Task<Response<string>> DeleteUserAsync(int id);
    Task<Response<string>> UpdateUserProfileImageAsync(int userId, IFormFile profileImage);
    
    Task<PaginationResponse<List<GetUserDto>>> GetStudentsAsync();//where role = "Student"
    Task<Response<GetUserDto>> GetStudentByIdAsync(int id);//where role = "Student"
    Task<PaginationResponse<List<GetUserDto>>> GetTeachersAsync();//where role = "Teacher"
    Task<Response<GetUserDto>> GetTeacherByIdAsync(int id);//where role = "Teacher"
}