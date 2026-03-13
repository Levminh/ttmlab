using System;
using System.Collections.Generic;

namespace LMS.Ovncr.Models;

public partial class TbDanhMucCauHoi
{
    public int Id { get; set; }

    public string? DanhMucCauHoi { get; set; }

    public virtual ICollection<TbCauHoi> TbCauHois { get; set; } = new List<TbCauHoi>();
}
