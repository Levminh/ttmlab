namespace ttm3._0.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    [Table("tbHocVienLopHoc")]
    public partial class tbHocVienLopHoc
    {
        [NotMapped]
        public int TT { get; set; }

        [Key]
        public int IdHocVienLopHoc { get; set; }

        public int? IdLopHoc { get; set; }

        [Display(Name = "Học viên")]
        [StringLength(128)]
        public string IdHocVien { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }

        public virtual tbLopHoc tbLopHoc { get; set; }

        [Display(Name = "Tổng điểm")]
        [NotMapped]
        public double? TongDiem { get; set; }

        [AllowHtml]
        [Column(TypeName = "ntext")]
        [Display(Name = "Ghi chú")]
        public string GhiChu { get; set; }

        [Display(Name = "Id Instance")]
        [StringLength(200)]
        public string IdInstance { get; set; }

        public int? IdComputer { get; set; }

        public virtual tbComputer tbComputer { get; set; }
    }
}
