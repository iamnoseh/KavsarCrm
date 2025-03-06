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

namespace Infrastructure.Services
{
    public class UserService (IUserRepository _userRepository,IMapper _mapper,string _uploadPath) : IUserService
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
    }
}
