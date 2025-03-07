using System.Net;
using Domain.Dtos;
using Domain.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Responses;

namespace Infrastructure.Services;

public class CourseService (ICourseRepository courseRepository,string uploadPath) : ICourseService
{
    private readonly string[] _allowedExtensions = [".jpg", ".jpeg", ".png", ".gif"];
    private const long MaxFileSize = 10 * 1024 * 1024; 
    public async Task<Response<List<GetCourseDto>>> GetCoursesAsync(string language = "En")
    {
        var courseType = typeof(Course);
        var courses = await courseRepository.GetAll();
        if (!courses.Any())
            return new Response<List<GetCourseDto>>(HttpStatusCode.NotFound,"Course not found");
        var dto = courses.Select(x => new GetCourseDto
        {
            Name = courseType.GetProperty("Name" + language).GetValue(x).ToString(),
            Description = courseType.GetProperty("Description" + language).GetValue(x).ToString(),
            Duration = x.Duration,
            Price = x.Price,
            ImagePath = x.ImagePath
        }).ToList();
        return new Response<List<GetCourseDto>>(dto);
    }

    public async Task<Response<GetCourseDto>> GetCourseByIdAsync(int courseId, string language = "En")
    {
        var courseType = typeof(Course);
        var course = await courseRepository.GetById(courseId);
        if (course == null)
            return new Response<GetCourseDto>(HttpStatusCode.NotFound,"Course not found");
        var dto = new GetCourseDto
        {
            Name = courseType.GetProperty("Name" + language)?.GetValue(course).ToString(),
            Description = courseType.GetProperty("Description" + language)?.GetValue(course).ToString(),
            Duration = course.Duration,
            Price = course.Price,
            ImagePath = course.ImagePath
        };
        return new Response<GetCourseDto>(dto);
    }

    public async Task<Response<string>> CreateCourseAsync(CreateCourseDto courseDto)
    {
        if (courseDto.Image == null || courseDto.Image.Length == 0)
            return new Response<string>(System.Net.HttpStatusCode.BadRequest, "Image file is required");

        if (courseDto.Image.Length > MaxFileSize)
            return new Response<string>(System.Net.HttpStatusCode.BadRequest, "Image file size must be less than 10MB");

        var fileExtension = Path.GetExtension(courseDto.Image.FileName).ToLower();
        if (!_allowedExtensions.Contains(fileExtension))
            return new Response<string>(System.Net.HttpStatusCode.BadRequest,
                "Invalid image format. Allowed formats: .jpg, .jpeg, .png, .gif");


        var uploadsFolder = Path.Combine(uploadPath, "uploads", "courses");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        await using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await courseDto.Image.CopyToAsync(fileStream);
        }
        var course = new Course
        {
            NameTj = courseDto.NameTj,
            DescriptionTj = courseDto.DescriptionTj,
            NameRu = courseDto.NameRu,
            DescriptionRu = courseDto.DescriptionRu,
            NameEn = courseDto.NameEn,
            DescriptionEn = courseDto.DescriptionEn,
            Duration = courseDto.Duration,
            Price = courseDto.Price,
            CreatedAt = DateTime.UtcNow,
            ImagePath = $"/uploads/banners/{uniqueFileName}",
        };
        int res = await courseRepository.Create(course);
        return res > 0
            ? new Response<string>(HttpStatusCode.Created,"Course created successfully")
            : new Response<string>(HttpStatusCode.BadRequest,"Something went wrong");
    }

    public async Task<Response<string>> UpdateCourseAsync(UpdateCourseDto courseDto)
    {
        var banner = await courseRepository.GetById(courseDto.Id);
        if (banner == null)
            return new Response<string>(System.Net.HttpStatusCode.NotFound, "Course not found");

        banner.NameTj = courseDto.NameTj;
        banner.NameRu = courseDto.NameRu ;
        banner.NameEn = courseDto.NameEn ;
        banner.DescriptionTj = courseDto.DescriptionTj;
        banner.DescriptionRu = courseDto.DescriptionRu;
        banner.DescriptionEn = courseDto.DescriptionEn;
        banner.UpdatedAt = DateTime.UtcNow;

        if (courseDto.Image != null)
        {
            if (courseDto.Image.Length > MaxFileSize)
                return new Response<string>(System.Net.HttpStatusCode.BadRequest,
                    "Image file size must be less than 10MB");

            var fileExtension = Path.GetExtension(courseDto.Image.FileName).ToLower();
            if (!_allowedExtensions.Contains(fileExtension))
                return new Response<string>(System.Net.HttpStatusCode.BadRequest,
                    "Invalid image format. Allowed formats: .jpg, .jpeg, .png, .gif");

            var uploadsFolder = Path.Combine(uploadPath, "uploads", "banners");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await courseDto.Image.CopyToAsync(fileStream);
            }

            banner.ImagePath = $"/uploads/course/{uniqueFileName}";
        }
        var result = await courseRepository.Update(banner);
        return result > 0
            ? new Response<string>(HttpStatusCode.NoContent,"Course updated successfully")
            : new Response<string>(HttpStatusCode.BadRequest,"Something went wrong");
    }

    public async Task<Response<string>> DeleteCourseAsync(int courseId)
    {
        var course = await courseRepository.GetById(courseId);
        if (course == null)
            return new Response<string>(System.Net.HttpStatusCode.NotFound, "Course not found");
        var result = await courseRepository.Delete(course);
        return result > 0
            ? new Response<string>(HttpStatusCode.NoContent,"Course deleted successfully")
            : new Response<string>(HttpStatusCode.BadRequest,"Something went wrong");
    }
}