using System.Net;
using System.Security.Claims;
using Domain.Dtos.Feedback;
using Domain.Entities;
using Domain.Filters;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Infrastructure.Responses;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;

public class FeedbackService(IFeedbackRepository feedbackRepository,  
        IHttpContextAccessor httpContextAccessor, DataContext context) : IFeedbackService
{
    public async Task<PaginationResponse<List<FeedbackGetDto>>> GetFeedbacksAsync(BaseFilter filter, string language = "En")
    {
        var feedbackType = typeof(Feedback);
        var feedbacks = await feedbackRepository.GetAll(filter);
        var totalRecords = feedbacks.Count;
        var feedbacksDto = new List<FeedbackGetDto>();

        foreach (var feedback in feedbacks)
        {
            string fullName;
            string? profileImagePath = null;
            if (feedback.UserId.HasValue)
            {
                var user = await context.Users.FindAsync(feedback.UserId.Value);
                fullName = user != null ? $"{user.FirstName} {user.LastName}" : " ";
                profileImagePath = user?.ProfileImagePath;
            }
            else
            {
                fullName = feedback.FullName;
            }

            var dto = new FeedbackGetDto
            {
                Id = feedback.Id,
                Text = feedbackType.GetProperty("Text" + language)?.GetValue(feedback).ToString(),//zaboni muvofiqro memonad
                Grade = feedback.Grade,
                FullName = fullName,
                ProfileImagePath = profileImagePath
            };
            feedbacksDto.Add(dto);
        }

        return feedbacksDto.Any()
            ? new PaginationResponse<List<FeedbackGetDto>>(feedbacksDto, totalRecords, filter.PageNumber, filter.PageSize)
            : new PaginationResponse<List<FeedbackGetDto>>(HttpStatusCode.NotFound, "Feedback not found");
    }

    public async Task<Response<FeedbackGetDto>> GetFeedbackByIdAsync(int id, string language = "En")
    {
        var feedbackType = typeof(Feedback);
        var feedback = await feedbackRepository.GetById(id);
        if (feedback == null)
            return new Response<FeedbackGetDto>(HttpStatusCode.NotFound, "Feedback not found");

        string fullName;
        string? profileImagePath = null;
        if (feedback.UserId.HasValue)
        {
            var user = await context.Users.FindAsync(feedback.UserId.Value);
            fullName = user != null ? $"{user.FirstName} {user.LastName}" : null;
            profileImagePath = user?.ProfileImagePath;
        }
        else
        {
            fullName = feedback.FullName;
        }
        var dto = new FeedbackGetDto
        {
            Id = feedback.Id,
            Text = feedbackType.GetProperty("Text" + language)?.GetValue(feedback).ToString(),
            Grade = feedback.Grade,
            FullName = fullName,
            ProfileImagePath = profileImagePath
        };
        return new Response<FeedbackGetDto>(dto);
    }

    public async Task<Response<string>> CreateFeedbackAsync(FeedbackCreateDto feedbackCreateDto)
    {
        var userIdClaim = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
        string fullName;
        string? profileImagePath = null;
        int? userId = null;

        if (userIdClaim != null)
        {
            userId = int.Parse(userIdClaim.Value);
            var user = await context.Users.FindAsync(userId);
            if (user == null)
                return new Response<string>(HttpStatusCode.Unauthorized, "User not found");

            fullName = $"{user.FirstName} {user.LastName}";
            profileImagePath = user.ProfileImagePath;
        }
        else
        {
            if (string.IsNullOrEmpty(feedbackCreateDto.FullName))
                return new Response<string>(HttpStatusCode.BadRequest, "FullName is required for non-registered users");
            fullName = feedbackCreateDto.FullName;
        }

        var feedback = new Feedback
        {
            TextTj = feedbackCreateDto.TextTj,
            TextRu = feedbackCreateDto.TextRu,
            TextEn = feedbackCreateDto.TextEn,
            FullName = fullName,
            ProfileImagePath = profileImagePath,
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        var result = await feedbackRepository.Create(feedback);
        return result > 0
            ? new Response<string>("Feedback created successfully")
            : new Response<string>(HttpStatusCode.InternalServerError, "Error creating feedback");
    }

    public async Task<Response<string>> UpdateFeedbackAsync(FeedbackUpdateDto feedbackUpdateDto)
    {
        var feedback = await feedbackRepository.GetById(feedbackUpdateDto.Id);
        if (feedback == null)
            return new Response<string>(HttpStatusCode.NotFound, "Feedback not found");

        feedback.TextTj = feedbackUpdateDto.TextTj;
        feedback.TextRu = feedbackUpdateDto.TextRu;
        feedback.TextEn = feedbackUpdateDto.TextEn;
        feedback.Grade = feedbackUpdateDto.Grade;
        feedback.UpdatedAt = DateTime.UtcNow;

        var result = await feedbackRepository.Update(feedback);
        return result > 0
            ? new Response<string>("Feedback updated successfully")
            : new Response<string>(HttpStatusCode.InternalServerError, "Error updating feedback");
    }

    public async Task<Response<string>> DeleteFeedbackAsync(int id)
    {
        var feedback = await feedbackRepository.GetById(id);
        if (feedback == null)
            return new Response<string>(HttpStatusCode.NotFound, "Feedback not found");

        var result = await feedbackRepository.Delete(feedback);
        return result > 0
            ? new Response<string>("Feedback deleted successfully")
            : new Response<string>(HttpStatusCode.InternalServerError, "Error deleting feedback");
    }
}
