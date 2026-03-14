using System.ComponentModel.DataAnnotations;

namespace LMS.Ovncr.ViewModels;

/// <summary>
/// ViewModel cho trang chỉnh sửa thông tin cá nhân.
/// </summary>
public class EditProfileViewModel
{
    /// <summary>Tên đăng nhập - chỉ hiển thị, không cho sửa</summary>
    [Display(Name = "Tên đăng nhập")]
    public string? UserName { get; set; }

    /// <summary>Họ và tên đầy đủ</summary>
    [StringLength(200, ErrorMessage = "Họ tên không được vượt quá 200 ký tự")]
    [Display(Name = "Họ và tên")]
    public string? FullName { get; set; }

    /// <summary>Địa chỉ email</summary>
    [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
    [Display(Name = "Email")]
    public string? Email { get; set; }

    /// <summary>Số điện thoại</summary>
    [Phone(ErrorMessage = "Số điện thoại không đúng định dạng")]
    [Display(Name = "Số điện thoại")]
    public string? PhoneNumber { get; set; }
}
