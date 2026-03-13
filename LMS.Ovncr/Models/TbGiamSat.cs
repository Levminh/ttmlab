using System;
using System.Collections.Generic;

namespace LMS.Ovncr.Models;

public partial class TbGiamSat
{
    public int Id { get; set; }

    public string? Ten { get; set; }

    public string? Link { get; set; }

    public int? IdProject { get; set; }

    public virtual TbProjectOpenStack? IdProjectNavigation { get; set; }
}
