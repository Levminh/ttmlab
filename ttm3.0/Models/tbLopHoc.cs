namespace ttm3._0.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tbLopHoc")]
    public partial class tbLopHoc
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbLopHoc()
        {
            tbHocVienLopHocs = new HashSet<tbHocVienLopHoc>();
            tbKetQuas = new HashSet<tbKetQua>();
            tbLopHocKichBans = new HashSet<tbLopHocKichBan>();
        }

        [Key]
        public int IdLopHoc { get; set; }

        [Display(Name = "Lớp học")]
        [StringLength(500)]
        public string LopHoc { get; set; }

        [Display(Name = "Thông tin")]
        [StringLength(1000)]
        public string ThongTin { get; set; }

        [Display(Name = "Số sinh viên")]
        [NotMapped]
        public int? SoSinhVien { get { return tbHocVienLopHocs.Count; } }

        [Display(Name = "Số kịch bản")]
        [NotMapped]
        public int? SoKichBan { get { return tbLopHocKichBans.Count; } }

        [Display(Name = "Người tạo")]
        [StringLength(128)]
        public string NguoiTao { get; set; }

        [Display(Name = "Openstack Project")]
        public int? IdProjectOS { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbHocVienLopHoc> tbHocVienLopHocs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbKetQua> tbKetQuas { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbLopHocKichBan> tbLopHocKichBans { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }
        public virtual tbProjectOpenStack tbProjectOpenStack { get; set; }

    }
}
