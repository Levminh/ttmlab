namespace ttm3._0.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class dbttm : DbContext
    {
        public dbttm()
            : base("name=dbttm")
        {
        }
        public virtual DbSet<tbGiamSat> tbGiamSats { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<tbHocVienLopHoc> tbHocVienLopHocs { get; set; }
        public virtual DbSet<tbKetQua> tbKetQuas { get; set; }
        public virtual DbSet<tbKichBan> tbKichBans { get; set; }
        public virtual DbSet<tbLopHoc> tbLopHocs { get; set; }
        public virtual DbSet<tbLopHocKichBan> tbLopHocKichBans { get; set; }
        public virtual DbSet<tbNhom> tbNhoms { get; set; }
        public virtual DbSet<tbProjectOpenStack> tbProjectOpenStacks { get; set; }
        public virtual DbSet<tbComputer> tbComputers { get; set; }
        public virtual DbSet<tbKieuDanhGia> tbKieuDanhGias { get; set; }
        public virtual DbSet<tbKichBanNhom> tbKichBanNhoms { get; set; }
        public virtual DbSet<tbSoDoMang> tbSoDoMangs { get; set; }
        public virtual DbSet<tbSoDoMangMay> tbSoDoMangMays { get; set; }
        public virtual DbSet<tbKichBanSoDoMang> tbKichBanSoDoMangs { get; set; }
        public virtual DbSet<tbSetting> tbSettings { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetUser>()
               .HasMany(e => e.tbLichPhongLabs)
               .WithOptional(e => e.AspNetUser)
               .HasForeignKey(e => e.IdUser)
               .WillCascadeOnDelete();

            modelBuilder.Entity<tbProjectOpenStack>()
                .HasMany(e => e.tbLichPhongLabs)
                .WithOptional(e => e.tbProjectOpenStack)
                .WillCascadeOnDelete();

            modelBuilder.Entity<AspNetRole>()
                .HasMany(e => e.AspNetUserRoles)
                .WithRequired(e => e.AspNetRole)
                .HasForeignKey(e => e.RoleId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserClaims)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserLogins)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.tbHocVienLopHocs)
                .WithOptional(e => e.AspNetUser)
                .HasForeignKey(e => e.IdHocVien)
                .WillCascadeOnDelete();

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.tbKetQuas)
                .WithOptional(e => e.AspNetUser)
                .HasForeignKey(e => e.IdHocVien)
                .WillCascadeOnDelete();

            modelBuilder.Entity<tbKichBan>()
                .HasMany(e => e.tbKetQuas)
                .WithOptional(e => e.tbKichBan)
                .WillCascadeOnDelete();

            modelBuilder.Entity<tbKichBan>()
                .HasMany(e => e.tbLopHocKichBans)
                .WithOptional(e => e.tbKichBan)
                .WillCascadeOnDelete();
            modelBuilder.Entity<tbKichBan>()
                .HasMany(e => e.tbKichBanSoDoMangs)
                .WithOptional(e => e.tbKichBan)
                .WillCascadeOnDelete();

            modelBuilder.Entity<tbLopHoc>()
                .HasMany(e => e.tbHocVienLopHocs)
                .WithOptional(e => e.tbLopHoc)
                .WillCascadeOnDelete();

            modelBuilder.Entity<tbLopHoc>()
                .HasMany(e => e.tbKetQuas)
                .WithOptional(e => e.tbLopHoc)
                .WillCascadeOnDelete();

            modelBuilder.Entity<tbLopHoc>()
                .HasMany(e => e.tbLopHocKichBans)
                .WithOptional(e => e.tbLopHoc)
                .WillCascadeOnDelete();

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.tbLopHocs)
                .WithOptional(e => e.AspNetUser)
                .HasForeignKey(e => e.NguoiTao);

            modelBuilder.Entity<tbProjectOpenStack>()
               .HasMany(e => e.tbComputers)
               .WithOptional(e => e.tbProjectOpenStack)
               .WillCascadeOnDelete();

            modelBuilder.Entity<tbComputer>()
              .HasMany(e => e.tbHocVienLopHocs)
              .WithOptional(e => e.tbComputer)
              .HasForeignKey(e =>e.IdComputer);

            modelBuilder.Entity<tbKieuDanhGia>()
              .HasMany(e => e.tbKichBans)
              .WithOptional(e => e.tbKieuDanhGia)
              .HasForeignKey(e => e.IdKieuDanhGia);

            modelBuilder.Entity<tbProjectOpenStack>()
              .HasMany(e => e.tbLopHocs)
              .WithOptional(e => e.tbProjectOpenStack)
              .HasForeignKey(e => e.IdProjectOS);
            modelBuilder.Entity<tbSoDoMang>()
                .HasMany(e => e.tbSoDoMangMays)
                .WithOptional(e => e.tbSoDoMang)
                .HasForeignKey(e => e.IdSoDoMang)
                .WillCascadeOnDelete();
            modelBuilder.Entity<tbSoDoMang>()
               .HasMany(e => e.tbKichBanSoDoMangs)
               .WithOptional(e => e.tbSoDoMang)
               .HasForeignKey(e => e.IdSoDoMang)
               .WillCascadeOnDelete();
            modelBuilder.Entity<tbProjectOpenStack>()
                .HasMany(e => e.tbGiamSats)
                .WithOptional(e => e.tbProjectOpenStack)
                .HasForeignKey(e => e.IdProject)
                .WillCascadeOnDelete();
        }

        public System.Data.Entity.DbSet<ttm3._0.Models.tbLichPhongLab> tbLichPhongLabs { get; set; }
    }
}
