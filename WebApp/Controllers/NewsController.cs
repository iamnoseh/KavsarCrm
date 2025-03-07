using Domain.Dtos;
using Infrastructure.Interfaces;
using Infrastructure.Responses;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NewsController(INewsService service) : ControllerBase
{
    [HttpGet]
    public async Task<Response<List<GetNewsDto>>> GetNews(string language = "En")
    {
        return await service.GetNewsAsync(language);
    }

    [HttpPost]
    public async Task<Response<string>> CreateNews([FromForm]CreateNewsDto newsDto)
    {
        return await service.CreateNewsAsync(newsDto);
    }
    
    [HttpPut]
    public async Task<Response<string>> UpdateNews([FromForm]UpdateNewsDto newsDto)
    {
        return await service.UpdateNewsAsync(newsDto);
    }
    [HttpDelete]
    public async Task<Response<string>> DeleteNews(int id)
    {
        return await service.DeleteNewsAsync(id);
    }

    [HttpGet("id")]
    public async Task<Response<GetNewsDto>> GetNews(int id,string language = "En")
    {
        return await service.GetNewsByIdAsync(id, language);
    }
    
}