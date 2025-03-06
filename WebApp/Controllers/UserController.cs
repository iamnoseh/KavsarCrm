using Domain.Dtos.User;
using Domain.Filters;
using Infrastructure.Interfaces;
using Infrastructure.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IUserService userService) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<Response<GetUserDto>>> GetUser(int id)
        {
            var response = await userService.GetByIdAsync(id);
            if(response.StatusCode != (int)HttpStatusCode.OK)
                return StatusCode(response.StatusCode, response);
            return Ok(response);
        }
        
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<GetUserDto>>>> GetUsers([FromQuery] BaseFilter filter)
        {
            var response = await userService.GetUsersAsync(filter);
            return Ok(response);
        }
        
        [HttpPost]
        public async Task<ActionResult<Response<string>>> CreateUser([FromForm] CreateUserDto createUserDto)
        {
            var response = await userService.CreateUserAsync(createUserDto);
            if(response.StatusCode != (int)HttpStatusCode.OK)
                return StatusCode(response.StatusCode, response);
            return Ok(response);
        }
        
        [HttpPut]
        public async Task<ActionResult<Response<string>>> UpdateUser([FromForm] UpdateUserDto updateUserDto)
        {
            var response = await userService.UpdateUserAsync(updateUserDto);
            if(response.StatusCode != (int)HttpStatusCode.OK)
                return StatusCode(response.StatusCode, response);
            return Ok(response);
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult<Response<string>>> DeleteUser(int id)
        {
            var response = await userService.DeleteUserAsync(id);
            if(response.StatusCode != (int)HttpStatusCode.OK)
                return StatusCode(response.StatusCode, response);
            return Ok(response);
        }
    }
}
