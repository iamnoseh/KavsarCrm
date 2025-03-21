using Domain.Dtos.Feedback;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Interfaces;
using Infrastructure.Seed;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;
[ApiController]
[Route("api/[controller]")]
public class FeedbackController (IFeedbackService service) : ControllerBase
{
    [HttpGet]
    public async Task<PaginationResponse<List<FeedbackGetDto>>> GetFeedbacks([FromQuery] BaseFilter filter,
        [FromQuery] string language = "Ru")
    {
        return await service.GetFeedbacksAsync(filter, language);
    }

    [HttpPost]
    public async Task<Response<string>> CreateFeedback([FromBody] FeedbackCreateDto feedbackCreateDto)
    {
        return await service.CreateFeedbackAsync(feedbackCreateDto);
    }

    [HttpPut("id")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<Response<string>> UpdateFeedback(FeedbackUpdateDto feedbackUpdateDto)
    {
        return await service.UpdateFeedbackAsync(feedbackUpdateDto);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<Response<string>> DeleteFeedback(int id)
    {
        return await service.DeleteFeedbackAsync(id);
    }

    [HttpGet("id")]
    public async Task<Response<FeedbackGetDto>> GetFeedbackById(int id, string language = "Ru")
    {
        return await service.GetFeedbackByIdAsync(id, language);
    }

}