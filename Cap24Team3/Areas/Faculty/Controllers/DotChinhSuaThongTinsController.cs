using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Cap24Team3.Models;

namespace Cap24Team3.Areas.Faculty.Controllers
{
    public class DotChinhSuaThongTinsController : Controller
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

        public string KiemTraDotChinhSua(string dotchinhsua)
        {
            string ListLoi = "";

            var listDCS = db.DotChinhSuaThongTins.ToList();
            var listCS = new List<string>();
            foreach (var item in listDCS)
            {
                listCS.Add(item.DotChinhSua.ToString());
            }
            if (CheckTonTai(dotchinhsua.ToString(), listCS))
            {
                ListLoi += "<p> Học kỳ đào tạo đã tồn tại trong hệ thống, vui lòng thử lại!</p>";
            }
            return ListLoi;
        }
        public ActionResult ListDotChinhSua()
        {
            return View(db.DotChinhSuaThongTins.ToList());
        }

        // GET: Faculty/DotChinhSuaThongTins

        // GET: Faculty/DotChinhSuaThongTins/Create
        public ActionResult CreateDotChinhSua()
        {
            return View();
        }

        // POST: Faculty/DotChinhSuaThongTins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDotChinhSua([Bind(Include = "ID,DotChinhSua,NgayBatDau,NgayKetThuc,NguoiTao,Lop")] DotChinhSuaThongTin dotChinhSuaThongTin)
        {
            if (ModelState.IsValid)
            {
                var ListLoi = KiemTraDotChinhSua(dotChinhSuaThongTin.DotChinhSua);
                if (ListLoi != "")
                {
                    TempData["Alert"] = ListLoi;
                    return RedirectToAction("EditDotChinhSua", new { id = dotChinhSuaThongTin.ID });
                }
                db.DotChinhSuaThongTins.Add(dotChinhSuaThongTin);
                db.SaveChanges();
                TempData["ThongBao"] = "Xóa đợt chỉnh sửa thành công";
                return RedirectToAction("ListDotChinhSua");
            }

            return View(dotChinhSuaThongTin);
        }

        // GET: Faculty/DotChinhSuaThongTins/Edit/5
        public ActionResult EditDotChinhSua(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DotChinhSuaThongTin dotChinhSuaThongTin = db.DotChinhSuaThongTins.Find(id);
            if (dotChinhSuaThongTin == null)
            {
                return HttpNotFound();
            }
            return View(dotChinhSuaThongTin);
        }

        // POST: Faculty/DotChinhSuaThongTins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDotChinhSua([Bind(Include = "ID,DotChinhSua,NgayBatDau,NgayKetThuc,NguoiTao,Lop")] DotChinhSuaThongTin dotChinhSuaThongTin)
        {
            if (ModelState.IsValid)
            {
                var ListLoi = KiemTraDotChinhSua(dotChinhSuaThongTin.DotChinhSua);
                if (ListLoi != "")
                {
                    TempData["Alert"] = ListLoi;
                    return RedirectToAction("EditDotChinhSua", new { id = dotChinhSuaThongTin.ID });
                }
                db.Entry(dotChinhSuaThongTin).State = EntityState.Modified;
                db.SaveChanges();
                TempData["ThongBao"] = "Xóa đợt chỉnh sửa thành công";
                return RedirectToAction("ListDotChinhSua");

            }
            return View(dotChinhSuaThongTin);
        }

        // GET: Faculty/DotChinhSuaThongTins/Delete/5
        public ActionResult XoaDotChinhSua(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DotChinhSuaThongTin dotChinhSuaThongTin = db.DotChinhSuaThongTins.Find(id);
            if (dotChinhSuaThongTin == null)
            {
                return HttpNotFound();
            }
            return View(dotChinhSuaThongTin);
        }

        // POST: Faculty/DotChinhSuaThongTins/Delete/5
        [HttpPost, ActionName("XoaDotChinhSua")]
        [ValidateAntiForgeryToken]
        public ActionResult XoaDotChinhSuaConfirmed(int id)
        {
            DotChinhSuaThongTin dotChinhSuaThongTin = db.DotChinhSuaThongTins.Find(id);
            db.DotChinhSuaThongTins.Remove(dotChinhSuaThongTin);
            db.SaveChanges();
            TempData["ThongBao"] = "Xóa đợt chỉnh sửa thành công";
            return RedirectToAction("ListDotChinhSua");
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
