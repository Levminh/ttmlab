using System;
using System.Collections.Generic;

namespace LMS.Ovncr.Models;

public partial class TbComputer
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? IdOpenStack { get; set; }

    public int? IdProject { get; set; }

    public string? Status { get; set; }

    public string? VncUri { get; set; }

    public string? MoTa { get; set; }

    public virtual TbProjectOpenStack? IdProjectNavigation { get; set; }

    public virtual ICollection<TbHocVienLopHoc> TbHocVienLopHocs { get; set; } = new List<TbHocVienLopHoc>();
}
