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
    
    public partial class DiemHocPhan
    {
        public int ID { get; set; }
        public string MSSV { get; set; }
        public string HocPhan { get; set; }
        public string TenHocPhan { get; set; }
        public Nullable<int> HocKyKeHoach { get; set; }
        public Nullable<int> SoTinChi { get; set; }
        public string Diem10 { get; set; }
        public string Diem4 { get; set; }
        public string DiemChu { get; set; }
        public Nullable<bool> QuaMon { get; set; }
        public Nullable<int> LichSu { get; set; }
        public Nullable<int> HocKyDangKy { get; set; }
    
        public virtual HocKyDaoTao HocKyDaoTao { get; set; }
        public virtual HocKyDaoTao HocKyDaoTao1 { get; set; }
        public virtual LichSuUpLoad LichSuUpLoad { get; set; }
    }
}
