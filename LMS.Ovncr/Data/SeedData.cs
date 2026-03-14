using LMS.Ovncr.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LMS.Ovncr.Data;

/// <summary>
/// Khởi tạo dữ liệu mặc định khi ứng dụng chạy lần đầu (Seed Data).
/// Tự động tạo các Role và tài khoản Admin mặc định nếu chưa tồn tại.
/// </summary>
public static class SeedData
{
    /// <summary>
    /// Phương thức khởi tạo chính, gọi từ Program.cs sau khi app được build.
    /// </summary>
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AspNetUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AspNetRole>>();
        var dbContext  = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // --- Bước 0: Normalize dữ liệu cũ (DB từ MVC5 có NormalizedName = NULL) ---
        // Identity Core tìm role theo NormalizedName; nếu NULL sẽ không thấy
        // → phải set NormalizedName trước để các câu lệnh bên dưới hoạt động đúng
        await NormalizeExistingDataAsync(dbContext);

        // --- Bước 1: Tạo các vai trò mặc định ---
        // Sử dụng lại các role đã có sẵn trong DB cũ
        await CreateRoleIfNotExists(roleManager, "Admin");      // Quản trị viên
        await CreateRoleIfNotExists(roleManager, "Teacher");    // Giảng viên (tương đương GiangVien)
        await CreateRoleIfNotExists(roleManager, "Student");    // Học viên (tương đương HocVien)

        // --- Bước 2: Tạo tài khoản Admin mặc định ---
        const string adminUserName = "admin";
        const string adminPassword = "Admin@123";   // Đổi mật khẩu này sau khi triển khai!

        var existingAdmin = await userManager.FindByNameAsync(adminUserName);
        if (existingAdmin == null)
        {
            var adminUser = new AspNetUser
            {
                Id        = Guid.NewGuid().ToString(),
                UserName  = adminUserName,
                Email     = "admin@lms.local",
                FullName  = "Quản trị viên hệ thống",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
            else
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Không thể tạo tài khoản admin mặc định: {errors}");
            }
        }
        else
        {
            // Tài khoản đã tồn tại từ hệ thống cũ, ta reset lại mật khẩu thành Admin@123 để đăng nhập
            var token = await userManager.GeneratePasswordResetTokenAsync(existingAdmin);
            await userManager.ResetPasswordAsync(existingAdmin, token, adminPassword);

            // Đồng thời đảm bảo tài khoản cũ này có quyền Admin
            if (!await userManager.IsInRoleAsync(existingAdmin, "Admin"))
            {
                await userManager.AddToRoleAsync(existingAdmin, "Admin");
            }
        }
    }

    /// <summary>
    /// Normalize dữ liệu từ DB cũ (ASP.NET MVC 5 Identity):
    ///   - AspNetRoles.NormalizedName  = UPPER(Name)
    ///   - AspNetUsers.NormalizedUserName = UPPER(UserName)
    ///   - AspNetUsers.NormalizedEmail    = UPPER(Email)
    ///   - AspNetUsers.ConcurrencyStamp   = NewGuid()   (nếu null)
    /// Nếu không làm bước này, RoleManager/UserManager sẽ không tìm thấy 
    /// các bản ghi cũ và cố tạo lại → lỗi duplicate key.
    /// </summary>
    private static async Task NormalizeExistingDataAsync(AppDbContext db)
    {
        bool changed = false;

        // Normalize Roles
        var roles = await db.Roles.ToListAsync();
        foreach (var role in roles)
        {
            if (role.Name != null && role.NormalizedName == null)
            {
                role.NormalizedName  = role.Name.ToUpperInvariant();
                role.ConcurrencyStamp ??= Guid.NewGuid().ToString();
                changed = true;
            }
        }

        // Normalize Users
        var users = await db.Users.ToListAsync();
        foreach (var user in users)
        {
            bool userChanged = false;

            if (user.UserName != null && user.NormalizedUserName == null)
            {
                user.NormalizedUserName = user.UserName.ToUpperInvariant();
                userChanged = true;
            }

            if (user.Email != null && user.NormalizedEmail == null)
            {
                user.NormalizedEmail = user.Email.ToUpperInvariant();
                userChanged = true;
            }

            if (user.ConcurrencyStamp == null)
            {
                user.ConcurrencyStamp = Guid.NewGuid().ToString();
                userChanged = true;
            }

            if (userChanged) changed = true;
        }

        if (changed)
            await db.SaveChangesAsync();
    }

    /// <summary>
    /// Tạo một Role nếu chưa tồn tại trong hệ thống.
    /// </summary>
    private static async Task CreateRoleIfNotExists(RoleManager<AspNetRole> roleManager, string roleName)
    {
        // RoleExistsAsync tìm theo NormalizedName (sau khi đã normalize bước 0 ở trên)
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            var result = await roleManager.CreateAsync(new AspNetRole 
            { 
                Id = Guid.NewGuid().ToString(),
                Name = roleName 
            });
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Không thể tạo role '{roleName}': {errors}");
            }
        }
    }
}
