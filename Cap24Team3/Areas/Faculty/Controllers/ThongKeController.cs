using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cap24Team3.Models;

namespace Cap24Team3.Areas.Faculty.Controllers
{
    public class ThongKeController : Controller
    {
        private Cap24 db = new Cap24();
        // GET: Faculty/ThongKe
        public ActionResult ThongKe()
        {
            var khoa = db.KhoaDaoTaos.ToList();
            var nganh = db.NganhDaoTaos.ToList();

            TempData["khoa"] = khoa;
            TempData["nganh"] = nganh;
            return View();
        }
        [HttpPost]
        public ActionResult LuuThongKe(int? khoa, int? nganh)
        {
            if (khoa is null)
            {
                throw new ArgumentNullException(nameof(khoa));
            }

            if (nganh is null)
            {
                throw new ArgumentNullException(nameof(nganh));
            }

            string tshk = db.Thamsoes.FirstOrDefault(s => s.Ma == "HocKyHienTai").Giatri;
            int hkht = db.HocKyDaoTaos.FirstOrDefault(s => s.HocKy.ToString() == tshk).STT;
            var listsv = db.SinhViens.Where(s => s.KhoaDaoTao.ID == khoa).Where(s => s.NganhDaoTao.ID == nganh).ToList();
            var listthongke = new List<chitietthongke>();
            int d = 0;
            foreach (var sinhvien in listsv)
            {
                d++;
                int hksv = hkht - db.HocKyDaoTaos.FirstOrDefault(s => s.HocKy == sinhvien.HocKyBatDau).STT + 1;
                var listhp = db.DiemHocPhans.Where(s => s.HocKyDangKy > hksv).Where(s => s.MSSV == sinhvien.MSSV).ToList();
                foreach (var item in listhp)
                {
                    var tk = new chitietthongke();
                    tk.MSSV = sinhvien.Ho + " " + sinhvien.Ten;
                    tk.TenHP = item.TenHocPhan;
                    tk.MaHP = item.HocPhan;
                    if (item.HocKyDangKy > hksv)
                    {
                        listthongke.Add(tk);
                    }
                }
            }
            Session["thongke"] = listthongke;
            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult XemChiTiet()
        {
            return View();
        }
    }
}