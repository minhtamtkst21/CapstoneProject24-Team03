using Cap24Team3.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Controllers
{
    public class HocPhanDaoTaoController : Controller
    {
        private Cap24 db = new Cap24();
        public bool CheckTonTai(string element, List<string> list)
        {
            foreach (var item in list)
            {
                if (element == item)
                {
                    return true;
                }
            }
            return false;
        }
        // GET: HocPhanDaoTao
        public ActionResult Index()
        {
            string ListLoi = "";
            var mailsv = User.Identity.Name;
            var sinhvien = db.SinhViens.FirstOrDefault(s => s.Email_1 == mailsv);
            if (sinhvien != null)
            {
                var nganh = db.NganhDaoTaos.FirstOrDefault(s => s.ID == sinhvien.ID_Nganh);
                var khoa = db.KhoaDaoTaos.FirstOrDefault(s => s.ID == sinhvien.ID_Khoa);
                var ctdt = db.ChuongTrinhDaoTaos.Where(s => s.ID_Nganh == nganh.ID).FirstOrDefault(s => s.ID_Khoa == khoa.ID);
                if (ctdt == null)
                {
                    ListLoi += "<p> Chương trình đào tạo của Khóa K" + khoa.Khoa + " - Ngành " + nganh.Nganh + " bị trống</p>";
                    TempData["Alert"] = ListLoi;
                    return View("Index");
                }
                else
                {
                    var hocPhanDaoTaos = db.HocPhanDaoTaos.Include(h => h.KhoiKienThuc).Include(h => h.HocPhanDaoTao2).Where(s => s.KhoiKienThuc.ChuongTrinhDaoTao.ID == ctdt.ID).ToList();
                    ViewData["NganhDaoTao"] = db.NganhDaoTaos.ToList();
                    ViewData["KhoaDaoTao"] = db.KhoaDaoTaos.ToList();
                    ViewData["HocKyDaoTao"] = db.HocKyDaoTaos.ToList();
                    ViewData["RangBuocHocPhan"] = db.RangBuocHocPhans.ToList();
                    var listHK = new List<string>();
                    var listHP = db.HocPhanDaoTaos.ToList();
                    foreach (var item in hocPhanDaoTaos)
                        if (!CheckTonTai(item.HocKy.ToString(), listHK))
                            listHK.Add(item.HocKy.ToString());
                    ViewData["listHK"] = listHK;
                    return View(hocPhanDaoTaos.ToList());
                }
            }
            else
            {
                TempData["Alert"] = "Bạn chưa đăng nhập!";
                return View("Index");
            }
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
