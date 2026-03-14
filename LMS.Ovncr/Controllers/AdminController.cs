using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LMS.Ovncr.Models;
using LMS.Ovncr.ViewModels;

namespace LMS.Ovncr.Controllers;

/// <summary>
/// Controller quản trị tài khoản người dùng (dành riêng cho Admin).
/// Chỉ người có role "Admin" mới có thể truy cập.
/// </summary>
[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly UserManager<AspNetUser> _userManager;
    private readonly RoleManager<AspNetRole> _roleManager;
    private readonly ILogger<AdminController> _logger;

    public AdminController(
        UserManager<AspNetUser> userManager,
        RoleManager<AspNetRole> roleManager,
        ILogger<AdminController> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    // ================================================================
    // DANH SÁCH NGƯỜI DÙNG
    // ================================================================

    /// <summary>
    /// GET: /Admin/Users
    /// Hiển thị danh sách tất cả người dùng trong hệ thống kèm vai trò và trạng thái.
    /// </summary>
    public async Task<IActionResult> Users(string? search = null)
    {
        IQueryable<AspNetUser> query = _userManager.Users;

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim();
            query = query.Where(u => u.UserName.Contains(search) 
                                  || u.FullName.Contains(search) 
                                  || u.Email.Contains(search));
        }

        // Sắp xếp theo tên đăng nhập để dễ tìm
        var users = query.OrderBy(u => u.UserName).ToList();

        // Giữ lại từ khóa tìm kiếm để truyền ra View
        ViewData["SearchQuery"] = search;

        // Xây dựng danh sách ViewModel cho từng user
        var userList = new List<UserListViewModel>();
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            userList.Add(new UserListViewModel
            {
                Id = user.Id,
                UserName = user.UserName ?? "",
                FullName = user.FullName,
                Email = user.Email,
                Roles = roles,
                // Tài khoản bị khóa khi LockoutEnd còn trong tương lai
                IsLockedOut = user.LockoutEnd.HasValue && user.LockoutEnd > DateTimeOffset.UtcNow,
                LockoutEnd = user.LockoutEnd
            });
        }

        return View(userList);
    }

    // ================================================================
    // TẠO NGƯỜI DÙNG MỚI
    // ================================================================

    /// <summary>
    /// GET: /Admin/CreateUser
    /// Hiển thị form tạo tài khoản mới với dropdown chọn vai trò.
    /// </summary>
    [HttpGet]
    public IActionResult CreateUser()
    {
        var model = new CreateUserViewModel
        {
            // Lấy danh sách tất cả roles trong hệ thống
            AvailableRoles = _roleManager.Roles.Select(r => r.Name!).ToList()
        };
        return View(model);
    }

    /// <summary>
    /// POST: /Admin/CreateUser
    /// Tạo tài khoản người dùng mới và gán vai trò được chỉ định.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateUser(CreateUserViewModel model)
    {
        // Load lại AvailableRoles cho trường hợp có lỗi cần hiển thị lại form
        model.AvailableRoles = _roleManager.Roles.Select(r => r.Name!).ToList();

        if (!ModelState.IsValid)
            return View(model);

        // Tạo đối tượng user mới
        var user = new AspNetUser
        {
            Id = Guid.NewGuid().ToString(),
            UserName = model.UserName,
            Email = model.Email,
            FullName = model.FullName,
            EmailConfirmed = true   // Admin tạo thì bỏ qua xác nhận email
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            _logger.LogInformation("Admin đã tạo tài khoản '{UserName}' với vai trò '{Role}'.", model.UserName, model.Role);

            // Gán vai trò cho user mới tạo
            if (!string.IsNullOrEmpty(model.Role))
                await _userManager.AddToRoleAsync(user, model.Role);

            TempData["SuccessMessage"] = $"Tạo tài khoản '{model.UserName}' thành công!";
            return RedirectToAction(nameof(Users));
        }

        foreach (var error in result.Errors)
            ModelState.AddModelError(string.Empty, error.Description);

        return View(model);
    }

    // ================================================================
    // CHỈNH SỬA NGƯỜI DÙNG
    // ================================================================

    /// <summary>
    /// GET: /Admin/EditUser/{id}
    /// Hiển thị form chỉnh sửa thông tin người dùng.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> EditUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound($"Không tìm thấy người dùng với ID: {id}");

        // Lấy vai trò hiện tại của user (lấy cái đầu tiên nếu có nhiều)
        var roles = await _userManager.GetRolesAsync(user);

        var model = new EditUserViewModel
        {
            Id = user.Id,
            UserName = user.UserName,
            FullName = user.FullName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Role = roles.FirstOrDefault() ?? "",
            AvailableRoles = _roleManager.Roles.Select(r => r.Name!).ToList()
        };

        return View(model);
    }

    /// <summary>
    /// POST: /Admin/EditUser/{id}
    /// Lưu thay đổi thông tin người dùng (thông tin cơ bản + vai trò + mật khẩu mới nếu có).
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditUser(EditUserViewModel model)
    {
        model.AvailableRoles = _roleManager.Roles.Select(r => r.Name!).ToList();

        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.FindByIdAsync(model.Id);
        if (user == null)
            return NotFound();

        // --- Cập nhật thông tin cơ bản ---
        user.FullName = model.FullName;
        user.Email = model.Email;
        user.PhoneNumber = model.PhoneNumber;

        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            foreach (var error in updateResult.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
            return View(model);
        }

        // --- Cập nhật vai trò ---
        var currentRoles = await _userManager.GetRolesAsync(user);
        // Xóa tất cả vai trò hiện tại
        await _userManager.RemoveFromRolesAsync(user, currentRoles);
        // Gán vai trò mới được chọn
        if (!string.IsNullOrEmpty(model.Role))
            await _userManager.AddToRoleAsync(user, model.Role);

        // --- Đặt lại mật khẩu mới (nếu Admin nhập) ---
        if (!string.IsNullOrEmpty(model.NewPassword))
        {
            // Xóa mật khẩu cũ và đặt mật khẩu mới (Admin không cần nhập MK cũ)
            await _userManager.RemovePasswordAsync(user);
            await _userManager.AddPasswordAsync(user, model.NewPassword);
        }

        _logger.LogInformation("Admin đã cập nhật tài khoản '{UserName}'.", user.UserName);
        TempData["SuccessMessage"] = $"Cập nhật tài khoản '{user.UserName}' thành công!";
        return RedirectToAction(nameof(Users));
    }

    // ================================================================
    // XÓA NGƯỜI DÙNG
    // ================================================================

    /// <summary>
    /// POST: /Admin/DeleteUser/{id}
    /// Xóa tài khoản người dùng. Dùng POST để tránh xóa nhầm qua GET request.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            TempData["ErrorMessage"] = "Không tìm thấy người dùng cần xóa.";
            return RedirectToAction(nameof(Users));
        }

        // Không cho phép xóa tài khoản mình đang dùng
        var currentUser = await _userManager.GetUserAsync(User);
        if (user.Id == currentUser?.Id)
        {
            TempData["ErrorMessage"] = "Không thể xóa tài khoản đang đăng nhập.";
            return RedirectToAction(nameof(Users));
        }

        var result = await _userManager.DeleteAsync(user);
        if (result.Succeeded)
        {
            _logger.LogInformation("Admin đã xóa tài khoản '{UserName}'.", user.UserName);
            TempData["SuccessMessage"] = $"Đã xóa tài khoản '{user.UserName}'.";
        }
        else
        {
            TempData["ErrorMessage"] = "Xóa tài khoản thất bại. Vui lòng thử lại.";
        }

        return RedirectToAction(nameof(Users));
    }

    // ================================================================
    // KHÓA / MỞ KHÓA TÀI KHOẢN
    // ================================================================

    /// <summary>
    /// POST: /Admin/ToggleLockout/{id}
    /// Khóa tài khoản 30 ngày nếu đang mở, hoặc mở khóa nếu đang bị khóa.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleLockout(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            TempData["ErrorMessage"] = "Không tìm thấy người dùng.";
            return RedirectToAction(nameof(Users));
        }

        // Không cho phép khóa chính mình
        var currentUser = await _userManager.GetUserAsync(User);
        if (user.Id == currentUser?.Id)
        {
            TempData["ErrorMessage"] = "Không thể khóa tài khoản đang đăng nhập.";
            return RedirectToAction(nameof(Users));
        }

        bool isCurrentlyLocked = user.LockoutEnd.HasValue && user.LockoutEnd > DateTimeOffset.UtcNow;

        if (isCurrentlyLocked)
        {
            // Mở khóa: đặt LockoutEnd về quá khứ
            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddDays(-1));
            _logger.LogInformation("Admin đã mở khóa tài khoản '{UserName}'.", user.UserName);
            TempData["SuccessMessage"] = $"Đã mở khóa tài khoản '{user.UserName}'.";
        }
        else
        {
            // Khóa: đặt LockoutEnd về tương lai xa (30 ngày)
            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddDays(30));
            _logger.LogInformation("Admin đã khóa tài khoản '{UserName}'.", user.UserName);
            TempData["SuccessMessage"] = $"Đã khóa tài khoản '{user.UserName}'.";
        }

        return RedirectToAction(nameof(Users));
    }
}
