using System;
using System.Collections.Generic;

namespace LMS.Ovncr.Models;

public partial class TbHocVienLopHoc
{
    public int IdHocVienLopHoc { get; set; }

    public int? IdLopHoc { get; set; }

    public string? IdHocVien { get; set; }

    public string? GhiChu { get; set; }

    public string? IdInstance { get; set; }

    public int? IdComputer { get; set; }

    public virtual TbComputer? IdComputerNavigation { get; set; }

    public virtual AspNetUser? IdHocVienNavigation { get; set; }

    public virtual TbLopHoc? IdLopHocNavigation { get; set; }
}
