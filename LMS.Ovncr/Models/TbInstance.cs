using System;
using System.Collections.Generic;

namespace LMS.Ovncr.Models;

public partial class TbInstance
{
    public int IdInstance { get; set; }

    public string? InstanceName { get; set; }

    public string? InstanceId { get; set; }

    public int? IdProject { get; set; }

    public string? Ip { get; set; }

    public virtual TbProjectOpenStack? IdProjectNavigation { get; set; }
}
