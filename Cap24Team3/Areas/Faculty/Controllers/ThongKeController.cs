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
            string tshk = db.Thamsoes.FirstOrDefault(s => s.Ma == "HocKyHienTai").Giatri;
            var listdiem = db.DiemHocPhans.ToList();
            var listthongke = new List<thongkehocphan>();
            foreach(var item in listdiem)
            {
                var mssv = item.MSSV;
                var sv = db.SinhViens.FirstOrDefault(s => s.MSSV == mssv);
            }
            return View();
        }
        public ActionResult XemChiTiet()
        {
            return View();
        }
    }
}