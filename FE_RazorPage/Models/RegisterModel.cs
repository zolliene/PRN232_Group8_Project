using System.ComponentModel.DataAnnotations;

namespace FE_RazorPage.Models;


public class RegisterPatientModel
{
    [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
    [Display(Name = "Tên đăng nhập")]
    public string Username { get; set; } = "";

    [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
    [Display(Name = "Mật khẩu")]
    public string Password { get; set; } = "";

    [Required(ErrorMessage = "Vui lòng nhập email")]
    [Display(Name = "Email")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Vui lòng nhập họ")]
    [Display(Name = "Họ")]
    public string FirstName { get; set; } = "";

    [Required(ErrorMessage = "Vui lòng nhập tên")]
    [Display(Name = "Tên")]
    public string LastName { get; set; } = "";

    [Required(ErrorMessage = "Vui lòng chọn ngày sinh")]
    [Display(Name = "Ngày sinh")]
    public DateTime Dob { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập giới tính")]
    [Display(Name = "Giới tính")]
    public string Gender { get; set; } = "";

    [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
    [Display(Name = "Số điện thoại")]
    public string Phone { get; set; } = "";

    [Display(Name = "Địa chỉ (không bắt buộc)")]
    public string? Address { get; set; }

    [Display(Name = "Mã BHYT (không bắt buộc)")]
    public string? InsuranceNumber { get; set; }
}

