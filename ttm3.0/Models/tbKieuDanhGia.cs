namespace ttm3._0.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tbKieuDanhGia")]
    public partial class tbKieuDanhGia
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbKieuDanhGia()
        {
            tbKichBans = new HashSet<tbKichBan>();
        }

        [Key]
        public int IdKieuDanhGia { get; set; }

        [Display(Name ="Kiểu đánh giá")]
        [StringLength(50)]
        public string KieuDanhGia { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbKichBan> tbKichBans { get; set; }
    }
}
