using System;
using System.Collections.Generic;

namespace LMS.Ovncr.Models;

public partial class TbSubmit
{
    public int Id { get; set; }

    public string? IdHocVien { get; set; }

    public int? IdLopHoc { get; set; }

    public int? IdKichBan { get; set; }

    public string? KetQua { get; set; }

    public string? FileKetQua { get; set; }

    public DateTime? NgayGio { get; set; }

    public int? Lan { get; set; }
}
