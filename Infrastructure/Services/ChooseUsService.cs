using System.Net;
using AutoMapper;
using Domain.Dtos;
using Infrastructure.Interfaces;
using Infrastructure.Responses;
using Microsoft.AspNetCore.Http;
using System.IO;
using Domain.Entities;

namespace Infrastructure.Services
{
    public class ChooseUsService(IChooseUsRepository chooseUsRepository, 
        IMapper mapper, string uploadPath)
        : IChooseUsService
    {
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
        private const long MaxFileSize = 10 * 1024 * 1024; // 10MB

        public async Task<Response<List<GetChooseUsDto>>> GetChooseUsAsync(string language = "En")
        {
            var typeChoose = typeof(ChooseUs);
            var chooseUsList = await chooseUsRepository.GetAll();
            if (!chooseUsList.Any())
                return new Response<List<GetChooseUsDto>>(HttpStatusCode.NotFound, "Choose us not found");

            var choosedto = chooseUsList.Select(x => new GetChooseUsDto()
            {
                Id = x.Id,
                Title = typeChoose.GetProperty("Title" + language)?.GetValue(x)?.ToString(),
                Description = typeChoose.GetProperty("Description" + language)?.GetValue(x)?.ToString(),
                IconPath = x.IconPath,
            }).ToList();
            return new Response<List<GetChooseUsDto>>(choosedto);
        }

        public async Task<Response<GetChooseUsDto>> GetChooseUsByIdAsync(int id, string language = "En")
        {
            var typeChoose = typeof(ChooseUs);
            var chooseUs = await chooseUsRepository.GetChooseUs(id);
            if (chooseUs == null)
                return new Response<GetChooseUsDto>(HttpStatusCode.NotFound, "ChooseUs not found");

            var choosedto = new GetChooseUsDto()
            {
                Id = chooseUs.Id,
                Title = typeChoose.GetProperty("Title" + language)?.GetValue(chooseUs)?.ToString(),
                Description = typeChoose.GetProperty("Description" + language)?.GetValue(chooseUs)?.ToString(),
                IconPath = chooseUs.IconPath,
            };
            return new Response<GetChooseUsDto>(choosedto);
        }

        public async Task<Response<string>> CreateChooseUsAsync(CreateChooseUsDto createChooseUsDto)
        {
            if (createChooseUsDto.Icon == null || createChooseUsDto.Icon.Length == 0)
                return new Response<string>(HttpStatusCode.BadRequest, "Icon is required");

            // Checking file extension and size
            var fileExtension = Path.GetExtension(createChooseUsDto.Icon.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(fileExtension))
                return new Response<string>(HttpStatusCode.BadRequest, "Invalid file format");

            if (createChooseUsDto.Icon.Length > MaxFileSize)
                return new Response<string>(HttpStatusCode.BadRequest, "File size exceeds the limit of 10MB");

            // Save icon
            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(uploadPath, "uploads", "chooseUsIcons", uniqueFileName);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await createChooseUsDto.Icon.CopyToAsync(stream);
            }
            var chooseUs = mapper.Map<ChooseUs>(createChooseUsDto);
            chooseUs.IconPath = $"/uploads/chooseUsIcons/{uniqueFileName}";
            await chooseUsRepository.CreateChooseUs(chooseUs);

            return new Response<string>("ChooseUs created successfully");
        }

        public async Task<Response<string>> UpdateChooseUsAsync(UpdateChooseUsDto updateChooseUsDto)
        {
            var chooseUs = await chooseUsRepository.GetChooseUs(updateChooseUsDto.Id);
            if (chooseUs == null)
                return new Response<string>(HttpStatusCode.NotFound, "ChooseUs not found");

            // Handle icon upload if exists
            if (updateChooseUsDto.Icon != null && updateChooseUsDto.Icon.Length > 0)
            {
                var fileExtension = Path.GetExtension(updateChooseUsDto.Icon.FileName).ToLowerInvariant();
                if (!_allowedExtensions.Contains(fileExtension))
                    return new Response<string>(HttpStatusCode.BadRequest, "Invalid file format");

                if (updateChooseUsDto.Icon.Length > MaxFileSize)
                    return new Response<string>(HttpStatusCode.BadRequest, "File size exceeds the limit of 10MB");

                // Delete the old icon if exists
                if (!string.IsNullOrEmpty(chooseUs.IconPath))
                {
                    var oldIconPath = Path.Combine(uploadPath, chooseUs.IconPath.TrimStart('/'));
                    if (File.Exists(oldIconPath))
                        File.Delete(oldIconPath);
                }

                // Save the new icon
                var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(uploadPath, "uploads", "chooseUsIcons", uniqueFileName);
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await updateChooseUsDto.Icon.CopyToAsync(stream);
                }

                chooseUs.IconPath = $"/uploads/chooseUsIcons/{uniqueFileName}";
            }

            // Update other fields
            mapper.Map(updateChooseUsDto, chooseUs);
            await chooseUsRepository.UpdateChooseUs(chooseUs);

            return new Response<string>("ChooseUs updated successfully");
        }

        public async Task<Response<string>> DeleteChooseUsAsync(int id)
        {
            var chooseUs = await chooseUsRepository.GetChooseUs(id);
            if (chooseUs == null)
                return new Response<string>(HttpStatusCode.NotFound, "ChooseUs not found");

            // Delete icon if exists
            if (!string.IsNullOrEmpty(chooseUs.IconPath))
            {
                var filePath = Path.Combine(uploadPath, chooseUs.IconPath.TrimStart('/'));
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }

            int res = await chooseUsRepository.DeleteChooseUs(chooseUs);
            return res > 0
                ? new Response<string>(HttpStatusCode.OK, "ChooseUs deleted successfully")
                : new Response<string>(HttpStatusCode.InternalServerError, "ChooseUs not deleted");
        }
    }
}
