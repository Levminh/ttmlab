using System.ComponentModel.DataAnnotations;

namespace LMS.Ovncr.ViewModels;

/// <summary>
/// ViewModel cho trang đăng nhập.
/// Chứa thông tin người dùng nhập vào form Login.
/// </summary>
public class LoginViewModel
{
    /// <summary>Tên đăng nhập (UserName)</summary>
    [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
    [Display(Name = "Tên đăng nhập")]
    public string UserName { get; set; } = string.Empty;

    /// <summary>Mật khẩu (sẽ được hash bởi Identity, không lưu raw)</summary>
    [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
    [DataType(DataType.Password)]
    [Display(Name = "Mật khẩu")]
    public string Password { get; set; } = string.Empty;

    /// <summary>Ghi nhớ đăng nhập (lưu cookie lâu dài)</summary>
    [Display(Name = "Ghi nhớ đăng nhập")]
    public bool RememberMe { get; set; }

    /// <summary>
    /// URL trang gốc trước khi bị chuyển hướng sang Login.
    /// Sau khi đăng nhập thành công sẽ redirect về đây.
    /// </summary>
    public string? ReturnUrl { get; set; }
}
