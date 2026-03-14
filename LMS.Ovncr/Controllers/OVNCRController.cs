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
        
        [HttpGet("")] 
        [HttpGet("Index")]
        public IActionResult Index()
        { // Code lấy danh sách kịch bản từ database (giả sử đã có service)
            return View();
        }

        [HttpGet("Management")]
        public IActionResult Management()
        {
            var scenarios = new List<Scenario>
            { // Dữ liệu mẫu, thay bằng truy vấn từ database
                new Scenario { Id = 1, Name = "Tấn công SQL Injection vào Cổng thông tin", Difficulty = "Easy", Category = "Web", IsActive = true },
                new Scenario { Id = 2, Name = "Kịch bản Diễn tập Chính phủ số - Phục hồi hệ thống", Difficulty = "Hard", Category = "Network", IsActive = true },
                new Scenario { Id = 3, Name = "Phát hiện mã độc trong mạng nội bộ", Difficulty = "Medium", Category = "Pwn", IsActive = true }
            };
            return View(scenarios);
        }

      
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

   
        [HttpPost("Create")]
        public IActionResult Create(Scenario scenario)
        {
            if (ModelState.IsValid)
            {
                // Code lưu vào db
                return RedirectToAction("Management");
            }
            return View(scenario);
        }
    }
}