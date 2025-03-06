using Domain.Dtos.User;
using Domain.Filters;
using Infrastructure.Interfaces;
using Infrastructure.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<Response<GetUserDto>>> GetUser(int id)
    {
        var response = await userService.GetByIdAsync(id);
        if (response.StatusCode != (int)HttpStatusCode.OK)
            return StatusCode(response.StatusCode, response);
        return Ok(response);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<PaginationResponse<List<GetUserDto>>>> GetUsers([FromQuery] BaseFilter filter)
    {
        var response = await userService.GetUsersAsync(filter);
        return Ok(response);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Response<string>>> CreateUser([FromForm] CreateUserDto createUserDto)
    {
        var response = await userService.CreateUserAsync(createUserDto);
        if (response.StatusCode != (int)HttpStatusCode.OK)
            return StatusCode(response.StatusCode, response);
        return Ok(response);
    }

    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Response<string>>> UpdateUser([FromForm] UpdateUserDto updateUserDto)
    {
        var response = await userService.UpdateUserAsync(updateUserDto);
        if (response.StatusCode != (int)HttpStatusCode.OK)
            return StatusCode(response.StatusCode, response);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Response<string>>> DeleteUser(int id)
    {
        var response = await userService.DeleteUserAsync(id);
        if (response.StatusCode != (int)HttpStatusCode.OK)
            return StatusCode(response.StatusCode, response);
        return Ok(response);
    }

    [HttpPut("update-profileImage")]
    // [Authorize(Roles = "Admin")]
    public async Task<Response<string>> UpdateUserProfileImage(int userId, IFormFile profileImage)
    {
        return await userService.UpdateUserProfileImageAsync(userId, profileImage);
    }

    [HttpGet("students")]
    public async Task<PaginationResponse<List<GetUserDto>>> GetStudents()
    {
        return await userService.GetStudentsAsync();
    }

    [HttpGet("student/{id}")]
    public async Task<Response<GetUserDto>> GetStudentById(int id)
    {
        return await userService.GetStudentByIdAsync(id);
    }
    
    [HttpGet("teachers")]
    public async Task<PaginationResponse<List<GetUserDto>>> GetTeachers()
    {
        return await userService.GetTeachersAsync();
    }
    [HttpGet("teacher/{id}")]
    public async Task<Response<GetUserDto>> GetTeacherById(int id)
    {
        return await userService.GetTeacherByIdAsync(id);
    }
}