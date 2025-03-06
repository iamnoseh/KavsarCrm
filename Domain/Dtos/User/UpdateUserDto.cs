using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos.User
{
    public class UpdateUserDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string? LastName { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public IFormFile? ProfileImage { get; set; }
    }
}