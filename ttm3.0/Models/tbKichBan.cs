namespace ttm3._0.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using System.Web.Mvc;

    [Table("tbKichBan")]
    public partial class tbKichBan
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbKichBan()
        {
            tbKetQuas = new HashSet<tbKetQua>();
            tbLopHocKichBans = new HashSet<tbLopHocKichBan>();
            tbKichBanNhoms = new HashSet<tbKichBanNhom>();
            tbKichBanSoDoMangs = new HashSet<tbKichBanSoDoMang>();
        }

        [Key]
        public int IdKichBan { get; set; }

        [Display(Name ="Mã kịch bản")]
        [StringLength(50)]
        public string MaKichBan { get; set; }

        [AllowHtml]
        [Display(Name = "Tên kịch bản")]
        [StringLength(1000)]
        public string TenKichBan { get; set; }

        [AllowHtml]
        [Display(Name = "Mục tiêu")]
        [Column(TypeName = "ntext")]
        public string MucTieu { get; set; }

        [AllowHtml]
        [Display(Name = "Mô tả")]
        [Column(TypeName = "ntext")]
        public string NoiDung { get; set; }

        [AllowHtml]
        [Display(Name = "Yêu cầu")]
        [Column(TypeName = "ntext")]
        public string YeuCau { get; set; }

        [Display(Name = "Kiểu đánh giá")]
        public int? IdKieuDanhGia { get; set; }

        [AllowHtml]
        [Display(Name = "Hướng dẫn thực hành")]
        [Column(TypeName = "ntext")]
        public string HuongDanThucHanh { get; set; }

        [Display(Name = "Đáp án")]
        [StringLength(1000)]
        public string DapAn { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbKetQua> tbKetQuas { get; set; }

        public virtual tbKieuDanhGia tbKieuDanhGia { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbLopHocKichBan> tbLopHocKichBans { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbKichBanNhom> tbKichBanNhoms { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbKichBanSoDoMang> tbKichBanSoDoMangs { get; set; }

        [NotMapped]
        [Display(Name = "Nhóm")]
        public string Nhoms
        {
            get { return string.Join(",", tbKichBanNhoms.Select(p => p.tbNhom).Select(o => o.Nhom).ToArray()); }
        }

        [NotMapped]
        public int? IdKetQua { get; set; }

        [NotMapped]
        public double? Diem { get; set; }

        [NotMapped]
        public int[] Nhom { get; set; }

        [NotMapped]
        public int[] GetNhom { get
            {
                return tbKichBanNhoms.Select(o => o.IdNhom).ToArray();
            }
        }
    }
}
