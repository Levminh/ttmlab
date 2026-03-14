using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LMS.Ovncr.Models;
using LMS.Ovncr.ViewModels;

namespace LMS.Ovncr.Controllers;

/// <summary>
/// Controller xử lý xác thực người dùng: Đăng nhập, Đăng ký, Đăng xuất.
/// </summary>
public class AuthController : Controller
{
    private readonly SignInManager<AspNetUser> _signInManager;
    private readonly UserManager<AspNetUser> _userManager;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        SignInManager<AspNetUser> signInManager,
        UserManager<AspNetUser> userManager,
        ILogger<AuthController> logger)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
    }

    // ================================================================
    // ĐĂNG NHẬP
    // ================================================================

    /// <summary>
    /// GET: /Auth/Login
    /// Hiển thị form đăng nhập. Nếu đã đăng nhập thì redirect về trang chủ.
    /// </summary>
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        // Nếu user đã đăng nhập rồi thì không cần vào trang Login nữa
        if (_signInManager.IsSignedIn(User))
            return RedirectToAction("Index", "Home");

        var model = new LoginViewModel { ReturnUrl = returnUrl };
        return View(model);
    }

    /// <summary>
    /// POST: /Auth/Login
    /// Xử lý đăng nhập: kiểm tra username/password, tạo cookie xác thực.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        // Thử đăng nhập với SignInManager (tự động hash + so sánh mật khẩu)
        var result = await _signInManager.PasswordSignInAsync(
            model.UserName,
            model.Password,
            model.RememberMe,
            lockoutOnFailure: true  // Kích hoạt cơ chế khóa tài khoản khi sai nhiều lần
        );

        if (result.Succeeded)
        {
            _logger.LogInformation("Người dùng '{UserName}' đã đăng nhập thành công.", model.UserName);

            // Redirect về trang gốc hoặc về trang chủ
            if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                return Redirect(model.ReturnUrl);

            return RedirectToAction("Index", "Home");
        }

        if (result.IsLockedOut)
        {
            // Tài khoản đang bị khóa do đăng nhập sai quá nhiều lần
            _logger.LogWarning("Tài khoản '{UserName}' bị khóa.", model.UserName);
            ModelState.AddModelError(string.Empty, "Tài khoản đang bị khóa tạm thời. Vui lòng thử lại sau 15 phút.");
        }
        else
        {
            // Sai username hoặc mật khẩu
            ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không đúng.");
        }

        return View(model);
    }

    // ================================================================
    // ĐĂNG KÝ
    // ================================================================

    /// <summary>
    /// GET: /Auth/Register
    /// Hiển thị form đăng ký tài khoản mới.
    /// </summary>
    [HttpGet]
    public IActionResult Register()
    {
        // Nếu đã đăng nhập thì không cho vào trang đăng ký
        if (_signInManager.IsSignedIn(User))
            return RedirectToAction("Index", "Home");

        return View();
    }

    /// <summary>
    /// POST: /Auth/Register
    /// Tạo tài khoản mới, gán role "HocVien", tự động đăng nhập sau khi đăng ký.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        // Tạo đối tượng user mới
        var user = new AspNetUser
        {
            Id = Guid.NewGuid().ToString(),
            UserName = model.UserName,
            Email = model.Email,
            FullName = model.FullName,
            EmailConfirmed = true // Bỏ qua xác nhận email (có thể bật sau)
        };

        // Tạo user với mật khẩu (Identity tự hash)
        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            _logger.LogInformation("Tài khoản '{UserName}' được tạo thành công.", model.UserName);

            // Gán vai trò mặc định là Học Viên
            await _userManager.AddToRoleAsync(user, "Student");

            // Tự động đăng nhập sau khi đăng ký
            await _signInManager.SignInAsync(user, isPersistent: false);

            return RedirectToAction("Index", "Home");
        }

        // Có lỗi khi tạo user -> hiển thị lỗi cho người dùng
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }

    // ================================================================
    // ĐĂNG XUẤT
    // ================================================================

    /// <summary>
    /// POST: /Auth/Logout
    /// Xóa cookie xác thực, đăng xuất người dùng hiện tại.
    /// Chỉ cho phép POST để tránh CSRF (không ai có thể ép đăng xuất bằng link).
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        _logger.LogInformation("Người dùng đã đăng xuất.");
        return RedirectToAction("Login", "Auth");
    }

    // ================================================================
    // TRUY CẬP BỊ TỪ CHỐI
    // ================================================================

    /// <summary>
    /// GET: /Auth/AccessDenied
    /// Trang thông báo khi người dùng không có quyền truy cập.
    /// </summary>
    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }
}
