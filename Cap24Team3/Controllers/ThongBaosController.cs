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
    public class ThongBaosController : Controller
    {
        private Cap24 db = new Cap24();
        [ChildActionOnly]
        public ActionResult BellTB()
        {
            var mail = User.Identity.Name;
            var listTb = db.ThongBaos.Where(t => t.NguoiNhan == mail).ToList();
            if(listTb.Count > 0)
            {
                ViewData["BellDB"] = listTb;
                return PartialView("Bell", new ThongBao());
            }
            else
            {
                TempData["AlertBell"] = "Hiện chưa có thông báo nào";
                return PartialView("Bell", new ThongBao());
            }
        }
        [ChildActionOnly]
        public ActionResult ModalXemTB()
        {
            return PartialView("ModalTB", new ThongBao());
        }
        public ActionResult NhanThongBao()
        {
            return View();
        }
        public ActionResult XemThongBao(int idTB)
        {
            var thongBao = db.ThongBaos.FirstOrDefault(t => t.ID == idTB);
            thongBao.TrangThai = true;
            db.Entry(thongBao).State = EntityState.Modified;
            db.SaveChanges();
            Session["XemThongBao"] = thongBao;
            ViewData["XemThongBao"] = thongBao;
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
