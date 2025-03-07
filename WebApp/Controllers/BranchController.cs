using Domain.Dtos;
using Infrastructure.Interfaces;
using Infrastructure.Responses;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class BranchController (IBranchService service) : Controller
{
    [HttpGet]
    public async Task<Response<List<GetBranchDto>>> GetBranches(string language = "En")
    {
        return await service.GetAllBranchesAsync(language);
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> GetBranchById(int id, [FromQuery] string language = "En")
    {
        var response = await service.GetBranchByIdAsync(id, language);
        return Ok(response);
    }

    [HttpPost]
    public async Task<Response<string>> CreateBranch([FromForm]CreateBranchDto branchDto)
    {
        return await service.CreateBranchAsync(branchDto);
    }

    [HttpDelete]
    public async Task<Response<string>> DeleteBranch(int id)
    {
        return await service.DeleteBranchAsync(id);
    }

    [HttpPut]
    public async Task<Response<string>> UpdateBranch(UpdateBranchDto branchDto)
    {
        return await service.UpdateBranchAsync(branchDto);
    }
    
}