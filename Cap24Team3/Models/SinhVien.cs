//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Cap24Team3.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SinhVien
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SinhVien()
        {
            this.ChinhSuaThongTins = new HashSet<ChinhSuaThongTin>();
            this.DiemHocPhans = new HashSet<DiemHocPhan>();
        }
    
        public int ID { get; set; }
        public string MSSV { get; set; }
        public string Ho { get; set; }
        public string Ten { get; set; }
        public string GioiTinh { get; set; }
        public string NgaySinh { get; set; }
        public Nullable<int> ID_TinhTrang { get; set; }
        public Nullable<int> ID_Lop { get; set; }
        public string Email_1 { get; set; }
        public string Email_2 { get; set; }
        public string DTDD { get; set; }
        public string DTCha { get; set; }
        public string DTMe { get; set; }
        public string DiaChi { get; set; }
        public Nullable<int> ID_Khoa { get; set; }
        public Nullable<int> ID_Nganh { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChinhSuaThongTin> ChinhSuaThongTins { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DiemHocPhan> DiemHocPhans { get; set; }
        public virtual KhoaDaoTao KhoaDaoTao { get; set; }
        public virtual LopQuanLy LopQuanLy { get; set; }
        public virtual NganhDaoTao NganhDaoTao { get; set; }
        public virtual TinhTrang TinhTrang { get; set; }
    }
}
