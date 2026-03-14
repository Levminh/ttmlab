using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using LMS.Ovncr.Models;
using LMS.Ovncr.Data;

var builder = WebApplication.CreateBuilder(args);

// ================================================================
// 1. Cấu hình DbContext với connection string từ appsettings.json
// ================================================================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ================================================================
// 2. Cấu hình ASP.NET Core Identity
//    - User: AspNetUser, Role: AspNetRole, Key type: string
// ================================================================
builder.Services.AddIdentity<AspNetUser, AspNetRole>(options =>
{
    // --- Chính sách mật khẩu ---
    options.Password.RequireDigit = false;           // Không bắt buộc có số
    options.Password.RequiredLength = 6;             // Tối thiểu 6 ký tự
    options.Password.RequireNonAlphanumeric = false; // Không bắt buộc ký tự đặc biệt
    options.Password.RequireUppercase = false;       // Không bắt buộc chữ hoa
    options.Password.RequireLowercase = false;       // Không bắt buộc chữ thường

    // --- Chính sách khóa tài khoản (Lockout) ---
    options.Lockout.MaxFailedAccessAttempts = 5;              // Khóa sau 5 lần sai
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15); // Khóa 15 phút
    options.Lockout.AllowedForNewUsers = true;

    // --- Cài đặt user ---
    options.User.RequireUniqueEmail = false; // Email không bắt buộc là duy nhất
})
.AddEntityFrameworkStores<AppDbContext>()  // Lưu dữ liệu Identity vào SQL Server qua EF Core
.AddDefaultTokenProviders();               // Token provider cho reset mật khẩu, xác nhận email...

// ================================================================
// 3. Cấu hình Cookie xác thực (Authentication Cookie)
// ================================================================
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Auth/Login";           // Trang đăng nhập
    options.LogoutPath = "/Auth/Logout";         // Endpoint đăng xuất
    options.AccessDeniedPath = "/Auth/AccessDenied"; // Trang báo không đủ quyền
    options.ExpireTimeSpan = TimeSpan.FromHours(8);  // Cookie hết hạn sau 8 giờ
    options.SlidingExpiration = true;             // Gia hạn cookie mỗi khi user hoạt động
});

// 4. Thêm MVC Controllers với Views
builder.Services.AddControllersWithViews();

var app = builder.Build();

// ================================================================
// 5. Khởi tạo dữ liệu mặc định (Seed Roles + Admin account)c
//    Chạy trước khi app bắt đầu nhận request
// ================================================================
await SeedData.InitializeAsync(app.Services);

// ================================================================
// 6. Cấu hình HTTP pipeline (middleware)
// ================================================================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// QUAN TRỌNG: UseAuthentication phải được gọi TRƯỚC UseAuthorization
app.UseAuthentication(); // Xử lý xác thực (đọc cookie, token...)
app.UseAuthorization();  // Kiểm tra quyền truy cập (roles, policies...)

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
