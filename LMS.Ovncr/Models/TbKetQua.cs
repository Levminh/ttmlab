using System;
using System.Collections.Generic;

namespace LMS.Ovncr.Models;

public partial class TbKetQua
{
    public int IdKetQua { get; set; }

    public int? IdKichBan { get; set; }

    public int? IdLopHoc { get; set; }

    public string? IdHocVien { get; set; }

    public double? Diem { get; set; }

    public string? DapAn { get; set; }

    public string? UrlFileDapAn { get; set; }

    public string? GhiChu { get; set; }

    public virtual AspNetUser? IdHocVienNavigation { get; set; }

    public virtual TbKichBan? IdKichBanNavigation { get; set; }

    public virtual TbLopHoc? IdLopHocNavigation { get; set; }
}
