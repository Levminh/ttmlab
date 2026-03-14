using System.ComponentModel.DataAnnotations;

namespace LMS.Ovncr.ViewModels;

/// <summary>
/// ViewModel cho trang đổi mật khẩu.
/// </summary>
public class ChangePasswordViewModel
{
    /// <summary>Mật khẩu hiện tại - dùng để xác nhận danh tính trước khi đổi</summary>
    [Required(ErrorMessage = "Vui lòng nhập mật khẩu hiện tại")]
    [DataType(DataType.Password)]
    [Display(Name = "Mật khẩu hiện tại")]
    public string CurrentPassword { get; set; } = string.Empty;

    /// <summary>Mật khẩu mới - tối thiểu 6 ký tự</summary>
    [Required(ErrorMessage = "Vui lòng nhập mật khẩu mới")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
    [DataType(DataType.Password)]
    [Display(Name = "Mật khẩu mới")]
    public string NewPassword { get; set; } = string.Empty;

    /// <summary>Xác nhận mật khẩu mới - phải trùng với NewPassword</summary>
    [Required(ErrorMessage = "Vui lòng xác nhận mật khẩu mới")]
    [Compare("NewPassword", ErrorMessage = "Mật khẩu xác nhận không khớp")]
    [DataType(DataType.Password)]
    [Display(Name = "Xác nhận mật khẩu mới")]
    public string ConfirmNewPassword { get; set; } = string.Empty;
}
