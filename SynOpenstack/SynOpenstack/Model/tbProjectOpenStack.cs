namespace SynOpenstack.Model
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
        }

        [Key]
        public int IdProject { get; set; }

        [StringLength(500)]
        public string ProjectName { get; set; }

        [StringLength(500)]
        public string ProjectId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbComputer> tbComputers { get; set; }
    }
}
