using System;
using System.Collections.Generic;

namespace LMS.Ovncr.Models;

public partial class AspNetUser
{
    public string Id { get; set; } = null!;

    public string? Email { get; set; }

    public bool EmailConfirmed { get; set; }

    public string? PasswordHash { get; set; }

    public string? SecurityStamp { get; set; }

    public string? PhoneNumber { get; set; }

    public bool PhoneNumberConfirmed { get; set; }

    public bool TwoFactorEnabled { get; set; }

    public DateTime? LockoutEndDateUtc { get; set; }

    public bool LockoutEnabled { get; set; }

    public int AccessFailedCount { get; set; }

    public string UserName { get; set; } = null!;

    public string? FullName { get; set; }

    public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; } = new List<AspNetUserClaim>();

    public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; } = new List<AspNetUserLogin>();

    public virtual ICollection<TbHocVienLopHoc> TbHocVienLopHocs { get; set; } = new List<TbHocVienLopHoc>();

    public virtual ICollection<TbKetQua> TbKetQuas { get; set; } = new List<TbKetQua>();

    public virtual ICollection<TbKyThi> TbKyThis { get; set; } = new List<TbKyThi>();

    public virtual ICollection<TbLichPhongLab> TbLichPhongLabs { get; set; } = new List<TbLichPhongLab>();

    public virtual ICollection<TbLopHoc> TbLopHocs { get; set; } = new List<TbLopHoc>();

    public virtual ICollection<AspNetRole> Roles { get; set; } = new List<AspNetRole>();
}
