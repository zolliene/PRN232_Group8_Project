using System.ComponentModel.DataAnnotations;

namespace FE_RazorPage.Models
{
    public class GetDoctorModel
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

    public class CreateDoctorModel
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
    public class UpdateDoctorModel
    {
        public int Id { get; set; } 
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string LicenseNumber { get; set; } = null!;
        public string Qualification { get; set; } = null!;
        public DateOnly? DateOfLicense { get; set; }
        public DateOnly? LicenseExpiryDate { get; set; }
    }
}
