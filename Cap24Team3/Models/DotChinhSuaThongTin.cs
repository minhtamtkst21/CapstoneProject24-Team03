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
    
    public partial class DotChinhSuaThongTin
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DotChinhSuaThongTin()
        {
            this.ChinhSuaThongTins = new HashSet<ChinhSuaThongTin>();
        }
    
        public int ID { get; set; }
        public string DotChinhSua { get; set; }
        public System.DateTime NgayBatDau { get; set; }
        public System.DateTime NgayKetThuc { get; set; }
        public string NguoiTao { get; set; }
        public int Lop { get; set; }
        public bool TinhTrang { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChinhSuaThongTin> ChinhSuaThongTins { get; set; }
        public virtual LopQuanLy LopQuanLy { get; set; }
    }
}
