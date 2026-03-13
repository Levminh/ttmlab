using System;
using System.Collections.Generic;

namespace LMS.Ovncr.Models;

public partial class TbTraLoi
{
    public int Id { get; set; }

    public int? ThuTu { get; set; }

    public int? IdKyThi { get; set; }

    public int? IdCauHoi { get; set; }

    public int? DapAnTraLoi { get; set; }

    public DateTime? ThoiGian { get; set; }

    public int? Diem { get; set; }

    public virtual TbCauHoi? IdCauHoiNavigation { get; set; }

    public virtual TbKyThi? IdKyThiNavigation { get; set; }
}
