using Repositories.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dto
{
    public class DoctorDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;

        public string LicenseNumber { get; set; }

        public string Qualification { get; set; }

        public DateOnly? DateOfLicense { get; set; }

        public DateOnly? LicenseExpiryDate { get; set; }

        public bool? IsActive { get; set; }
    }
    public class CreateDoctorDTO
    {
        public string Name { get; set; } = null!;
        [Required]
        [RegularExpression(@"^[\w\.-]+@gmail\.com$", ErrorMessage = "Email phải có định dạng hợp lệ và kết thúc bằng @gmail.com")]
        public string Email { get; set; } = null!;
        public string LicenseNumber { get; set; } = null!;
        public string Qualification { get; set; } = null!;
        public DateOnly? DateOfLicense { get; set; }
        public DateOnly? LicenseExpiryDate { get; set; }
    }

    public class UpdateDoctorDTO
    {
        public int Id { get; set; }                     
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string LicenseNumber { get; set; } = null!;
        public string Qualification { get; set; } = null!;
        public DateOnly? DateOfLicense { get; set; }
        public DateOnly? LicenseExpiryDate { get; set; }
    }

    //public class CreateUserDTO
    //{
    //    public string Username { get; set; } = null!;
    //    public string Email { get; set; } = null!;
    //    public string Password { get; set; } = null!;
    //    public string Role { get; set; }
    //}

}
