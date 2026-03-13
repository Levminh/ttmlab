using System;
using System.Collections.Generic;

namespace LMS.Ovncr.Models;

public partial class TbLopHoc
{
    public int IdLopHoc { get; set; }

    public string? LopHoc { get; set; }

    public string? ThongTin { get; set; }

    public string? NguoiTao { get; set; }

    public int? IdProjectOs { get; set; }

    public virtual TbProjectOpenStack? IdProjectOsNavigation { get; set; }

    public virtual AspNetUser? NguoiTaoNavigation { get; set; }

    public virtual ICollection<TbHocVienLopHoc> TbHocVienLopHocs { get; set; } = new List<TbHocVienLopHoc>();

    public virtual ICollection<TbKetQua> TbKetQuas { get; set; } = new List<TbKetQua>();

    public virtual ICollection<TbLopHocKichBan> TbLopHocKichBans { get; set; } = new List<TbLopHocKichBan>();
}
