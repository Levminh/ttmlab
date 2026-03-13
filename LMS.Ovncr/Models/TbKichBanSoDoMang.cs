using System;
using System.Collections.Generic;

namespace LMS.Ovncr.Models;

public partial class TbKichBanSoDoMang
{
    public int Id { get; set; }

    public int? IdKichBan { get; set; }

    public int? IdSoDoMang { get; set; }

    public virtual TbKichBan? IdKichBanNavigation { get; set; }

    public virtual TbSoDoMang? IdSoDoMangNavigation { get; set; }
}
