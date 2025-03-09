using Domain.Dtos.Request;
using Domain.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Responses;
using Infrastructure.Seed;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class RequestController (IRequestService service) : Controller
{
    [HttpGet]
    [Authorize(Roles = Roles.Admin)]
    public async Task<Response<List<GetRequestDto>>> GetRequests()
    {
        return await service.GetAllRequestsAsync();
    }

    [HttpGet("{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<Response<GetRequestDto>> GetRequest(int id)
    {
        return await service.GetRequestByIdAsync(id);
    }

    [HttpPost]
    public async Task<Response<string>> CreateRequest([FromForm]CreateRequestDto createRequestDto)
    {
        return await service.CreateRequestAsync(createRequestDto);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<Response<string>> UpdateRequest( UpdateRequestDto updateRequestDto)
    {
        return await service.UpdateRequestAsync(updateRequestDto);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<Response<string>> DeleteRequest(int id)
    {
        return await service.DeleteRequestAsync(id);
    }
}