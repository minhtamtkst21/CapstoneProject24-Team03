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
        public ActionResult DanhSachLop()
        {
            var mailGV = User.Identity.Name;
            var lopQuanLies = db.LopQuanLies.Include(l => l.KhoaDaoTao).Include(l => l.NganhDaoTao).Where(l => l.ChuNhiem == mailGV);
            var Lop = new string[lopQuanLies.Count()];
            var SiSo = new int[lopQuanLies.Count()];
            var idLop = new int[lopQuanLies.Count()];
            int stt = 0;
            foreach (var item in lopQuanLies)
            {
                Lop[stt] = item.TenLop;
                idLop[stt] = item.ID;
                stt++;
            }
            for (int i = 0; i < Lop.Length; i++)
            {
                foreach (var item in db.SinhViens.ToList())
                {
                    if (item.LopQuanLy.TenLop == Lop[i])
                    {
                        SiSo[i]++;
                    }
                }
            }
            ViewData["DanhSachLop"] = Lop;
            ViewData["SiSo"] = SiSo;
            ViewData["idLop"] = idLop;
            return View(lopQuanLies.ToList());
        }
        public ActionResult DanhSachSV(int idLop)
        {
            var danhSachSV = db.SinhViens.Where(s => s.LopQuanLy.ID == idLop).OrderBy(s => s.ID_TinhTrang).ToList();

            return View(danhSachSV);
        }
        public ActionResult DanhSachDotChinhSua()
        {
            var chinhSuaThongTins = db.DotChinhSuaThongTins.Include(s => s.LopQuanLy);
            return View(chinhSuaThongTins.ToList());
        }
        public ActionResult TaoDotChinhSua()
        {
            var mailGV = User.Identity.Name;
            ViewData["LopQuanLy"] = db.LopQuanLies.Where(l => l.ChuNhiem == mailGV).ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TaoDotChinhSua(DotChinhSuaThongTin dotChinhSuaThongTin, int[] id)
        {
            if (ModelState.IsValid)
            {
                if (dotChinhSuaThongTin.NgayKetThuc > dotChinhSuaThongTin.NgayBatDau)
                {
                    for (int i = 0; i < id.Length; i++)
                    {
                        dotChinhSuaThongTin.NguoiTao = User.Identity.Name;
                        dotChinhSuaThongTin.DotChinhSua = "Đợt chỉnh sửa tháng " + dotChinhSuaThongTin.NgayBatDau.Month;
                        dotChinhSuaThongTin.TinhTrang = true;
                        dotChinhSuaThongTin.LopQuanLy = db.LopQuanLies.Find(id[i]);
                        db.Entry(dotChinhSuaThongTin).State = EntityState.Added;
                        db.SaveChanges();
                    }
                    return RedirectToAction("DanhSachDotChinhSua");
                }
            }
            return View(dotChinhSuaThongTin);
        }
        public ActionResult XemChiTietSV(int id)
        {
            Session["sinhvien"] = db.SinhViens.Find(id);
            Session["tinhtrang"] = db.SinhViens.Find(id).TinhTrang.TenTinhTrang;
            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult XemDiemSinhVien(int id)
        {
            var sinhvien = db.SinhViens.Find(id);
            Session["sinhvien1"] = sinhvien;
            Session["diemso"] = db.DiemHocPhans.Where(s => s.MSSV == sinhvien.MSSV).ToList();
            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult XoaSVKhoiLop(int id)
        {
            var sv = db.SinhViens.Find(id);
            //ViewBag.TinhTrang = db.TinhTrangs.ToList();
            if (sv.TinhTrang.TenTinhTrang == "Còn học")
            {
                var ChuyenLop = db.TinhTrangs.FirstOrDefault(t => t.TenTinhTrang == "Chuyển lớp");
                sv.ID_TinhTrang = ChuyenLop.ID;
                db.SaveChanges();
            }
            return Redirect(Request.UrlReferrer.ToString());
        }
        [HttpPost]
        public ActionResult ThemSinhVienVaoLop(int idLop, string SinhVien)
        {
            var sv = db.SinhViens.FirstOrDefault(s => s.MSSV == SinhVien);
            if (sv != null)
            {
                var ConHoc = db.TinhTrangs.FirstOrDefault(t => t.TenTinhTrang == "Còn học");
                sv.ID_TinhTrang = ConHoc.ID;
                sv.ID_Lop = idLop;
                db.SaveChanges();
            }
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
