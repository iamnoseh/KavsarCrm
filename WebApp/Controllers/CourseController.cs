using Domain.Dtos;
using Infrastructure.Interfaces;
using Infrastructure.Responses;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CourseController(ICourseService service) : Controller
{
    [HttpGet]
    public async Task<Response<List<GetCourseDto>>> GetCourses(string language = "En")
    {
        return await service.GetCoursesAsync(language);
    }

    [HttpGet("{id}")]
    public async Task<Response<GetCourseDto>> GetCourseById(int id,string language = "En")
    {
        return await service.GetCourseByIdAsync(id, language);
    }

    [HttpPost]
    public async Task<Response<string>> CreateCourse(CreateCourseDto courseDto)
    {
        return await service.CreateCourseAsync(courseDto);
    }
    [HttpPut("{id}")]
    public async Task<Response<string>> UpdateCourse([FromForm]UpdateCourseDto courseDto)
    {
        return await service.UpdateCourseAsync(courseDto);
    }

    [HttpDelete("{id}")]
    public async Task<Response<string>> DeleteCourse(int id)
    {
        return await service.DeleteCourseAsync(id);
    }
    
    
}