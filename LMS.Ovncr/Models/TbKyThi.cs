using System;
using System.Collections.Generic;

namespace LMS.Ovncr.Models;

public partial class TbKyThi
{
    public int Id { get; set; }

    public string? UserId { get; set; }

    public int? KyThi { get; set; }

    public DateTime? NgayTao { get; set; }

    public DateTime? ThoiGianBatDat { get; set; }

    public DateTime? ThoiGianKetThuc { get; set; }

    public int? Diem { get; set; }

    public string? DanhMucCauHoi { get; set; }

    public virtual ICollection<TbTraLoi> TbTraLois { get; set; } = new List<TbTraLoi>();

    public virtual AspNetUser? User { get; set; }
}
