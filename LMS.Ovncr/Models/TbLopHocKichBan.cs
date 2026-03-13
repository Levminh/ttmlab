using System;
using System.Collections.Generic;

namespace LMS.Ovncr.Models;

public partial class TbLopHocKichBan
{
    public int IdLopHocKichBan { get; set; }

    public int? IdLopHoc { get; set; }

    public int? IdKichBan { get; set; }

    public string? GhiChu { get; set; }

    public virtual TbKichBan? IdKichBanNavigation { get; set; }

    public virtual TbLopHoc? IdLopHocNavigation { get; set; }
}
