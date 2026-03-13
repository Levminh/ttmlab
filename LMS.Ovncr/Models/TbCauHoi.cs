using System;
using System.Collections.Generic;

namespace LMS.Ovncr.Models;

public partial class TbCauHoi
{
    public int Id { get; set; }

    public int? ThuTu { get; set; }

    public string? CauHoi { get; set; }

    public string? Dap1 { get; set; }

    public string? Dap2 { get; set; }

    public string? Dap3 { get; set; }

    public string? Dap4 { get; set; }

    public int? DapAnDung { get; set; }

    public int? IdDanhMucCauHoi { get; set; }

    public virtual TbDanhMucCauHoi? IdDanhMucCauHoiNavigation { get; set; }

    public virtual ICollection<TbTraLoi> TbTraLois { get; set; } = new List<TbTraLoi>();
}
