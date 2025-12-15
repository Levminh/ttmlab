using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ttm3._0.Models
{
    public class cleve
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Địa chỉ máy chủ EVE")]
        public string Ip { get; set; }

        [Display(Name = "Tên đăng nhập máy chủ EVE")]
        public string Username { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu máy chủ EVE")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu không trùng")]
        public string RePassword { get; set; }

        [Display(Name = "Đường dẫn lưu sơ đồ mạng")]
        public string Path { get; set; }

        [Display(Name = "Tên đăng nhập dịch vụ EVE")]
        public string UsernameEve { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu dịch vụ EVE")]
        public string PasswordEve { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("PasswordEve", ErrorMessage = "Mật khẩu không trùng")]
        public string RePasswordEve { get; set; }

    }
}