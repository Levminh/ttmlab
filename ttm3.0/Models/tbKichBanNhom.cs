namespace ttm3._0.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tbKichBanNhom")]
    public partial class tbKichBanNhom
    {
        [Key]
        public int IdKichBanNhom { get; set; }

        public int IdNhom { get; set; }

        public int IdKichBan { get; set; }

        public virtual tbKichBan tbKichBan { get; set; }

        public virtual tbNhom tbNhom { get; set; }
    }
}
