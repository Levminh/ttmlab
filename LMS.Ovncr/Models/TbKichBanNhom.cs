using System;
using System.Collections.Generic;

namespace LMS.Ovncr.Models;

public partial class TbKichBanNhom
{
    public int IdKichBanNhom { get; set; }

    public int IdNhom { get; set; }

    public int IdKichBan { get; set; }

    public virtual TbKichBan IdKichBanNavigation { get; set; } = null!;

    public virtual TbNhom IdNhomNavigation { get; set; } = null!;
}
