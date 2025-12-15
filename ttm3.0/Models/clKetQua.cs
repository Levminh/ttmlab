using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ttm3._0.Models
{
    public class clKetQua
    {
        public clKetQua()
        {
            lsKQ = new List<tbKetQua>();
        }
        [Key]
        public AspNetUser User { get; set; }
        public List<tbKetQua> lsKQ { get; set; }

        public double? TongDiem
        {
            get { return lsKQ.Sum(o => o.Diem); }
        }
    }
}