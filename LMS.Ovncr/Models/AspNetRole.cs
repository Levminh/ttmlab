using Microsoft.AspNetCore.Identity;

namespace LMS.Ovncr.Models;

/// <summary>
/// Vai trò (Role) trong hệ thống. Kế thừa IdentityRole để tương thích
/// với cơ chế phân quyền của ASP.NET Core Identity.
/// Các role mặc định: "Admin", "User".
/// </summary>
public partial class AspNetRole : IdentityRole<string>
{
    // IdentityRole<string> đã cung cấp: Id, Name, NormalizedName, ConcurrencyStamp
    // Không cần khai báo thêm trường nào ở đây, ngoại trừ constructor
    // để gán giá trị mặc định cho Id để EF Core có thể track object mới.
    public AspNetRole()
    {
        Id = Guid.NewGuid().ToString();
    }

    public AspNetRole(string roleName) : this()
    {
        Name = roleName;
    }
}
