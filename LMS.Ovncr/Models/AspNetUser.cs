using Microsoft.AspNetCore.Identity;

namespace LMS.Ovncr.Models;

/// <summary>
/// Người dùng của hệ thống. Kế thừa IdentityUser để tận dụng toàn bộ
/// cơ chế xác thực của ASP.NET Core Identity (hash mật khẩu, lockout, claims...).
/// </summary>
public partial class AspNetUser : IdentityUser<string>
{
    // IdentityUser<string> đã cung cấp sẵn: Id, UserName, Email, PasswordHash,
    // PhoneNumber, SecurityStamp, LockoutEnabled, LockoutEnd, AccessFailedCount...
    // Ta chỉ cần khai báo thêm các trường đặc thù của hệ thống.
    // ta cần chủ động tự sinh Id cho record mới để EF Core track
    public AspNetUser()
    {
        Id = Guid.NewGuid().ToString();
    }

    /// <summary>Họ và tên đầy đủ của người dùng</summary>
    public string? FullName { get; set; }

    // ========== Navigation Properties ==========
    // Các mối quan hệ tới các bảng nghiệp vụ trong hệ thống

    /// <summary>Danh sách lớp học mà học viên tham gia</summary>
    public virtual ICollection<TbHocVienLopHoc> TbHocVienLopHocs { get; set; } = new List<TbHocVienLopHoc>();

    /// <summary>Danh sách kết quả diễn tập của học viên</summary>
    public virtual ICollection<TbKetQua> TbKetQuas { get; set; } = new List<TbKetQua>();

    /// <summary>Danh sách kỳ thi do người dùng tạo</summary>
    public virtual ICollection<TbKyThi> TbKyThis { get; set; } = new List<TbKyThi>();

    /// <summary>Danh sách lịch phòng lab liên quan đến người dùng</summary>
    public virtual ICollection<TbLichPhongLab> TbLichPhongLabs { get; set; } = new List<TbLichPhongLab>();

    /// <summary>Danh sách lớp học mà người dùng tạo (với vai trò giảng viên)</summary>
    public virtual ICollection<TbLopHoc> TbLopHocs { get; set; } = new List<TbLopHoc>();
}
