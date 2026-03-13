using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LMS.Ovncr.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<MigrationHistory> MigrationHistories { get; set; }

    public virtual DbSet<TbCauHoi> TbCauHois { get; set; }

    public virtual DbSet<TbComputer> TbComputers { get; set; }

    public virtual DbSet<TbDanhMucCauHoi> TbDanhMucCauHois { get; set; }

    public virtual DbSet<TbGiamSat> TbGiamSats { get; set; }

    public virtual DbSet<TbHocVienLopHoc> TbHocVienLopHocs { get; set; }

    public virtual DbSet<TbInstance> TbInstances { get; set; }

    public virtual DbSet<TbKetQua> TbKetQuas { get; set; }

    public virtual DbSet<TbKichBan> TbKichBans { get; set; }

    public virtual DbSet<TbKichBanNhom> TbKichBanNhoms { get; set; }

    public virtual DbSet<TbKichBanSoDoMang> TbKichBanSoDoMangs { get; set; }

    public virtual DbSet<TbKieuDanhGium> TbKieuDanhGia { get; set; }

    public virtual DbSet<TbKyThi> TbKyThis { get; set; }

    public virtual DbSet<TbLichPhongLab> TbLichPhongLabs { get; set; }

    public virtual DbSet<TbLopHoc> TbLopHocs { get; set; }

    public virtual DbSet<TbLopHocKichBan> TbLopHocKichBans { get; set; }

    public virtual DbSet<TbNhom> TbNhoms { get; set; }

    public virtual DbSet<TbProjectOpenStack> TbProjectOpenStacks { get; set; }

    public virtual DbSet<TbSetting> TbSettings { get; set; }

    public virtual DbSet<TbSoDoMang> TbSoDoMangs { get; set; }

    public virtual DbSet<TbSoDoMangMay> TbSoDoMangMays { get; set; }

    public virtual DbSet<TbSubmit> TbSubmits { get; set; }

    public virtual DbSet<TbTinhTrangCv> TbTinhTrangCvs { get; set; }

    public virtual DbSet<TbTraLoi> TbTraLois { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-RHKHTO4\\SQLEXPRESS01;Database=dbttm;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.AspNetRoles");

            entity.HasIndex(e => e.Name, "RoleNameIndex").IsUnique();

            entity.Property(e => e.Id).HasMaxLength(128);
            entity.Property(e => e.Name).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.AspNetUsers");

            entity.HasIndex(e => e.UserName, "UserNameIndex").IsUnique();

            entity.Property(e => e.Id).HasMaxLength(128);
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.FullName).HasMaxLength(200);
            entity.Property(e => e.LockoutEndDateUtc).HasColumnType("datetime");
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany()
                        .HasForeignKey("RoleId")
                        .HasConstraintName("FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId").HasName("PK_dbo.AspNetUserRoles");
                        j.ToTable("AspNetUserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_RoleId");
                        j.HasIndex(new[] { "UserId" }, "IX_UserId");
                        j.IndexerProperty<string>("UserId").HasMaxLength(128);
                        j.IndexerProperty<string>("RoleId").HasMaxLength(128);
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.AspNetUserClaims");

            entity.HasIndex(e => e.UserId, "IX_UserId");

            entity.Property(e => e.UserId).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId");
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey, e.UserId }).HasName("PK_dbo.AspNetUserLogins");

            entity.HasIndex(e => e.UserId, "IX_UserId");

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.ProviderKey).HasMaxLength(128);
            entity.Property(e => e.UserId).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId");
        });

        modelBuilder.Entity<MigrationHistory>(entity =>
        {
            entity.HasKey(e => new { e.MigrationId, e.ContextKey }).HasName("PK_dbo.__MigrationHistory");

            entity.ToTable("__MigrationHistory");

            entity.Property(e => e.MigrationId).HasMaxLength(150);
            entity.Property(e => e.ContextKey).HasMaxLength(300);
            entity.Property(e => e.ProductVersion).HasMaxLength(32);
        });

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
            entity.Property(e => e.NoiDung)
                .HasComment("Nội dung")
                .HasColumnType("ntext");
            entity.Property(e => e.TenKichBan)
                .HasMaxLength(1000)
                .HasComment("Tên kịch bản");
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
            entity.Property(e => e.Uuid)
                .HasMaxLength(200)
                .HasColumnName("UUID");
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
