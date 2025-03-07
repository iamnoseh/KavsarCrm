using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Dtos.BannerDto;
using Domain.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Interfaces.IServices;
using Infrastructure.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class BannerService(
    IBannerRepository bannerRepository,
    IMapper mapper,
    string uploadPath) : IBannerService
{
    private readonly IMapper _mapper = mapper;
    private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
    private const long MaxFileSize = 10 * 1024 * 1024; // 10MB 

    public async Task<Response<List<GetBannerDto>>> GetAllBanners(string language = "En")
    {
        var bannerType = typeof(Banner);
        var banners = await bannerRepository.GetAll();
        var dto = banners.Select(banner => new GetBannerDto
        {
            Id = banner.Id,
            Title = bannerType.GetProperty("Title" + language)?.GetValue(banner)?.ToString(),
            Description = bannerType.GetProperty("Description" + language)?.GetValue(banner)?.ToString(),
            ImagePath = banner.ImagePath
        }).ToList();

        return new Response<List<GetBannerDto>>(dto) { Message = "Banners retrieved successfully" };
    }

    public async Task<Response<GetBannerDto>> GetBannerById(int id, string language = "En")
    {
        var banner = await bannerRepository.GetBanner(id);
        if (banner == null)
            return new Response<GetBannerDto>(System.Net.HttpStatusCode.NotFound, "Banner not found");

        var bannerType = typeof(Banner);
        var dto = new GetBannerDto
        {
            Id = banner.Id,
            Title = bannerType.GetProperty("Title" + language)?.GetValue(banner)?.ToString(),
            Description = bannerType.GetProperty("Description" + language)?.GetValue(banner)?.ToString(),
            ImagePath = banner.ImagePath
        };
        return new Response<GetBannerDto>(dto) { Message = "Banner retrieved successfully" };
    }
    public async Task<Response<string>> CreateBanner(CreateBannerDto dto)
    {
        if (dto.ImageFile == null || dto.ImageFile.Length == 0)
            return new Response<string>(System.Net.HttpStatusCode.BadRequest, "Image file is required");

        if (dto.ImageFile.Length > MaxFileSize)
            return new Response<string>(System.Net.HttpStatusCode.BadRequest, "Image file size must be less than 10MB");

        var fileExtension = Path.GetExtension(dto.ImageFile.FileName).ToLower();
        if (!_allowedExtensions.Contains(fileExtension))
            return new Response<string>(System.Net.HttpStatusCode.BadRequest,
                "Invalid image format. Allowed formats: .jpg, .jpeg, .png, .gif");


        var uploadsFolder = Path.Combine(uploadPath, "uploads", "banners");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        await using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await dto.ImageFile.CopyToAsync(fileStream);
        }

        var banner = new Banner
        {
            TitleTj = dto.TitleTj,
            TitleRu = dto.TitleRu,
            TitleEn = dto.TitleEn,
            DescriptionTj = dto.DescriptionTj,
            DescriptionRu = dto.DescriptionRu,
            DescriptionEn = dto.DescriptionEn,
            ImagePath = $"/uploads/banners/{uniqueFileName}",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var result = await bannerRepository.CreateBanner(banner);
        return result > 0
            ? new Response<string>("Banner created successfully")
            : new Response<string>(System.Net.HttpStatusCode.InternalServerError, "Error creating banner");
    }

    public async Task<Response<string>> UpdateBanner(UpdateBannerDto dto)
    {
        var banner = await bannerRepository.GetBanner(dto.Id);
        if (banner == null)
            return new Response<string>(System.Net.HttpStatusCode.NotFound, "Banner not found");

        banner.TitleTj = dto.TitleTj ?? banner.TitleTj;
        banner.TitleRu = dto.TitleRu ?? banner.TitleRu;
        banner.TitleEn = dto.TitleEn ?? banner.TitleEn;
        banner.DescriptionTj = dto.DescriptionTj ?? banner.DescriptionTj;
        banner.DescriptionRu = dto.DescriptionRu ?? banner.DescriptionRu;
        banner.DescriptionEn = dto.DescriptionEn ?? banner.DescriptionEn;
        banner.UpdatedAt = DateTime.UtcNow;

        if (dto.ImageFile != null)
        {
            if (dto.ImageFile.Length > MaxFileSize)
                return new Response<string>(System.Net.HttpStatusCode.BadRequest,
                    "Image file size must be less than 10MB");

            var fileExtension = Path.GetExtension(dto.ImageFile.FileName).ToLower();
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
                await dto.ImageFile.CopyToAsync(fileStream);
            }

            banner.ImagePath = $"/uploads/banners/{uniqueFileName}";
        }

        var result = await bannerRepository.UpdateBanner(banner);
        return result > 0
            ? new Response<string>("Banner updated successfully")
            : new Response<string>(System.Net.HttpStatusCode.InternalServerError, "Error updating banner");
    }

    public async Task<Response<string>> DeleteBanner(int id)
    {
        var result = await bannerRepository.DeleteBanner(id);
        return result > 0
            ? new Response<string>("Banner deleted successfully")
            : new Response<string>(System.Net.HttpStatusCode.InternalServerError, "Error deleting banner");
    }
    
}

