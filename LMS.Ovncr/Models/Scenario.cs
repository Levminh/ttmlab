using Microsoft.AspNetCore.Mvc;
using LMS.Ovncr.Models;
using System;
using System.Collections.Generic;
// Đây là file Model cho Kịch bản diễn tập trong hệ thống OVNCR ;()))()())()
namespace LMS.Ovncr.Models;
public class Scenario
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Difficulty { get; set; } = "Medium"; // Easy, Medium, Hard
    public string Category { get; set; } = "Pentest";  // Web, Network, Pwn...
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public bool IsActive { get; set; }
}