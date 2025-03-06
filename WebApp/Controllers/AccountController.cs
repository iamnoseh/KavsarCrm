using Domain.Dtos.Account;
using Domain.Dtos.Auth;
using Infrastructure.Interfaces.Account;
using Infrastructure.Responses;
using Infrastructure.Seed;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController(IAccountService service) : ControllerBase
{
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<Response<string>> Register(RegisterDto request)
    {
        return await service.Register(request);
    }
    
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<Response<string>> Login(LoginDto request)
    {
        return await service.Login(request);
    }
    
    [HttpPost("add-role-to-user")]
    [Authorize(Roles = "Admin")]
    public async Task<Response<string>> AddRoleToUser(RoleDto request)
    {
        return await service.AddRoleToUser(request);
    }
    
    [HttpDelete("remove-role-from-user")]
    [Authorize(Roles = "Admin")]
    public async Task<Response<string>> RemoveRoleFromUser(RoleDto request)
    {
        return await service.RemoveRoleFromUser(request);
    }
}