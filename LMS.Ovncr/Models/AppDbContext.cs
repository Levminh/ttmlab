using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace LMS.Ovncr.Models;

/// <summary>
/// DbContext chính của ứng dụng. Kế thừa IdentityDbContext để tích hợp
/// ASP.NET Core Identity, quản lý xác thực và phân quyền người dùng.
/// Generic params: User=AspNetUser, Role=AspNetRole, Key=string
/// </summary>
public partial class AppDbContext : IdentityDbContext<AspNetUser, AspNetRole, string,
    IdentityUserClaim<string>, IdentityUserRole<string>, IdentityUserLogin<string>,
    IdentityRoleClaim<string>, IdentityUserToken<string>>
{
    public AppDbContext()
    {
    }

    /// <summary>
    /// Constructor nhận options từ DI container (Program.cs).
    /// Connection string được lấy từ appsettings.json thay vì hardcode.
    /// </summary>
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // ========== DbSet cho các bảng nghiệp vụ đặc thù của hệ thống ==========
    // (Các bảng Identity như AspNetUsers, AspNetRoles... được quản lý bởi IdentityDbContext)

    /// <summary>Ngân hàng câu hỏi trắc nghiệm</summary>
    public virtual DbSet<TbCauHoi> TbCauHois { get; set; }

    /// <summary>Máy tính (VM) trong môi trường thực hành</summary>
    public virtual DbSet<TbComputer> TbComputers { get; set; }

    /// <summary>Danh mục câu hỏi (phân loại câu hỏi)</summary>
    public virtual DbSet<TbDanhMucCauHoi> TbDanhMucCauHois { get; set; }

    /// <summary>Giám sát (monitoring) trong môi trường thực hành</summary>
    public virtual DbSet<TbGiamSat> TbGiamSats { get; set; }

    /// <summary>Quan hệ học viên - lớp học</summary>
    public virtual DbSet<TbHocVienLopHoc> TbHocVienLopHocs { get; set; }

    /// <summary>Instance VM trong OpenStack</summary>
    public virtual DbSet<TbInstance> TbInstances { get; set; }

    /// <summary>Kết quả thực hành của học viên</summary>
    public virtual DbSet<TbKetQua> TbKetQuas { get; set; }

    /// <summary>Kịch bản diễn tập an toàn thông tin</summary>
    public virtual DbSet<TbKichBan> TbKichBans { get; set; }

    /// <summary>Quan hệ kịch bản - nhóm</summary>
    public virtual DbSet<TbKichBanNhom> TbKichBanNhoms { get; set; }

    /// <summary>Quan hệ kịch bản - sơ đồ mạng</summary>
    public virtual DbSet<TbKichBanSoDoMang> TbKichBanSoDoMangs { get; set; }

    /// <summary>Kiểu đánh giá kết quả diễn tập</summary>
    public virtual DbSet<TbKieuDanhGium> TbKieuDanhGia { get; set; }

    /// <summary>Kỳ thi (bài kiểm tra trắc nghiệm)</summary>
    public virtual DbSet<TbKyThi> TbKyThis { get; set; }

    /// <summary>Lịch sử dụng phòng lab thực hành</summary>
    public virtual DbSet<TbLichPhongLab> TbLichPhongLabs { get; set; }

    /// <summary>Lớp học tổ chức cho học viên diễn tập</summary>
    public virtual DbSet<TbLopHoc> TbLopHocs { get; set; }

    /// <summary>Quan hệ lớp học - kịch bản</summary>
    public virtual DbSet<TbLopHocKichBan> TbLopHocKichBans { get; set; }

    /// <summary>Nhóm học viên trong lớp</summary>
    public virtual DbSet<TbNhom> TbNhoms { get; set; }

    /// <summary>Project OpenStack (môi trường ảo hóa)</summary>
    public virtual DbSet<TbProjectOpenStack> TbProjectOpenStacks { get; set; }

    /// <summary>Cài đặt / cấu hình hệ thống</summary>
    public virtual DbSet<TbSetting> TbSettings { get; set; }

    /// <summary>Sơ đồ mạng của môi trường thực hành</summary>
    public virtual DbSet<TbSoDoMang> TbSoDoMangs { get; set; }

    /// <summary>Máy tính trong sơ đồ mạng</summary>
    public virtual DbSet<TbSoDoMangMay> TbSoDoMangMays { get; set; }

    /// <summary>Bài nộp của học viên</summary>
    public virtual DbSet<TbSubmit> TbSubmits { get; set; }

    /// <summary>Tình trạng công việc (cv)</summary>
    public virtual DbSet<TbTinhTrangCv> TbTinhTrangCvs { get; set; }

    /// <summary>Câu trả lời của học viên trong kỳ thi</summary>
    public virtual DbSet<TbTraLoi> TbTraLois { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // BẮT BUỘC gọi base để Identity cấu hình các bảng AspNetUsers, AspNetRoles...
        base.OnModelCreating(modelBuilder);

        // ========== Cấu hình bảng nghiệp vụ (giữ nguyên từ scaffold) ==========

        modelBuilder.Entity<TbCauHoi>(entity =>
        {
            entity.ToTable("tbCauHoi");
            entity.Property(e => e.CauHoi).HasColumnType("ntext");
            entity.Property(e => e.Dap1).HasMaxLength(500);
            entity.Property(e => e.Dap2).HasMaxLength(500);
            entity.Property(e => e.Dap3).HasMaxLength(500);
            entity.Property(e => e.Dap4).HasMaxLength(500);
            entity.HasOne(d => d.IdDanhMucCauHoiNavigation).WithMany(p => p.TbCauHois)
                .HasForeignKey(d => d.IdDanhMucCauHoi)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_tbCauHoi_tbDanhMucCauHoi");
        });

        modelBuilder.Entity<TbComputer>(entity =>
        {
            entity.ToTable("tbComputer");
            entity.Property(e => e.IdOpenStack).HasMaxLength(50);
            entity.Property(e => e.MoTa).HasColumnType("ntext");
            entity.Property(e => e.Name).HasMaxLength(500);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.VncUri).HasMaxLength(500);
            entity.HasOne(d => d.IdProjectNavigation).WithMany(p => p.TbComputers)
                .HasForeignKey(d => d.IdProject)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tbComputer_tbProjectOpenStack");
        });

        modelBuilder.Entity<TbDanhMucCauHoi>(entity =>
        {
            entity.ToTable("tbDanhMucCauHoi");
            entity.Property(e => e.DanhMucCauHoi).HasMaxLength(50);
        });

        modelBuilder.Entity<TbGiamSat>(entity =>
        {
            entity.ToTable("tbGiamSat");
            entity.Property(e => e.Link).HasMaxLength(500);
            entity.Property(e => e.Ten).HasMaxLength(500);
            entity.HasOne(d => d.IdProjectNavigation).WithMany(p => p.TbGiamSats)
                .HasForeignKey(d => d.IdProject)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tbGiamSat_tbProjectOpenStack");
        });

        modelBuilder.Entity<TbHocVienLopHoc>(entity =>
        {
            entity.HasKey(e => e.IdHocVienLopHoc);
            entity.ToTable("tbHocVienLopHoc");
            entity.Property(e => e.GhiChu).HasColumnType("ntext");
            entity.Property(e => e.IdHocVien).HasMaxLength(128);
            entity.Property(e => e.IdInstance).HasMaxLength(200);
            entity.HasOne(d => d.IdComputerNavigation).WithMany(p => p.TbHocVienLopHocs)
                .HasForeignKey(d => d.IdComputer)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_tbHocVienLopHoc_tbComputer");
            entity.HasOne(d => d.IdHocVienNavigation).WithMany(p => p.TbHocVienLopHocs)
                .HasForeignKey(d => d.IdHocVien)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tbHocVienLopHoc_AspNetUsers");
            entity.HasOne(d => d.IdLopHocNavigation).WithMany(p => p.TbHocVienLopHocs)
                .HasForeignKey(d => d.IdLopHoc)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tbHocVienLopHoc_tbLopHoc");
        });

        modelBuilder.Entity<TbInstance>(entity =>
        {
            entity.HasKey(e => e.IdInstance);
            entity.ToTable("tbInstance");
            entity.Property(e => e.InstanceId).HasMaxLength(500);
            entity.Property(e => e.InstanceName).HasMaxLength(500);
            entity.Property(e => e.Ip).HasMaxLength(50);
            entity.HasOne(d => d.IdProjectNavigation).WithMany(p => p.TbInstances)
                .HasForeignKey(d => d.IdProject)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tbInstance_tbProjectOpenStack");
        });

        modelBuilder.Entity<TbKetQua>(entity =>
        {
            entity.HasKey(e => e.IdKetQua);
            entity.ToTable("tbKetQua");
            entity.Property(e => e.DapAn).HasMaxLength(1000);
            entity.Property(e => e.GhiChu).HasMaxLength(1000);
            entity.Property(e => e.IdHocVien).HasMaxLength(128);
            entity.Property(e => e.UrlFileDapAn).HasMaxLength(1000);
            entity.HasOne(d => d.IdHocVienNavigation).WithMany(p => p.TbKetQuas)
                .HasForeignKey(d => d.IdHocVien)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tbKetQua_AspNetUsers");
            entity.HasOne(d => d.IdKichBanNavigation).WithMany(p => p.TbKetQuas)
                .HasForeignKey(d => d.IdKichBan)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tbKetQua_tbKichBan");
            entity.HasOne(d => d.IdLopHocNavigation).WithMany(p => p.TbKetQuas)
                .HasForeignKey(d => d.IdLopHoc)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tbKetQua_tbLopHoc");
        });

        modelBuilder.Entity<TbKichBan>(entity =>
        {
            entity.HasKey(e => e.IdKichBan);
            entity.ToTable("tbKichBan");
            entity.Property(e => e.DapAn).HasMaxLength(1000);
            entity.Property(e => e.HuongDanThucHanh).HasColumnType("ntext");
            entity.Property(e => e.MaKichBan).HasMaxLength(50);
            entity.Property(e => e.MucTieu).HasColumnType("ntext");
            entity.Property(e => e.NoiDung).HasComment("Nội dung").HasColumnType("ntext");
            entity.Property(e => e.TenKichBan).HasMaxLength(1000).HasComment("Tên kịch bản");
            entity.Property(e => e.YeuCau).HasColumnType("ntext");
            entity.HasOne(d => d.IdKieuDanhGiaNavigation).WithMany(p => p.TbKichBans)
                .HasForeignKey(d => d.IdKieuDanhGia)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_tbKichBan_tbKieuDanhGia");
        });

        modelBuilder.Entity<TbKichBanNhom>(entity =>
        {
            entity.HasKey(e => e.IdKichBanNhom);
            entity.ToTable("tbKichBanNhom");
            entity.HasOne(d => d.IdKichBanNavigation).WithMany(p => p.TbKichBanNhoms)
                .HasForeignKey(d => d.IdKichBan)
                .HasConstraintName("FK_tbKichBanNhom_tbKichBan");
            entity.HasOne(d => d.IdNhomNavigation).WithMany(p => p.TbKichBanNhoms)
                .HasForeignKey(d => d.IdNhom)
                .HasConstraintName("FK_tbKichBanNhom_tbNhom");
        });

        modelBuilder.Entity<TbKichBanSoDoMang>(entity =>
        {
            entity.ToTable("tbKichBanSoDoMang");
            entity.HasOne(d => d.IdKichBanNavigation).WithMany(p => p.TbKichBanSoDoMangs)
                .HasForeignKey(d => d.IdKichBan)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tbKichBanSoDoMang_tbKichBan");
            entity.HasOne(d => d.IdSoDoMangNavigation).WithMany(p => p.TbKichBanSoDoMangs)
                .HasForeignKey(d => d.IdSoDoMang)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tbKichBanSoDoMang_tbSoDoMang");
        });

        modelBuilder.Entity<TbKieuDanhGium>(entity =>
        {
            entity.HasKey(e => e.IdKieuDanhGia);
            entity.ToTable("tbKieuDanhGia");
            entity.Property(e => e.KieuDanhGia).HasMaxLength(500);
        });

        modelBuilder.Entity<TbKyThi>(entity =>
        {
            entity.ToTable("tbKyThi");
            entity.Property(e => e.DanhMucCauHoi).HasMaxLength(50);
            entity.Property(e => e.NgayTao).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianBatDat).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianKetThuc).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasMaxLength(128);
            entity.HasOne(d => d.User).WithMany(p => p.TbKyThis)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tbKyThi_AspNetUsers");
        });

        modelBuilder.Entity<TbLichPhongLab>(entity =>
        {
            entity.ToTable("tbLichPhongLab");
            entity.Property(e => e.DenNgay).HasColumnType("datetime");
            entity.Property(e => e.IdUser).HasMaxLength(128);
            entity.Property(e => e.TuNgay).HasColumnType("datetime");
            entity.HasOne(d => d.IdProjectNavigation).WithMany(p => p.TbLichPhongLabs)
                .HasForeignKey(d => d.IdProject)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tbLichPhongLab_tbProjectOpenStack");
            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.TbLichPhongLabs)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tbLichPhongLab_AspNetUsers");
        });

        modelBuilder.Entity<TbLopHoc>(entity =>
        {
            entity.HasKey(e => e.IdLopHoc);
            entity.ToTable("tbLopHoc");
            entity.Property(e => e.IdProjectOs).HasColumnName("IdProjectOS");
            entity.Property(e => e.LopHoc).HasMaxLength(500);
            entity.Property(e => e.NguoiTao).HasMaxLength(128);
            entity.Property(e => e.ThongTin).HasMaxLength(1000);
            entity.HasOne(d => d.IdProjectOsNavigation).WithMany(p => p.TbLopHocs)
                .HasForeignKey(d => d.IdProjectOs)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_tbLopHoc_tbProjectOpenStack");
            entity.HasOne(d => d.NguoiTaoNavigation).WithMany(p => p.TbLopHocs)
                .HasForeignKey(d => d.NguoiTao)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_tbLopHoc_AspNetUsers");
        });

        modelBuilder.Entity<TbLopHocKichBan>(entity =>
        {
            entity.HasKey(e => e.IdLopHocKichBan);
            entity.ToTable("tbLopHocKichBan");
            entity.Property(e => e.GhiChu).HasMaxLength(500);
            entity.HasOne(d => d.IdKichBanNavigation).WithMany(p => p.TbLopHocKichBans)
                .HasForeignKey(d => d.IdKichBan)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tbLopHocKichBan_tbKichBan");
            entity.HasOne(d => d.IdLopHocNavigation).WithMany(p => p.TbLopHocKichBans)
                .HasForeignKey(d => d.IdLopHoc)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tbLopHocKichBan_tbLopHoc");
        });

        modelBuilder.Entity<TbNhom>(entity =>
        {
            entity.HasKey(e => e.IdNhom);
            entity.ToTable("tbNhom");
            entity.Property(e => e.Nhom).HasMaxLength(50);
        });

        modelBuilder.Entity<TbProjectOpenStack>(entity =>
        {
            entity.HasKey(e => e.IdProject);
            entity.ToTable("tbProjectOpenStack");
            entity.Property(e => e.ProjectId).HasMaxLength(500);
            entity.Property(e => e.ProjectName).HasMaxLength(500);
        });

        modelBuilder.Entity<TbSetting>(entity =>
        {
            entity.ToTable("tbSettings");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Value).HasMaxLength(4000);
        });

        modelBuilder.Entity<TbSoDoMang>(entity =>
        {
            entity.ToTable("tbSoDoMang");
            entity.Property(e => e.Path).HasMaxLength(500);
            entity.Property(e => e.Ten).HasMaxLength(200);
            entity.Property(e => e.Uuid).HasMaxLength(200).HasColumnName("UUID");
        });

        modelBuilder.Entity<TbSoDoMangMay>(entity =>
        {
            entity.ToTable("tbSoDoMangMay");
            entity.Property(e => e.GhiChu).HasMaxLength(500);
            entity.Property(e => e.Kieu).HasMaxLength(100);
            entity.Property(e => e.MaMay).HasMaxLength(200);
            entity.Property(e => e.TenMay).HasMaxLength(200);
            entity.HasOne(d => d.IdSoDoMangNavigation).WithMany(p => p.TbSoDoMangMays)
                .HasForeignKey(d => d.IdSoDoMang)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tbSoDoMangMay_tbSoDoMang");
        });

        modelBuilder.Entity<TbSubmit>(entity =>
        {
            entity.ToTable("tbSubmit");
            entity.Property(e => e.FileKetQua).HasMaxLength(200);
            entity.Property(e => e.IdHocVien).HasMaxLength(128);
            entity.Property(e => e.KetQua).HasMaxLength(1000);
            entity.Property(e => e.NgayGio).HasColumnType("datetime");
        });

        modelBuilder.Entity<TbTinhTrangCv>(entity =>
        {
            entity.ToTable("tbTinhTrangCV");
            entity.Property(e => e.TinhTrang).HasMaxLength(50);
        });

        modelBuilder.Entity<TbTraLoi>(entity =>
        {
            entity.ToTable("tbTraLoi");
            entity.Property(e => e.ThoiGian).HasColumnType("datetime");
            entity.HasOne(d => d.IdCauHoiNavigation).WithMany(p => p.TbTraLois)
                .HasForeignKey(d => d.IdCauHoi)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tbTraLoi_tbCauHoi");
            entity.HasOne(d => d.IdKyThiNavigation).WithMany(p => p.TbTraLois)
                .HasForeignKey(d => d.IdKyThi)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tbTraLoi_tbKyThi");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
