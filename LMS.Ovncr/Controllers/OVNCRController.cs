using Microsoft.AspNetCore.Mvc;
using LMS.Ovncr.Models;
using System.Collections.Generic;
using System;

// Đây là file Controller chính cho module OVNCR, quản lý các hành động liên quan đến Kịch bản diễn tập
namespace LMS.Ovncr.Controllers
{
    [Route("About")] 
    public class OVNCRController : Controller
    {
        // 1. Trang Thông tin chung 
        [HttpGet("")] 
        [HttpGet("Index")]
        public IActionResult Index()
        { 
            return View();
        }

        // 2. Trang Quản lý kịch bản: Chuyển sang đường dẫn độc lập /kichban
        [HttpGet("/kichban")]
        public IActionResult Management()
        {
            var scenarios = new List<TbKichBan>
            {
                new TbKichBan { 
                    IdKichBan = 1, 
                    MaKichBan = "WEB_01",
                    TenKichBan = "Tấn công SQL Injection vào Cổng thông tin", 
                    MucTieu = "Giúp học viên nắm vững kỹ năng khai thác và vá lỗi SQLi.",
                    NoiDung = "Thực hành trên hệ thống web portal giả lập với các mức độ bảo mật khác nhau."
                },
                new TbKichBan { 
                    IdKichBan = 2, 
                    MaKichBan = "NET_02",
                    TenKichBan = "Kịch bản Diễn tập Chính phủ số", 
                    MucTieu = "Thực chiến quy trình ứng cứu sự cố mã độc tống tiền (Ransomware).",
                    NoiDung = "Phục hồi hệ thống máy chủ ảo hóa nội bộ sau khi bị mã hóa dữ liệu."
                }
            };
            return View(scenarios);
        }

        // 3. Trang Thêm kịch bản (Hiển thị Form): Đường dẫn /kichban/create
        
        [HttpGet("/kichban/create")]
        public IActionResult Create()
        {
            return View();
        }

        // 4. Xử lý nhận dữ liệu từ Form gửi lên
        [HttpPost("/kichban/create")]
        public IActionResult Create(TbKichBan kichBan)
        {
            if (ModelState.IsValid)
            {
                // Code lưu vào db sau này...
                return RedirectToAction("Management");
            }
            return View(kichBan);
        }
    }
}