using AutoMapper;
using Domain.Dtos.User;
using Domain.Entities;
using Domain.Filters;
using Infrastructure.Interfaces;
using Infrastructure.Responses;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services;

public class UserService(
    IUserRepository _userRepository,
    IMapper _mapper,
    IHttpContextAccessor _httpContextAccessor,
    UserManager<User> _userManager,
    string _uploadPath) : IUserService
{
    public async Task<Response<string>> CreateUserAsync(CreateUserDto createUserDto)
    {
        var user = _mapper.Map<User>(createUserDto);
        user.RegistrationDate = DateTime.UtcNow;
        if (createUserDto.ProfileImage != null && createUserDto.ProfileImage.Length > 0)
        {
            var fileExtension = Path.GetExtension(createUserDto.ProfileImage.FileName).ToLowerInvariant();
            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(_uploadPath, uniqueFileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await createUserDto.ProfileImage.CopyToAsync(stream);
            }

            user.ProfileImagePath = $"/uploads/profiles/{uniqueFileName}";
        }

        await _userRepository.AddAsync(user);
        return new Response<string>("User created successfully");
    }

    public async Task<Response<string>> DeleteUserAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            return new Response<string>(HttpStatusCode.NotFound, "User not found");

        await _userRepository.DeleteAsync(user);
        return new Response<string>("User deleted successfully");
    }

    public async Task<Response<GetUserDto>> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            return new Response<GetUserDto>(HttpStatusCode.NotFound, "User not found");

        var userDto = _mapper.Map<GetUserDto>(user);
        return new Response<GetUserDto>(userDto);
    }

    public async Task<PaginationResponse<List<GetUserDto>>> GetUsersAsync(BaseFilter filter)
    {
        var users = await _userRepository.GetAllAsync(filter);
        var totalRecords = await _userRepository.CountAsync(filter);
        var userDtos = _mapper.Map<List<GetUserDto>>(users);
        return new PaginationResponse<List<GetUserDto>>(userDtos, totalRecords, filter.PageNumber, filter.PageSize);
    }

    public async Task<Response<string>> UpdateUserAsync(UpdateUserDto updateUserDto)
    {
        var user = await _userRepository.GetByIdAsync(updateUserDto.Id);
        if (user == null)
            return new Response<string>(HttpStatusCode.NotFound, "User not found");
        _mapper.Map(updateUserDto, user);

        if (updateUserDto.ProfileImage != null && updateUserDto.ProfileImage.Length > 0)
        {
            var fileExtension = Path.GetExtension(updateUserDto.ProfileImage.FileName).ToLowerInvariant();
            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(_uploadPath, uniqueFileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await updateUserDto.ProfileImage.CopyToAsync(stream);
            }

            user.ProfileImagePath = $"/uploads/profiles/{uniqueFileName}";
        }

        await _userRepository.UpdateAsync(user);
        return new Response<string>("User updated successfully");
    }

    public async Task<Response<string>> UpdateUserProfileImageAsync(int userId, IFormFile profileImage)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return new Response<string>(HttpStatusCode.NotFound, "User not found");
        
        var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
        if (currentUser == null)
            return new Response<string>(HttpStatusCode.Unauthorized, "Unauthorized");
        //mesanjem ki user admin ast
        var isAdmin = await _userManager.IsInRoleAsync(currentUser, "Admin");

        // tanho admin va sohibi sayt baroi ivaz kardan haq dorand
        if (!isAdmin && currentUser.Id != userId)
            return new Response<string>(HttpStatusCode.Forbidden,
                "You are not allowed to update this user's profile image");

        if (profileImage == null || profileImage.Length == 0)
            return new Response<string>(HttpStatusCode.BadRequest, "Profile image is required");

        const long maxFileSize = 5 * 1024 * 1024;
        if (profileImage.Length > maxFileSize)
            return new Response<string>(HttpStatusCode.BadRequest, "Image file size must be less than 5MB");

        var allowedExtensions = new HashSet<string> { ".jpg", ".jpeg", ".png", ".gif" };
        var fileExtension = Path.GetExtension(profileImage.FileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(fileExtension))
            return new Response<string>(HttpStatusCode.BadRequest,
                "Invalid image format. Allowed: .jpg, .jpeg, .png, .gif");

        var uploadsFolder = Path.Combine(_uploadPath, "uploads", "profiles");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        // Agar rasm doshta boshad onro nest nekunem
        if (!string.IsNullOrEmpty(user.ProfileImagePath))
        {
            var oldFilePath = Path.Combine(_uploadPath, user.ProfileImagePath.TrimStart('/'));
            if (File.Exists(oldFilePath))
                File.Delete(oldFilePath);
        }

        var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
        var newFilePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var stream = new FileStream(newFilePath, FileMode.Create))
        {
            await profileImage.CopyToAsync(stream);
        }

        user.ProfileImagePath = $"/uploads/profiles/{uniqueFileName}";
        await _userRepository.UpdateAsync(user);

        return new Response<string>("Profile image updated successfully");
    }
    public async Task<PaginationResponse<List<GetUserDto>>> GetStudentsAsync()
    {
        var filter = new BaseFilter();
        var students = await _userRepository.GetUsersByRoleAsync("Student", filter);
        var totalRecords = await _userRepository.CountUsersByRoleAsync("Student");

        var studentDtos = _mapper.Map<List<GetUserDto>>(students);
        return new PaginationResponse<List<GetUserDto>>(studentDtos, totalRecords, filter.PageNumber, filter.PageSize);
    }

    public async Task<PaginationResponse<List<GetUserDto>>> GetTeachersAsync()
    {
        var filter = new BaseFilter();
        var teachers = await _userRepository.GetUsersByRoleAsync("Teacher", filter);
        if (!teachers.Any())
        {
            return new PaginationResponse<List<GetUserDto>>(HttpStatusCode.NotFound, "Teachers not found");
        }
        var totalRecords = await _userRepository.CountUsersByRoleAsync("Teacher");

        var teacherDtos = _mapper.Map<List<GetUserDto>>(teachers);
        return new PaginationResponse<List<GetUserDto>>(teacherDtos, totalRecords, filter.PageNumber, filter.PageSize);
    }
    public async Task<Response<GetUserDto>> GetStudentByIdAsync(int id)
    {
        var student = await _userRepository.GetUserByIdAsync(id, "Student");
        if (student == null)
            return new Response<GetUserDto>(HttpStatusCode.NotFound,"Student not found");

        var studentDto = _mapper.Map<GetUserDto>(student);
        return new Response<GetUserDto>(studentDto);
    }

    public async Task<Response<GetUserDto>> GetTeacherByIdAsync(int id)
    {
        var teacher = await _userRepository.GetUserByIdAsync(id, "Teacher");
        if (teacher == null)
            return new Response<GetUserDto>(HttpStatusCode.NotFound,"Teacher not found");

        var teacherDto = _mapper.Map<GetUserDto>(teacher);
        return new Response<GetUserDto>(teacherDto);
    }
}