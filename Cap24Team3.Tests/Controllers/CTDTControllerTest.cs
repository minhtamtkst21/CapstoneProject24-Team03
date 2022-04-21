using Cap24Team3.Areas.Faculty.Controllers;
using Cap24Team3.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Cap24Team3.Tests.Controllers
{
    [TestClass]
    public class CTDTControllerTest
    {
        //Unit test index nganh
        [TestMethod]
        public void ListNganhDTTest()
        {
            ChuongTrinhDaoTaoController controller = new ChuongTrinhDaoTaoController();
            var result = controller.ListNganhDT() as ViewResult;

            Assert.IsNotNull(result);

            var model = result.Model as List<NganhDaoTao>;
            Assert.IsNotNull(model);

            var db = new Cap24();
            Assert.AreEqual(db.NganhDaoTaos.Count(), model.Count());
        }
        //Unit test create nganh
        [TestMethod]
        public void CreateNganhViewTest()
        {
            var controller = new ChuongTrinhDaoTaoController();

            var result = controller.TaoMoiNganh("1","1") as ViewResult;

            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void CreateNganhTest()
        {
            var nganh = new NganhDaoTao
            {
                MaNganh = 7480000,
                Nganh = "Test"
            };
            var controller = new ChuongTrinhDaoTaoController();

            var result = controller.TaoMoiNganh("1","1");

            Assert.IsNotNull(result);
        }
        //Unit test delete nganh
        [TestMethod]
        public void DeleteNganhView()
        {
            var db = new Cap24();
            var nganh = db.NganhDaoTaos.AsNoTracking().First();

            var controller = new ChuongTrinhDaoTaoController();

            var result = controller.XoaNganhDT(nganh.ID);
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void DeleteNganh()
        {
            var db = new Cap24();
            var nganh = db.NganhDaoTaos.FirstOrDefault(n => n.Nganh == "Test").ID;

            var controller = new ChuongTrinhDaoTaoController();

            var result = controller.XacNhanXoaNganhDT(nganh);
            Assert.IsNotNull(result);
        }

        //Unit test index khoa
        [TestMethod]
        //public void ListKhoaDTTest()
        //{
        //    ChuongTrinhDaoTaoController controller = new ChuongTrinhDaoTaoController();
        //    var result = controller.ListKhoaDT() as ViewResult;

        //    Assert.IsNotNull(result);

        //    var model = result.Model as List<KhoaDaoTao>;
        //    Assert.IsNotNull(model);

        //    var db = new Cap24();
        //    Assert.AreEqual(db.KhoaDaoTaos.Count(), model.Count());
        //}
        //Unit test create khoa
        //[TestMethod]
        public void CreateKhoaViewTest()
        {
            var controller = new ChuongTrinhDaoTaoController();

            var result = controller.TaoMoiKhoa("1") as ViewResult;

            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void CreateKhoaTest()
        {
            var khoa = new KhoaDaoTao
            {
                Khoa = 1000
            };
            var controller = new ChuongTrinhDaoTaoController();

            var result = controller.TaoMoiKhoa("1");

            Assert.IsNotNull(result);
        }
        //Unit test delete khoa
        [TestMethod]
        public void DeleteKhoaView()
        {
            var db = new Cap24();
            var khoa = db.KhoaDaoTaos.AsNoTracking().First();

            var controller = new ChuongTrinhDaoTaoController();

            var result = controller.XoaKhoaDT(khoa.ID);
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void DeleteKhoa()
        {
            var db = new Cap24();
            var khoa = db.KhoaDaoTaos.FirstOrDefault(n => n.Khoa == 1000).ID;

            var controller = new ChuongTrinhDaoTaoController();

            var result = controller.XacNhanXoaKhoaDT(khoa);
            Assert.IsNotNull(result);
        }

        //Unit test index HK
        //[TestMethod]
        //public void ListHKDTTest()
        //{
        //    ChuongTrinhDaoTaoController controller = new ChuongTrinhDaoTaoController();
        //    var result = controller.ListHocKyDT() as ViewResult;

        //    Assert.IsNotNull(result);

        //    var model = result.Model as List<HocKyDaoTao>;
        //    Assert.IsNotNull(model);

        //    var db = new Cap24();
        //    Assert.AreEqual(db.HocKyDaoTaos.Count(), model.Count());
        //}
        //Unit test create HK
        [TestMethod]
        public void CreateHKViewTest()
        {
            var controller = new ChuongTrinhDaoTaoController();

            var result = controller.TaoMoiHocKy("1") as ViewResult;

            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void CreateHKTest()
        {
            var hocky = new HocKyDaoTao
            {
                HocKy = 999
            };
            var controller = new ChuongTrinhDaoTaoController();

            var result = controller.TaoMoiHocKy("1");

            Assert.IsNotNull(result);
        }
        //Unit test delete HK
        [TestMethod]
        public void DeleteHKView()
        {
            var db = new Cap24();
            var hocky = db.HocKyDaoTaos.AsNoTracking().First();

            var controller = new ChuongTrinhDaoTaoController();

            var result = controller.XoaHocKyDT(hocky.ID);
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void DeleteHK()
        {
            var db = new Cap24();
            var hocky = db.HocKyDaoTaos.FirstOrDefault(n => n.HocKy == 999).ID;

            var controller = new ChuongTrinhDaoTaoController();

            var result = controller.XacNhanXoaHocKyDT(hocky);
            Assert.IsNotNull(result);
        }

        //Unit test index ctdt
        [TestMethod]
        public void ListCTDTTest()
        {
            ChuongTrinhDaoTaoController controller = new ChuongTrinhDaoTaoController();
            var result = controller.ListCTDaoTao() as ViewResult;

            Assert.IsNotNull(result);

            var model = result.Model as List<ChuongTrinhDaoTao>;
            Assert.IsNotNull(model);

            var db = new Cap24();
            Assert.AreEqual(db.ChuongTrinhDaoTaos.Count(), model.Count());
        }
        //Unit test chi tiet ctdt
        [TestMethod]
        public void ChiTietCTDTTest()
        {
            ChuongTrinhDaoTaoController controller = new ChuongTrinhDaoTaoController();
            var db = new Cap24();
            var ctdt = db.ChuongTrinhDaoTaos.FirstOrDefault().ID;

            var result = controller.ChiTietCTDaoTao(ctdt) as ViewResult;
            var model = result.Model as List<HocPhanDaoTao>;

            Assert.IsNotNull(result);
        }
    }
}
