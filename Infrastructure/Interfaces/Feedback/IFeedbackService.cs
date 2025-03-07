using Domain.Dtos.Feedback;
using Domain.Filters;
using Infrastructure.Responses;

namespace Infrastructure.Interfaces;

public interface IFeedbackService
{
    Task<PaginationResponse<List<FeedbackGetDto>>> GetFeedbacksAsync(BaseFilter filter,string language = "En");
    Task<Response<FeedbackGetDto>> GetFeedbackByIdAsync(int id,string language = "En");
    Task<Response<string>> CreateFeedbackAsync(FeedbackCreateDto feedbackCreateDto);
    Task<Response<string>> UpdateFeedbackAsync(FeedbackUpdateDto feedbackUpdateDto);
    Task<Response<string>> DeleteFeedbackAsync(int id);
}