namespace ttm3._0.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tbLichPhongLab")]
    public partial class tbLichPhongLab
    {
        public int Id { get; set; }

        [Display(Name = "Phòng Lab")]
        public int? IdProject { get; set; }

        [Display(Name = "Người sử dụng")]
        [StringLength(128)]
        public string IdUser { get; set; }

        [Display(Name = "Thời gian từ")]
        public DateTime? TuNgay { get; set; }

        [Display(Name = "Đến")]
        public DateTime? DenNgay { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }

        public virtual tbProjectOpenStack tbProjectOpenStack { get; set; }

        [Required]
        [Display(Name = "Bắt đầu từ giờ")]
        [NotMapped]

        public string TuGio { get; set; }
        [Required]
        [Display(Name = "Đến giờ")]
        [NotMapped]

        public string DenGio { get; set; }
        [Required]
        [Display(Name = "Ngày")]
        [NotMapped]

        public string stTuNgay { get; set; }
        [Required]
        [Display(Name = "Ngày")]
        [NotMapped]
        public string stDenNgay { get; set; }

    }
}
