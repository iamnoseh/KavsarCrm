using Domain.Dtos;
using Infrastructure.Responses;

namespace Infrastructure.Interfaces;

public interface IChooseUsService
{
    Task<Response<List<GetChooseUsDto>>> GetChooseUsAsync(string language = "En");
    Task<Response<GetChooseUsDto>> GetChooseUsByIdAsync(int id,string language = "En");
    Task<Response<string>> CreateChooseUsAsync(CreateChooseUsDto createChooseUsDto);
    Task<Response<string>> UpdateChooseUsAsync(UpdateChooseUsDto updateChooseUsDto);
    Task<Response<string>> DeleteChooseUsAsync(int id);
}