using System;
using System.Collections.Generic;

namespace LMS.Ovncr.Models;

public partial class TbKieuDanhGium
{
    public int IdKieuDanhGia { get; set; }

    public string? KieuDanhGia { get; set; }

    public virtual ICollection<TbKichBan> TbKichBans { get; set; } = new List<TbKichBan>();
}
