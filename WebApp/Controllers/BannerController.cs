using System.Threading.Tasks;
using Domain.Dtos.BannerDto;
using Infrastructure.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BannerController(IBannerService bannerService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllBanners([FromQuery] string language = "En")
    {
        var response = await bannerService.GetAllBanners(language);
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBannerById(int id, [FromQuery] string language = "En")
    {
        var response = await bannerService.GetBannerById(id, language);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBanner([FromForm] CreateBannerDto dto)
    {
        var response = await bannerService.CreateBanner(dto);
        return Ok(response);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateBanner([FromForm] UpdateBannerDto dto)
    {
        var response = await bannerService.UpdateBanner(dto);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBanner(int id)
    {
        var response = await bannerService.DeleteBanner(id);
        return Ok(response);
    }
}