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
    
    public partial class HocKyDaoTao
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HocKyDaoTao()
        {
            this.ChuongTrinhDaoTaos = new HashSet<ChuongTrinhDaoTao>();
            this.DiemHocPhans = new HashSet<DiemHocPhan>();
            this.DiemHocPhans1 = new HashSet<DiemHocPhan>();
            this.DiemHocPhans2 = new HashSet<DiemHocPhan>();
        }
    
        public int ID { get; set; }
        public int STT { get; set; }
        public int HocKy { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChuongTrinhDaoTao> ChuongTrinhDaoTaos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DiemHocPhan> DiemHocPhans { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DiemHocPhan> DiemHocPhans1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DiemHocPhan> DiemHocPhans2 { get; set; }
    }
}
