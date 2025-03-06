using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Dtos.Account;
using Domain.Dtos.Auth;
using Domain.Entities;
using Infrastructure.Interfaces.Account;
using Infrastructure.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;
using System.Net;
using System.IO;
using System.Linq;
using Infrastructure.Seed;

namespace Infrastructure.Services
{
    public class AccountService(
        UserManager<User> userManager,
        RoleManager<IdentityRole<int>> roleManager,
        IConfiguration configuration,
        string uploadPath)
        : IAccountService
    {
        private readonly string[] _allowedImageExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
        private const long MaxImageSize = 10 * 1024 * 1024; // 10MB

        public async Task<Response<string>> Register(RegisterDto model)
        {
            var existingUser = await userManager.FindByNameAsync(model.UserName);
            if (existingUser != null)
                return new Response<string>(HttpStatusCode.BadRequest, "Username already exists");
            string profileImagePath = string.Empty;
            if (model.ProfileImage != null && model.ProfileImage.Length > 0)
            {
                var fileExtension = Path.GetExtension(model.ProfileImage.FileName).ToLowerInvariant();
                if (!_allowedImageExtensions.Contains(fileExtension))
                    return new Response<string>(HttpStatusCode.BadRequest, "Invalid profile image format. Allowed formats: .jpg, .jpeg, .png, .gif");

                if (model.ProfileImage.Length > MaxImageSize)
                    return new Response<string>(HttpStatusCode.BadRequest, "Profile image size must be less than 10MB");

                var profilesFolder = Path.Combine(uploadPath, "uploads", "profiles");
                if (!Directory.Exists(profilesFolder))
                    Directory.CreateDirectory(profilesFolder);

                var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(profilesFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfileImage.CopyToAsync(fileStream);
                }
                profileImagePath = $"/uploads/profiles/{uniqueFileName}";
            }

            var newUser = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email,
                Age = model.Age,
                ProfileImagePath = profileImagePath,
                Address = model.Address,
            };

            var result = await userManager.CreateAsync(newUser, model.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                return new Response<string>(HttpStatusCode.BadRequest, errors);
            }

            return new Response<string>("User registered successfully");
        }

        public async Task<Response<string>> Login(LoginDto login)
        {
            var user = await userManager.FindByNameAsync(login.Username);
            if (user == null)
                return new Response<string>(HttpStatusCode.NotFound, "User not found");

            var isPasswordValid = await userManager.CheckPasswordAsync(user, login.Password);
            if (!isPasswordValid)
                return new Response<string>(HttpStatusCode.BadRequest, "Invalid password");

            var token = await GenerateJwtToken(user);
            return new Response<string>(token) { Message = "Login successful" };
        }

        public async Task<Response<string>> AddRoleToUser(RoleDto userRole)
        {
            var user = await userManager.FindByIdAsync(userRole.UserId);
            if (user == null)
                return new Response<string>(HttpStatusCode.NotFound, "User not found");

            if (!await roleManager.RoleExistsAsync(userRole.RoleId))
                return new Response<string>(HttpStatusCode.BadRequest, "Role does not exist");

            var result = await userManager.AddToRoleAsync(user, userRole.RoleId);
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                return new Response<string>(HttpStatusCode.BadRequest, errors);
            }

            return new Response<string>("Role added successfully");
        }

        public async Task<Response<string>> RemoveRoleFromUser(RoleDto userRole)
        {
            var user = await userManager.FindByIdAsync(userRole.UserId);
            if (user == null)
                return new Response<string>(HttpStatusCode.NotFound, "User not found");

            var result = await userManager.RemoveFromRoleAsync(user, userRole.RoleId);
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                return new Response<string>(HttpStatusCode.BadRequest, errors);
            }

            return new Response<string>("Role removed successfully");
        }

        private async Task<string> GenerateJwtToken(User user)
        {
            var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!);
            var securityKey = new SymmetricSecurityKey(key);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString())
            };
            
            var roles = await userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenString;
        }
    }
}
