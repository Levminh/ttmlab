namespace ttm3._0.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tbSoDoMangMay")]
    public partial class tbSoDoMangMay
    {
        public int Id { get; set; }

        public int? IdSoDoMang { get; set; }

        [Display(Name = "Mã máy")]
        [StringLength(200)]
        public string MaMay { get; set; }

        [Display(Name = "Tên máy")]
        [StringLength(200)]
        public string TenMay { get; set; }

        [Display(Name = "Kiểu máy")]
        [StringLength(100)]
        public string Kieu { get; set; }

        [Display(Name = "Ghi chú")]
        [StringLength(500)]
        public string GhiChu { get; set; }

        public virtual tbSoDoMang tbSoDoMang { get; set; }
    }
}
