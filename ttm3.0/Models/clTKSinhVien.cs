using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ttm3._0.Models
{
    public class clTKSinhVien
    {
        public AspNetUser SinhVien { get; set; }
        public string Lop { get; set; }
        public double? TongDiem { get; set; }
    }
}