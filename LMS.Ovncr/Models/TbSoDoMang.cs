using System;
using System.Collections.Generic;

namespace LMS.Ovncr.Models;

public partial class TbSoDoMang
{
    public int Id { get; set; }

    public string? Ten { get; set; }

    public string? Uuid { get; set; }

    public string? Path { get; set; }

    public virtual ICollection<TbKichBanSoDoMang> TbKichBanSoDoMangs { get; set; } = new List<TbKichBanSoDoMang>();

    public virtual ICollection<TbSoDoMangMay> TbSoDoMangMays { get; set; } = new List<TbSoDoMangMay>();
}
