using System;
using System.Collections.Generic;

namespace LMS.Ovncr.Models;

public partial class TbKichBan
{
    public int IdKichBan { get; set; }

    public string? MaKichBan { get; set; }

    /// <summary>
    /// Tên kịch bản
    /// </summary>
    public string? TenKichBan { get; set; }

    public string? MucTieu { get; set; }

    /// <summary>
    /// Nội dung
    /// </summary>
    public string? NoiDung { get; set; }

    public string? YeuCau { get; set; }

    public int? IdKieuDanhGia { get; set; }

    public string? HuongDanThucHanh { get; set; }

    public string? DapAn { get; set; }

    public virtual TbKieuDanhGium? IdKieuDanhGiaNavigation { get; set; }

    public virtual ICollection<TbKetQua> TbKetQuas { get; set; } = new List<TbKetQua>();

    public virtual ICollection<TbKichBanNhom> TbKichBanNhoms { get; set; } = new List<TbKichBanNhom>();

    public virtual ICollection<TbKichBanSoDoMang> TbKichBanSoDoMangs { get; set; } = new List<TbKichBanSoDoMang>();

    public virtual ICollection<TbLopHocKichBan> TbLopHocKichBans { get; set; } = new List<TbLopHocKichBan>();
}
