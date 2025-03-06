using System.Net;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Responses;

namespace Infrastructure.Services;

public class BranchService(IBranchRepository branchRepository, IMapper mapper) : IBranchService
{
    
    public async Task<Response<List<GetBranchDto>>> GetAllBranchesAsync(string language = "En")
    {
        var typeBranch = typeof(Branch);
        var branches = await branchRepository.GetAll();
        if (!branches.Any())
        {
            return new Response<List<GetBranchDto>>(HttpStatusCode.NotFound, "Branches not found");
        }

        var branchDtos = branches.Select(branch => new GetBranchDto
        {
            Id = branch.Id,
            BranchName = typeBranch.GetProperty("BranchName" + language)?.GetValue(branch)?.ToString(),
            Address = typeBranch.GetProperty("Address" + language)?.GetValue(branch)?.ToString(),
            PhoneNumber = branch.PhoneNumber,
            Email = branch.Email
        }).ToList();

        return new Response<List<GetBranchDto>>(branchDtos) { Message = "Branches retrieved successfully" };
    }

    public async Task<Response<GetBranchDto>> GetBranchByIdAsync(int id, string language = "En")
    {
        var typeBranch = typeof(Branch);
        var branch = await branchRepository.GetBranch(id);
        if (branch == null)
        {
            return new Response<GetBranchDto>(HttpStatusCode.NotFound, "Branch not found");
        }

        var branchDto = new GetBranchDto
        {
            Id = branch.Id,
            BranchName = typeBranch.GetProperty("BranchName" + language)?.GetValue(branch)?.ToString(),
            Address = typeBranch.GetProperty("Address" + language)?.GetValue(branch)?.ToString(),
            PhoneNumber = branch.PhoneNumber,
            Email = branch.Email
        };

        return new Response<GetBranchDto>(branchDto) { Message = "Branch retrieved successfully" };
    }

    public async Task<Response<string>> CreateBranchAsync(CreateBranchDto request)
    {
        var newBranch = mapper.Map<Branch>(request);
        var res = await branchRepository.CreateBranch(newBranch);

        return res > 0
            ? new Response<string>(HttpStatusCode.Created, "Branch created successfully")
            : new Response<string>(HttpStatusCode.BadRequest, "Failed to create branch");
    }

    public async Task<Response<string>> UpdateBranchAsync(UpdateBranchDto request)
    {
        var branch = await branchRepository.GetBranch(request.Id);
        if (branch == null)
        {
            return new Response<string>(HttpStatusCode.NotFound, "Branch not found");
        }

        var updatedBranch = mapper.Map(request, branch);
        var res = await branchRepository.UpdateBranch(updatedBranch);

        return res > 0
            ? new Response<string>(HttpStatusCode.OK, "Branch updated successfully")
            : new Response<string>(HttpStatusCode.BadRequest, "Failed to update branch");
    }

    public async Task<Response<string>> DeleteBranchAsync(int id)
    {
        var branch = await branchRepository.GetBranch(id);
        if (branch == null)
        {
            return new Response<string>(HttpStatusCode.NotFound, "No branch found");
        }

        var res = await branchRepository.DeleteBranch(branch);
        return res > 0
            ? new Response<string>(HttpStatusCode.OK, "Branch deleted successfully")
            : new Response<string>(HttpStatusCode.BadRequest, "Failed to delete branch");
    }
}
