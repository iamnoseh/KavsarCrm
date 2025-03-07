using Domain.Dtos;
using Infrastructure.Responses;

namespace Infrastructure.Interfaces;

public interface ICourseService
{
    Task<Response<List<GetCourseDto>>> GetCoursesAsync(string language = "En");
    Task<Response<GetCourseDto>> GetCourseByIdAsync(int courseId, string language = "En");
    Task<Response<string>> CreateCourseAsync(CreateCourseDto courseDto);
    Task<Response<string>> UpdateCourseAsync(UpdateCourseDto courseDto);
    Task<Response<string>> DeleteCourseAsync(int courseId);
}