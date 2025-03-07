using Domain.Dtos;
using Infrastructure.Responses;

namespace Infrastructure.Interfaces;

public interface INewsService
{
    Task<Response<List<GetNewsDto>>> GetNewsAsync(string language = "En");
    Task<Response<GetNewsDto>> GetNewsByIdAsync(int id,string language = "En");
    Task<Response<string>> CreateNewsAsync(CreateNewsDto request);
    Task<Response<string>> UpdateNewsAsync(UpdateNewsDto request);
    Task<Response<string>> DeleteNewsAsync(int id);
}