namespace ttm3._0.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tbNhom")]
    public partial class tbNhom
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbNhom()
        {
            tbKichBanNhoms = new HashSet<tbKichBanNhom>();
        }

        [Key]
        public int IdNhom { get; set; }

        [Display(Name = "Nhóm")]
        [StringLength(50)]
        public string Nhom { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbKichBanNhom> tbKichBanNhoms { get; set; }
    }
}
