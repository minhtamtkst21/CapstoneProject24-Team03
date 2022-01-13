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
    public class ChinhSuaThongTinsvController : Controller
    {
        private Cap24 db = new Cap24();

        // GET: ChinhSuaThongTinsv
        public ActionResult Index()
        {
            var chinhSuaThongTins = db.ChinhSuaThongTins.Include(c => c.DotChinhSuaThongTin).Include(c => c.SinhVien);
            return View(chinhSuaThongTins.ToList());
        }

        // GET: ChinhSuaThongTinsv/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChinhSuaThongTin chinhSuaThongTin = db.ChinhSuaThongTins.Find(id);
            if (chinhSuaThongTin == null)
            {
                return HttpNotFound();
            }
            return View(chinhSuaThongTin);
        }

        // GET: ChinhSuaThongTinsv/Create
        public ActionResult Create()
        {
            ViewBag.ID_DotChinhSua = new SelectList(db.DotChinhSuaThongTins, "ID", "DotChinhSua");
            ViewBag.ID_SinhVien = new SelectList(db.SinhViens, "ID", "MSSV");
            return View();
        }

        // POST: ChinhSuaThongTinsv/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ID_SinhVien,MailCaNhan,DTDD,DTCha,DTMe,DiaChi,ID_DotChinhSua")] ChinhSuaThongTin chinhSuaThongTin)
        {
            if (ModelState.IsValid)
            {
                db.ChinhSuaThongTins.Add(chinhSuaThongTin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID_DotChinhSua = new SelectList(db.DotChinhSuaThongTins, "ID", "DotChinhSua", chinhSuaThongTin.ID_DotChinhSua);
            ViewBag.ID_SinhVien = new SelectList(db.SinhViens, "ID", "MSSV", chinhSuaThongTin.ID_SinhVien);
            return View(chinhSuaThongTin);
        }

        // GET: ChinhSuaThongTinsv/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChinhSuaThongTin chinhSuaThongTin = db.ChinhSuaThongTins.Find(id);
            if (chinhSuaThongTin == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_DotChinhSua = new SelectList(db.DotChinhSuaThongTins, "ID", "DotChinhSua", chinhSuaThongTin.ID_DotChinhSua);
            ViewBag.ID_SinhVien = new SelectList(db.SinhViens, "ID", "MSSV", chinhSuaThongTin.ID_SinhVien);
            return View(chinhSuaThongTin);
        }

        // POST: ChinhSuaThongTinsv/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ID_SinhVien,MailCaNhan,DTDD,DTCha,DTMe,DiaChi,ID_DotChinhSua")] ChinhSuaThongTin chinhSuaThongTin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chinhSuaThongTin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_DotChinhSua = new SelectList(db.DotChinhSuaThongTins, "ID", "DotChinhSua", chinhSuaThongTin.ID_DotChinhSua);
            ViewBag.ID_SinhVien = new SelectList(db.SinhViens, "ID", "MSSV", chinhSuaThongTin.ID_SinhVien);
            return View(chinhSuaThongTin);
        }

        // GET: ChinhSuaThongTinsv/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChinhSuaThongTin chinhSuaThongTin = db.ChinhSuaThongTins.Find(id);
            if (chinhSuaThongTin == null)
            {
                return HttpNotFound();
            }
            return View(chinhSuaThongTin);
        }

        // POST: ChinhSuaThongTinsv/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ChinhSuaThongTin chinhSuaThongTin = db.ChinhSuaThongTins.Find(id);
            db.ChinhSuaThongTins.Remove(chinhSuaThongTin);
            db.SaveChanges();
            return RedirectToAction("Index");
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
