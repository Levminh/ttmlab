using System;
using System.Collections.Generic;

namespace LMS.Ovncr.Models;

public partial class TbNhom
{
    public int IdNhom { get; set; }

    public string? Nhom { get; set; }

    public virtual ICollection<TbKichBanNhom> TbKichBanNhoms { get; set; } = new List<TbKichBanNhom>();
}
