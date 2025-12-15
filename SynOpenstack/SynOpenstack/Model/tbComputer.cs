namespace SynOpenstack.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tbComputer")]
    public partial class tbComputer
    {
        public int Id { get; set; }

        [StringLength(500)]
        public string Name { get; set; }

        [StringLength(50)]
        public string IdOpenStack { get; set; }

        public int? IdProject { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(500)]
        public string VncUri { get; set; }
        [Column(TypeName = "ntext")]
        public string MoTa { get; set; }

        public virtual tbProjectOpenStack tbProjectOpenStack { get; set; }
    }
}
