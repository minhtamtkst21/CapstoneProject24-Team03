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
    
    public partial class LopQuanLy
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LopQuanLy()
        {
            this.DotChinhSuaThongTins = new HashSet<DotChinhSuaThongTin>();
            this.SinhViens = new HashSet<SinhVien>();
        }
    
        public int ID { get; set; }
        public string TenLop { get; set; }
        public string ChuNhiem { get; set; }
        public Nullable<int> ID_Nganh { get; set; }
        public Nullable<int> ID_Khoa { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DotChinhSuaThongTin> DotChinhSuaThongTins { get; set; }
        public virtual KhoaDaoTao KhoaDaoTao { get; set; }
        public virtual NganhDaoTao NganhDaoTao { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SinhVien> SinhViens { get; set; }
    }
}
