using System;
using System.Collections.Generic;

namespace LMS.Ovncr.Models;

public partial class TbProjectOpenStack
{
    public int IdProject { get; set; }

    public string? ProjectName { get; set; }

    public string? ProjectId { get; set; }

    public virtual ICollection<TbComputer> TbComputers { get; set; } = new List<TbComputer>();

    public virtual ICollection<TbGiamSat> TbGiamSats { get; set; } = new List<TbGiamSat>();

    public virtual ICollection<TbInstance> TbInstances { get; set; } = new List<TbInstance>();

    public virtual ICollection<TbLichPhongLab> TbLichPhongLabs { get; set; } = new List<TbLichPhongLab>();

    public virtual ICollection<TbLopHoc> TbLopHocs { get; set; } = new List<TbLopHoc>();
}
