using System.Net;
using Domain.Dtos;
using Domain.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Responses;

namespace Infrastructure.Services;

public class NewsService (INewsRepository repository,string uploadPath) : INewsService
{
    // private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
    private const long MaxFileSize = 100 * 1024 * 1024; // 100MB 
     
    public async Task<Response<List<GetNewsDto>>> GetNewsAsync(string language = "En")
    {
        var newsType = typeof(News);
        var news = await repository.GetAllNews();
        if (!news.Any())
            return new Response<List<GetNewsDto>>(HttpStatusCode.NotFound,"News not found");
        var dtos = news.Select(x => new GetNewsDto
        {
            Id = x.Id,
            Title = newsType.GetProperty("Title" + language).GetValue(x).ToString(),
            Content = newsType.GetProperty("Content" + language).GetValue(x).ToString(),
            CreatedAt = x.CreatedAt,
            LikeCount = x.LikeCount,
            UserId = x.UserId,
            MediaUrl = x.MediaUrl,
        }).ToList();
        return new Response<List<GetNewsDto>>(dtos);
    }

    
    public async Task<Response<GetNewsDto>> GetNewsByIdAsync(int id, string language = "En")
    {
        var newsType = typeof(News);
        var news = await repository.GetNewsById(id);
        if (news == null) 
            return new Response<GetNewsDto>(HttpStatusCode.NotFound,"News not found");
        var dto = new GetNewsDto
        {
            Id = news.Id,
            Title = newsType.GetProperty("Title" + language).GetValue(news).ToString(),
            Content = newsType.GetProperty("Content" + language).GetValue(news).ToString(),
            CreatedAt = news.CreatedAt,
            LikeCount = news.LikeCount,
            UserId = news.UserId,
            MediaUrl = news.MediaUrl,
        };
        return new Response<GetNewsDto>(dto);
    }

    
    public async Task<Response<string>> CreateNewsAsync(CreateNewsDto request)
    {
        if (request.Media == null || request.Media.Length == 0)
            return new Response<string>(System.Net.HttpStatusCode.BadRequest, "Image file is required");

        if (request.Media.Length > MaxFileSize)
            return new Response<string>(System.Net.HttpStatusCode.BadRequest, "Image file size must be less than 100MB");

        var fileExtension = Path.GetExtension(request.Media.FileName).ToLower();
        // if (!_allowedExtensions.Contains(fileExtension))
        //     return new Response<string>(System.Net.HttpStatusCode.BadRequest,
        //         "Invalid image format. Allowed formats: .jpg, .jpeg, .png, .gif");
        
        var uploadsFolder = Path.Combine(uploadPath, "uploads", "news");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        await using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await request.Media.CopyToAsync(fileStream);
        }

        var news = new News()
        {
            UserId = request.UserId,
            TitleTj = request.TitleTj,
            TitleRu = request.TitleRu,
            TitleEn = request.TitleEn,
            ContentTj = request.ContentTj,
            ContentRu = request.ContentRu,
            ContentEn = request.ContentEn,
            CreatedAt = DateTime.UtcNow,
            MediaUrl = $"/uploads/news/{uniqueFileName}"
        };
        int res = await repository.CreateNews(news);
        return res > 0
            ? new Response<string>(HttpStatusCode.Created, "News created")
            : new Response<string>(HttpStatusCode.BadRequest,"Something went wrong");
    }

    
    public async Task<Response<string>> UpdateNewsAsync(UpdateNewsDto request)
    {
        var oldNews = await repository.GetNewsById(request.Id);
        if (oldNews == null) 
            return new Response<string>(HttpStatusCode.NotFound,"News not found");
        oldNews.TitleTj = request.TitleTj;
        oldNews.TitleRu = request.TitleRu;
        oldNews.TitleEn = request.TitleEn;
        oldNews.ContentTj = request.ContentTj;
        oldNews.ContentRu = request.ContentRu;
        oldNews.ContentEn = request.ContentEn;
        oldNews.UpdatedAt = DateTime.UtcNow;

        if (request.Media != null)
        {
            if (request.Media.Length > MaxFileSize)
                return new Response<string>(System.Net.HttpStatusCode.BadRequest,
                    "Image file size must be less than 100MB");

            var fileExtension = Path.GetExtension(request.Media.FileName).ToLower();

            var uploadsFolder = Path.Combine(uploadPath, "uploads", "news");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await request.Media.CopyToAsync(fileStream);
            }

            oldNews.MediaUrl = $"/uploads/news/{uniqueFileName}";
        }
        var res = await repository.UpdateNews(oldNews);
        return res > 0 
            ? new Response<string>(HttpStatusCode.NoContent, "News updated")
            : new Response<string>(HttpStatusCode.BadRequest,"Something went wrong");
    }

    
    public async Task<Response<string>> DeleteNewsAsync(int id)
    {
        var deletedNews = await repository.GetNewsById(id);
        if (deletedNews == null)
            return new Response<string>(HttpStatusCode.NotFound,"News not found");
        int res = await repository.DeleteNews(deletedNews);
        return res > 0 
            ? new Response<string>(HttpStatusCode.NoContent,"News deleted")
            : new Response<string>(HttpStatusCode.BadRequest, "Something went wrong");
    }
}