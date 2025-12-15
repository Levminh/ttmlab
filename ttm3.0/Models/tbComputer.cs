namespace ttm3._0.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tbComputer")]
    public partial class tbComputer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbComputer()
        {
            tbHocVienLopHocs = new HashSet<tbHocVienLopHoc>();
        }
        
        public int Id { get; set; }

        [StringLength(500)]
        public string Name { get; set; }

        [StringLength(50)]
        public string IdOpenStack { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [Column(TypeName = "ntext")]
        public string MoTa { get; set; }

        public int? IdProject { get; set; }

        [StringLength(500)]
        public string VncUri { get; set; }

        public virtual tbProjectOpenStack tbProjectOpenStack { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbHocVienLopHoc> tbHocVienLopHocs { get; set; }
    }
}
