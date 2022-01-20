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
            var user = User.Identity.Name;
            if (user != null)
            {
                var sinhvien = db.SinhViens.FirstOrDefault(s => s.Email_1 == user);
                var thongtin = new ChinhSuaThongTin();
                if (sinhvien != null)
                {
                    if (mail != null)
                        thongtin.MailCaNhan = mail;
                    if (sdt != null)
                        thongtin.DTDD = sdt.ToString();
                    if (dtcha != null)
                        thongtin.DTCha = dtcha.ToString();
                    if (dtme != null)
                        thongtin.DTMe = dtme.ToString();
                    if (diachi != null)
                        thongtin.DiaChi = diachi;
                    thongtin.SinhVien = sinhvien;
                    db.Entry(thongtin).State = EntityState.Added;
                    db.SaveChanges();
                    ViewData["sinhvien"] = db.SinhViens.FirstOrDefault(s => s.Email_1 == mail);
                    return View();
                }
            }
            return View();
        }
    }
}