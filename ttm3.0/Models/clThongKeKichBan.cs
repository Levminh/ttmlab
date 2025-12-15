using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ttm3._0.Models
{
    public class clThongKeKichBan
    {
        public clThongKeKichBan()
        {
        }
        [Key]
        public tbKichBan tbKichBan { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        [Display(Name = "Điểm TB")]
        public double? DiemTB { get; set; }
    }
}