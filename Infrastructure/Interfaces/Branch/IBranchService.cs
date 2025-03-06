using Domain.Dtos;
using Infrastructure.Responses;

namespace Infrastructure.Interfaces;

public interface IBranchService
{
    Task<Response<List<GetBranchDto>>> GetAllBranchesAsync(string language = "En");
    Task<Response<GetBranchDto>> GetBranchByIdAsync(int id,string language = "En");
    Task<Response<string>> CreateBranchAsync(CreateBranchDto request);
    Task<Response<string>> UpdateBranchAsync(UpdateBranchDto request);
    Task<Response<string>> DeleteBranchAsync(int id);
}