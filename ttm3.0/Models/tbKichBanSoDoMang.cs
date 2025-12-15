namespace ttm3._0.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tbKichBanSoDoMang")]
    public partial class tbKichBanSoDoMang
    {
        public int Id { get; set; }

        public int? IdKichBan { get; set; }

        public int? IdSoDoMang { get; set; }

        public virtual tbKichBan tbKichBan { get; set; }

        public virtual tbSoDoMang tbSoDoMang { get; set; }
    }
}
