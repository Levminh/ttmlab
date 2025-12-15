namespace ttm3._0.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tbProjectOpenStack")]
    public partial class tbProjectOpenStack
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbProjectOpenStack()
        {
            tbComputers = new HashSet<tbComputer>();
            tbLopHocs = new HashSet<tbLopHoc>();
            tbLichPhongLabs = new HashSet<tbLichPhongLab>();
            tbGiamSats = new HashSet<tbGiamSat>();
        }

        [Key]
        public int IdProject { get; set; }

        [Display(Name = "Project Name")]
        [StringLength(500)]
        public string ProjectName { get; set; }

        [Display(Name = "Project Id")]
        [StringLength(500)]
        public string ProjectId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbComputer> tbComputers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbLopHoc> tbLopHocs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbLichPhongLab> tbLichPhongLabs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbGiamSat> tbGiamSats { get; set; }
    }
}
