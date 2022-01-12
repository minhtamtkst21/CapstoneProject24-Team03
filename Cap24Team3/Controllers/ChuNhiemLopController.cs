using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Cap24Team3.Models;

namespace Cap24Team3.Controllers
{
    public class ChuNhiemLopController : Controller
    {
        private Cap24 db = new Cap24();

        // GET: ChuNhiemLop
        public ActionResult Index()
        {
            var mailGV = User.Identity.Name;
            var lopQuanLies = db.LopQuanLies.Include(l => l.KhoaDaoTao).Include(l => l.NganhDaoTao).Where(l => l.ChuNhiem == mailGV);
            return View(lopQuanLies.ToList());
        }
        public ActionResult DanhSachSV(int idLop)
        {
            //var lop = db.LopQuanLies.Find(idLop);
            //var danhSachSV = db.SinhViens.Where(s => s.LopQuanLy.TenLop == lop.ToString()).ToList();
            var danhSachSV = db.SinhViens.Where(s => s.LopQuanLy.ID == idLop).ToList();
            return View(danhSachSV);
        }
        public ActionResult XemChiTietSV(int id)
        {
            Session["sinhvien"] = db.SinhViens.Find(id);
            Session["tinhtrang"] = db.SinhViens.Find(id).TinhTrang.TenTinhTrang;
            return Redirect(Request.UrlReferrer.ToString());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
