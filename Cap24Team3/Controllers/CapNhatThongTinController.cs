using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cap24Team3.Models;

namespace Cap24Team3.Controllers
{
    public class CapNhatThongTinController : Controller
    {
        // GET: CapNhatThongTin
        private Cap24 db = new Cap24();
        public ActionResult CapNhatThongTin()
        {
            var mail = User.Identity.Name;
            if (mail != null)
            {
                var sinhvien = db.SinhViens.FirstOrDefault(s => s.Email_1 == mail);
                if (sinhvien != null)
                    return View(sinhvien);
            }
            return View();
        }
        [HttpPost]
        public ActionResult CapNhatThongTin(string mail, int? sdt, int? dtcha, int? dtme, string diachi)
        {
            var email = User.Identity.Name;
            if (email != null)
            {
                var sinhvien = db.SinhViens.FirstOrDefault(s => s.Email_1 == mail);
                if (sinhvien != null)
                {
                    if(mail != null)
                    sinhvien.Email_2 = mail;
                    if(sdt != null)
                    sinhvien.DTDD = sdt.ToString();
                    if(dtcha != null)
                    sinhvien.DTCha = dtcha.ToString();
                    if(dtme != null)
                    sinhvien.DTMe = dtme.ToString();
                    if(diachi != null)
                    sinhvien.DiaChi = diachi;
                    db.Entry(sinhvien).State = EntityState.Modified;
                    db.SaveChanges();
                    return View(sinhvien);
                }
            }
            return View();
        }
    }
}