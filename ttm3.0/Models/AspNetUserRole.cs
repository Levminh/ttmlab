namespace ttm3._0.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AspNetUserRole
    {
        [Key]
        [Column(Order = 0)]
        public string UserId { get; set; }

        [Display(Name ="Quyền hạn")]
        [Key]
        [Column(Order = 1)]
        public string RoleId { get; set; }

        public virtual AspNetRole AspNetRole { get; set; }
    }
}
