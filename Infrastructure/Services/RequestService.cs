using System.Net;
using AutoMapper;
using Domain.Dtos.Request;
using Domain.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Responses;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class RequestService (IRequestRepository requestRepository,
    IMapper mapper,ILogger<RequestService> logger) : IRequestService
{
    public async Task<Response<List<GetRequestDto>>> GetAllRequestsAsync()
    {
        var requests = await requestRepository.GetAll();
        if (!requests.Any())
            return new Response<List<GetRequestDto>>(HttpStatusCode.NotFound,"No requests found");
        var res = mapper.Map<List<GetRequestDto>>(requests);
        return new Response<List<GetRequestDto>>(res);
    }

    public async Task<Response<GetRequestDto>> GetRequestByIdAsync(int id)
    {
        var request = await requestRepository.GetRequest(id);
        if (request == null) 
            return new Response<GetRequestDto>(HttpStatusCode.NotFound,"No request found");
        var res = mapper.Map<GetRequestDto>(request);
        return new Response<GetRequestDto>(res);
    }

    public async Task<Response<string>> CreateRequestAsync(CreateRequestDto request)
    {
        var newRequest = new Request()
        {
            FullName = request.FullName,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Email = request.Email,
            Phone = request.Phone,
            IsDeleted = false,
            Question = request.Question,
            DeletedAt = null
        };
        var res = await requestRepository.CreateRequest(newRequest);
        return res > 0 
            ? new Response<string>(HttpStatusCode.Created, "Request Created")
            : new Response<string>(HttpStatusCode.BadRequest, "Failed to create request");
    }

    public async Task<Response<string>> UpdateRequestAsync(UpdateRequestDto request)
    {
        var oldRequest = await requestRepository.GetRequest(request.Id);
        if (oldRequest == null)
            return new Response<string>(HttpStatusCode.NotFound, "No request found");
        mapper.Map(request, oldRequest);

        var res = await requestRepository.UpdateRequest(oldRequest);
        return res > 0
            ? new Response<string>(HttpStatusCode.OK, "Request Updated")
            : new Response<string>(HttpStatusCode.BadRequest, "Failed to update request");
    }


    public async Task<Response<string>> DeleteRequestAsync(int id)
    {
        var oldRequest = await requestRepository.GetRequest(id);
        if (oldRequest == null) return new Response<string>(HttpStatusCode.NotFound,"No request found");
        var res = await requestRepository.DeleteRequest(oldRequest);
        return res > 0 
            ? new Response<string>(HttpStatusCode.OK, "Request Deleted")
            : new Response<string>(HttpStatusCode.BadRequest, "Failed to delete request");
    }
}