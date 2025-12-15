namespace ttm3._0.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tbLopHocKichBan")]
    public partial class tbLopHocKichBan
    {
        [Key]
        public int IdLopHocKichBan { get; set; }

        public int? IdLopHoc { get; set; }

        [Display(Name ="Kịch bản")]
        public int? IdKichBan { get; set; }

        [Display(Name = "Ghi chú")]
        [StringLength(500)]
        public string GhiChu { get; set; }

        public virtual tbKichBan tbKichBan { get; set; }

        public virtual tbLopHoc tbLopHoc { get; set; }
    }
}
