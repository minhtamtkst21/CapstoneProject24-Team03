using Cap24Team3.Areas.Faculty.Controllers;
using Cap24Team3.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Cap24Team3.Tests.Controllers
{
    [TestClass]
    public class DanhSachSVTest
    {
        //Unit test danh sach sinh vien theo khoa
        [TestMethod]
        public void ListKhoaTest()
        {
            DanhSachSinhVienController controller = new DanhSachSinhVienController();
            
            var result = controller.KhoaSinhVien() as ViewResult;
            Assert.IsNotNull(result);
            var model = result.Model as List<KhoaDaoTao>;
            Assert.IsNotNull(model);

            var db = new Cap24();
            Assert.AreEqual(db.SinhViens.GroupBy(s => s.KhoaDaoTao).Count(), model.Count());
        }
        //Unit test danh sach sinh vien theo nganh
        [TestMethod]
        public void ListNganhTest()
        {
            DanhSachSinhVienController controller = new DanhSachSinhVienController();
            var db = new Cap24();
            var khoa = db.KhoaDaoTaos.FirstOrDefault().ID;
            var result = controller.NganhSinhVien(khoa) as ViewResult;
            Assert.IsNotNull(result);
            var model = result.Model as List<NganhDaoTao>;
            Assert.IsNotNull(model);
            var lop = db.LopQuanLies.Where(s => s.KhoaDaoTao.ID == khoa).ToList();
            Assert.AreEqual(lop.Count(), model.Count());
        }
        ////Unit test danh sach lop sinh vien theo khoa nganh
        //[TestMethod]
        //public void ListLopTest()
        //{
        //    DanhSachSinhVienController controller = new DanhSachSinhVienController();
        //    var db = new Cap24();

        //}
        //Unit test danh sach sinh vien theo lop
        [TestMethod]
        public void ListSVTest()
        {
            DanhSachSinhVienController controller = new DanhSachSinhVienController();
            var db = new Cap24();
            var lop = db.LopQuanLies.FirstOrDefault().ID;
            var result = controller.ListSinhVien(lop) as ViewResult;
            Assert.IsNotNull(result);
            var model = result.Model as List<SinhVien>;
            Assert.IsNotNull(model);
            var sv = db.SinhViens.Where(s => s.LopQuanLy.ID == lop);

            Assert.AreEqual(sv.Count(), model.Count());
        }
        //Unit test tim sinh vien
        [TestMethod]
        public void TimSVTest()
        {
            DanhSachSinhVienController controller = new DanhSachSinhVienController();
            var db = new Cap24();
            var sv = db.SinhViens.ToList();
            var keyword = sv.First().MSSV.Split().First();
            sv = sv.Where(s => s.MSSV.ToLower().Contains(keyword.ToLower())).ToList();
            var result = controller.TimKiemSinhVien(keyword) as ViewResult;
            Assert.IsNotNull(result);
            var model = result.Model as List<SinhVien>;
            Assert.IsNotNull(model);
            Assert.AreEqual(sv.Count(), model.Count());
        }
    }
}
