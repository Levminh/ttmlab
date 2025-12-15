using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ttm3._0.Models
{
    public class clTaiKhoan
    {
        [Key]
        public int IdLop { get; set; }

        [Required]
        [Display(Name ="Tiếp đầu ngữ")]
        public string TiepDauNgu { get; set; }

        [Required]
        [Display(Name = "Số tài khoản")]
        public int? SoTaiKhoan { get; set; }

        [Required]
        [Display(Name = "Bắt đầu")]
        public int? BatDau { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu không trùng")]
        public string ConfirmPassword { get; set; }
    }
}