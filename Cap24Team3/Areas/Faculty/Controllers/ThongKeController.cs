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
        public ActionResult LuuThongKe()
        {
            string tshk = db.Thamsoes.FirstOrDefault(s => s.Ma == "HocKyHienTai").Giatri;
            int hkht = db.HocKyDaoTaos.FirstOrDefault(s => s.HocKy.ToString() == tshk).STT;
            var listdiem = db.DiemHocPhans.Where(s => s.HocKyDangKy > 1).ToList();
            var listthongke = new List<thongkehocphan>();
            int d = 0;
            foreach (var item in listdiem)
            {
                d++;
                var mssv = item.MSSV;
                var sv = db.SinhViens.FirstOrDefault(s => s.MSSV == mssv);
                int hksv = hkht - db.HocKyDaoTaos.FirstOrDefault(s => s.HocKy == sv.HocKyBatDau).STT + 1;
                var tk = new thongkehocphan();
                tk.MSSV = mssv;
                tk.TenHP = item.TenHocPhan;
                tk.MaHP = item.HocPhan;
                if (item.HocKyDangKy > hksv)
                {
                    listthongke.Add(tk);
                }
            }
            return View();
        }
        public ActionResult XemChiTiet()
        {
            return View();
        }
    }
}