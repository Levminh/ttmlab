namespace ttm3._0.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tbSoDoMang")]
    public partial class tbSoDoMang
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbSoDoMang()
        {
            tbSoDoMangMays = new HashSet<tbSoDoMangMay>();
            tbKichBanSoDoMangs = new HashSet<tbKichBanSoDoMang>();
        }

        public int Id { get; set; }

        [Display(Name = "Tên sơ đồ mạng")]
        [StringLength(200)]
        public string Ten { get; set; }

        [Display(Name = "UUID")]
        [StringLength(200)]
        public string UUID { get; set; }

        [Display(Name = "Path")]
        [StringLength(500)]
        public string Path { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbSoDoMangMay> tbSoDoMangMays { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbKichBanSoDoMang> tbKichBanSoDoMangs { get; set; }
    }
}
