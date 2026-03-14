using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LMS.Ovncr.Models;
using LMS.Ovncr.ViewModels;

namespace LMS.Ovncr.Controllers;

/// <summary>
/// Controller quản lý thông tin cá nhân của người dùng đang đăng nhập.
/// Tất cả các action đều yêu cầu đăng nhập [Authorize].
/// </summary>
[Authorize] // Toàn bộ controller yêu cầu đăng nhập
public class AccountController : Controller
{
    private readonly UserManager<AspNetUser> _userManager;
    private readonly SignInManager<AspNetUser> _signInManager;
    private readonly ILogger<AccountController> _logger;

    public AccountController(
        UserManager<AspNetUser> userManager,
        SignInManager<AspNetUser> signInManager,
        ILogger<AccountController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    // ================================================================
    // XEM & SỬA THÔNG TIN CÁ NHÂN
    // ================================================================

    /// <summary>
    /// GET: /Account/Profile
    /// Hiển thị thông tin cá nhân của user đang đăng nhập.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        // Lấy thông tin user hiện tại từ Claims Identity
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound("Không tìm thấy thông tin tài khoản.");

        // Điền thông tin vào ViewModel để hiển thị
        var model = new EditProfileViewModel
        {
            UserName = user.UserName,
            FullName = user.FullName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber
        };

        return View(model);
    }

    /// <summary>
    /// POST: /Account/Profile
    /// Lưu thay đổi thông tin cá nhân (FullName, Email, PhoneNumber).
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Profile(EditProfileViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound("Không tìm thấy thông tin tài khoản.");

        // Cập nhật các trường thông tin
        user.FullName = model.FullName;
        user.Email = model.Email;
        user.PhoneNumber = model.PhoneNumber;

        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
        {
            _logger.LogInformation("Người dùng '{UserName}' đã cập nhật thông tin cá nhân.", user.UserName);

            // Cập nhật lại Security Stamp để cookie vẫn còn hiệu lực
            await _signInManager.RefreshSignInAsync(user);

            TempData["SuccessMessage"] = "Cập nhật thông tin thành công!";
            return RedirectToAction(nameof(Profile));
        }

        // Thêm lỗi vào ModelState để hiển thị cho user
        foreach (var error in result.Errors)
            ModelState.AddModelError(string.Empty, error.Description);

        return View(model);
    }

    // ================================================================
    // ĐỔI MẬT KHẨU
    // ================================================================

    /// <summary>
    /// GET: /Account/ChangePassword
    /// Hiển thị form đổi mật khẩu.
    /// </summary>
    [HttpGet]
    public IActionResult ChangePassword()
    {
        return View();
    }

    /// <summary>
    /// POST: /Account/ChangePassword
    /// Xử lý đổi mật khẩu: kiểm tra mật khẩu cũ, cập nhật mật khẩu mới.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound("Không tìm thấy thông tin tài khoản.");

        // Đổi mật khẩu - Identity sẽ tự kiểm tra mật khẩu hiện tại và hash mật khẩu mới
        var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

        if (result.Succeeded)
        {
            _logger.LogInformation("Người dùng '{UserName}' đã đổi mật khẩu thành công.", user.UserName);

            // Cập nhật lại Security Stamp (invalidate các session cũ)
            await _signInManager.RefreshSignInAsync(user);

            TempData["SuccessMessage"] = "Đổi mật khẩu thành công!";
            return RedirectToAction(nameof(ChangePassword));
        }

        foreach (var error in result.Errors)
            ModelState.AddModelError(string.Empty, error.Description);

        return View(model);
    }
}
