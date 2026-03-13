using System;
using System.Collections.Generic;

namespace LMS.Ovncr.Models;

public partial class TbLichPhongLab
{
    public int Id { get; set; }

    public int? IdProject { get; set; }

    public string? IdUser { get; set; }

    public DateTime? TuNgay { get; set; }

    public DateTime? DenNgay { get; set; }

    public virtual TbProjectOpenStack? IdProjectNavigation { get; set; }

    public virtual AspNetUser? IdUserNavigation { get; set; }
}
