using System;
using System.Collections.Generic;

namespace LMS.Ovncr.Models;

public partial class TbSoDoMangMay
{
    public int Id { get; set; }

    public int? IdSoDoMang { get; set; }

    public string? MaMay { get; set; }

    public string? TenMay { get; set; }

    public string? Kieu { get; set; }

    public string? GhiChu { get; set; }

    public virtual TbSoDoMang? IdSoDoMangNavigation { get; set; }
}
