﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Cap24 : DbContext
    {
        public Cap24()
            : base("name=Cap24")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<ChinhSuaThongTin> ChinhSuaThongTins { get; set; }
        public virtual DbSet<ChuongTrinhDaoTao> ChuongTrinhDaoTaos { get; set; }
        public virtual DbSet<DiemHocPhan> DiemHocPhans { get; set; }
        public virtual DbSet<DotChinhSuaThongTin> DotChinhSuaThongTins { get; set; }
        public virtual DbSet<HocKyDaoTao> HocKyDaoTaos { get; set; }
        public virtual DbSet<HocPhanDaoTao> HocPhanDaoTaos { get; set; }
        public virtual DbSet<KhoaDaoTao> KhoaDaoTaos { get; set; }
        public virtual DbSet<KhoiKienThuc> KhoiKienThucs { get; set; }
        public virtual DbSet<LichSuUpLoad> LichSuUpLoads { get; set; }
        public virtual DbSet<LopQuanLy> LopQuanLies { get; set; }
        public virtual DbSet<NganhDaoTao> NganhDaoTaos { get; set; }
        public virtual DbSet<RangBuocHocPhan> RangBuocHocPhans { get; set; }
        public virtual DbSet<SinhVien> SinhViens { get; set; }
        public virtual DbSet<TinhTrang> TinhTrangs { get; set; }
    }
}
