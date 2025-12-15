namespace ttm3._0.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tbGiamSat")]
    public partial class tbGiamSat
    {
        public int Id { get; set; }

        [Display(Name ="Giám sát")]
        [StringLength(500)]
        public string Ten { get; set; }

        [Display(Name = "Link")]
        [StringLength(500)]
        public string Link { get; set; }
        public string Link2 {
            get{
                if (Link != null || Link.Length<=100)
                {
                    return Link.Substring(0, 100) + "...";
                }
                else
                    return Link;
            }
        }

        public int? IdProject { get; set; }
        public virtual tbProjectOpenStack tbProjectOpenStack { get; set; }

        [NotMapped]
        public int? TT { get; set; }
    }
}
