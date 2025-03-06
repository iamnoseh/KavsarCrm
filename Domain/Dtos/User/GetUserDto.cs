using System;

namespace Domain.Dtos.User
{
    public class GetUserDto
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string? LastName { get; set; }

        public int Age { get; set; }

        public string Address { get; set; }

        public DateTime RegistrationDate { get; set; }

        public string? ProfileImagePath { get; set; }
    }
}