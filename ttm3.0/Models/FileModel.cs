using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ttm3._0.Models
{
    public class FileModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Chọn file")]
        [Display(Name = "Duyệt File")]
        public HttpPostedFileBase files { get; set; }

    }
}