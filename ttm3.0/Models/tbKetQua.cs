namespace ttm3._0.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tbKetQua")]
    public partial class tbKetQua
    {
        [Key]
        public int IdKetQua { get; set; }

        public int? IdKichBan { get; set; }

        public int? IdLopHoc { get; set; }


        [Display(Name = "Học viên")]
        [StringLength(128)]
        public string IdHocVien { get; set; }


        [Display(Name = "Điểm")]
        public double? Diem { get; set; }
        
        [Display(Name = "Đáp án")]
        [StringLength(1000)]
        public string DapAn { get; set; }

        [Display(Name = "Url file đáp án")]
        [StringLength(1000)]
        public string UrlFileDapAn { get; set; }

        [Display(Name = "Ghi chú")]
        [StringLength(1000)]
        public string GhiChu { get; set; }

        [NotMapped]
        public int? KieuDanhGia { get { return tbKichBan.IdKieuDanhGia; } }
        public virtual AspNetUser AspNetUser { get; set; }

        public virtual tbKichBan tbKichBan { get; set; }

        public virtual tbLopHoc tbLopHoc { get; set; }
    }
}
