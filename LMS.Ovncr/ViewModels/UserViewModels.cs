using System.ComponentModel.DataAnnotations;

namespace LMS.Ovncr.ViewModels;

/// <summary>
/// ViewModel hiển thị thông tin người dùng trong danh sách quản lý tài khoản (Admin).
/// </summary>
public class UserListViewModel
{
    /// <summary>ID nội bộ của user (GUID)</summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>Tên đăng nhập</summary>
    [Display(Name = "Tên đăng nhập")]
    public string UserName { get; set; } = string.Empty;

    /// <summary>Họ và tên đầy đủ</summary>
    [Display(Name = "Họ và tên")]
    public string? FullName { get; set; }

    /// <summary>Email</summary>
    [Display(Name = "Email")]
    public string? Email { get; set; }

    /// <summary>Danh sách vai trò của user (Admin, GiangVien, HocVien...)</summary>
    [Display(Name = "Vai trò")]
    public IList<string> Roles { get; set; } = new List<string>();

    /// <summary>Trạng thái khóa tài khoản (true = đang bị khóa)</summary>
    [Display(Name = "Đang bị khóa")]
    public bool IsLockedOut { get; set; }

    /// <summary>Ngày hết hạn khóa (null nếu không bị khóa)</summary>
    [Display(Name = "Khóa đến")]
    public DateTimeOffset? LockoutEnd { get; set; }
}

/// <summary>
/// ViewModel cho form tạo user mới (Admin tạo, không cần xác nhận email).
/// </summary>
public class CreateUserViewModel
{
    [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
    [StringLength(50, MinimumLength = 3)]
    [Display(Name = "Tên đăng nhập")]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập họ và tên")]
    [Display(Name = "Họ và tên")]
    public string FullName { get; set; } = string.Empty;

    [EmailAddress]
    [Display(Name = "Email")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
    [StringLength(100, MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Mật khẩu")]
    public string Password { get; set; } = string.Empty;

    /// <summary>Role được gán cho user mới (chọn từ danh sách)</summary>
    [Required(ErrorMessage = "Vui lòng chọn vai trò")]
    [Display(Name = "Vai trò")]
    public string Role { get; set; } = "Student";

    /// <summary>Danh sách roles có trong hệ thống để bind vào dropdown</summary>
    public List<string> AvailableRoles { get; set; } = new();
}

/// <summary>
/// ViewModel cho form chỉnh sửa user (Admin dùng).
/// </summary>
public class EditUserViewModel
{
    public string Id { get; set; } = string.Empty;

    [Display(Name = "Tên đăng nhập")]
    public string? UserName { get; set; }

    [Display(Name = "Họ và tên")]
    public string? FullName { get; set; }

    [EmailAddress]
    [Display(Name = "Email")]
    public string? Email { get; set; }

    [Phone]
    [Display(Name = "Số điện thoại")]
    public string? PhoneNumber { get; set; }

    [Display(Name = "Vai trò")]
    public string Role { get; set; } = "Student";

    /// <summary>Danh sách roles có trong hệ thống để bind vào dropdown</summary>
    public List<string> AvailableRoles { get; set; } = new();

    /// <summary>Mật khẩu mới (để trống nếu không muốn đổi)</summary>
    [DataType(DataType.Password)]
    [Display(Name = "Mật khẩu mới (để trống nếu không đổi)")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
    public string? NewPassword { get; set; }
}
