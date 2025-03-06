using Domain.Dtos.Request;
using Infrastructure.Responses;
namespace Infrastructure.Interfaces;

public interface IRequestService
{
    Task<Response<List<GetRequestDto>>> GetAllRequestsAsync();
    Task<Response<GetRequestDto>> GetRequestByIdAsync(int id);
    Task<Response<string>> CreateRequestAsync(CreateRequestDto request);
    Task<Response<string>> UpdateRequestAsync(UpdateRequestDto request);
    Task<Response<string>> DeleteRequestAsync(int id);
}