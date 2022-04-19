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
            var mail = User.Identity.Name;
            if (mail != "")
            {
                //var sinhvien = db.SinhViens.FirstOrDefault(s => s.Email_1 == mail);
                //var thongBaos = db.ThongBaos.Where(t => t.NguoiNhan == sinhvien.Email_1).ToList(); Longu lam sai
                var thongBaos = db.ThongBaos.Where(t => t.NguoiNhan == mail).ToList();
                foreach (var item in thongBaos)
                {
                    ViewData["thongbao"] = item;
                }
                return View(thongBaos.ToList());
            }
            else
            {
                TempData["Alert"] = "Yêu cầu đăng nhập";
                return View();
            }
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
        // GET: ThongBaos/Create
        public ActionResult TaoThongBao()
        {
            var mail = User.Identity.Name;
            if (mail != "")
            {
                ViewData["DSLop"] = db.LopQuanLies.OrderByDescending(l => l.ID_Khoa).ToList();
                ViewData["DSKhoa"] = db.KhoaDaoTaos.OrderByDescending(l => l.ID).ToList();
                ViewData["DSNganh"] = db.NganhDaoTaos.ToList();
                var nguoiTao = db.AspNetUsers.FirstOrDefault(n => n.Email == mail);
                ViewData["NguoiTao"] = nguoiTao;
                return View();
            }
            else
            {
                TempData["Alert"] = "Yêu cầu đăng nhập";
                return View();
            }           
        }

        // POST: ThongBaos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult TaoThongBao(string tieuDe, string noiDung, DateTime ngay, string nguoiGui, int[] idKhoa, int[] idNganh, int[] idLop)
        {
            var email = User.Identity.Name;
            if (email != null)
            {
                if (ModelState.IsValid)
                {
                    if (idLop != null)
                    {
                        for (int i = 0; i < idLop.Length; i++)
                        {
                            var listLopMoi = new List<LopQuanLy>();
                            var stringlop = new List<string>();
                            var listLop = db.LopQuanLies.Find(idLop[i]);
                            var listsv = db.SinhViens.Where(s => s.LopQuanLy.ID == listLop.ID).ToList();
                            foreach (var sv in listsv)
                            {
                                string mail = sv.Email_1;
                                var thongBao = new ThongBao();
                                thongBao.Ngay = ngay;
                                thongBao.TieuDe = tieuDe;
                                thongBao.NoiDung = noiDung;
                                thongBao.NguoiGui = nguoiGui;
                                thongBao.NguoiNhan = mail;
                                thongBao.TrangThai = false;
                                db.Entry(thongBao).State = EntityState.Added;
                            }
                        }
                        db.SaveChanges();
                    }
                    if (idNganh != null && idKhoa != null)
                    {
                        for (int i = 0; i < idNganh.Length; i++)
                        {
                            for (int j = 0; j < idKhoa.Length; j++)
                            {
                                var listNganh = db.NganhDaoTaos.Find(idNganh[i]);
                                var listKhoa = db.KhoaDaoTaos.Find(idKhoa[j]);
                                var listsv = db.SinhViens.Where(s => s.NganhDaoTao.ID == listNganh.ID).Where(s => s.KhoaDaoTao.ID == listKhoa.ID).ToList();
                                foreach (var sv in listsv)
                                {
                                    string mail = sv.Email_1;
                                    var thongBao = new ThongBao();
                                    thongBao.Ngay = ngay;
                                    thongBao.TieuDe = tieuDe;
                                    thongBao.NoiDung = noiDung;
                                    thongBao.NguoiGui = nguoiGui;
                                    thongBao.NguoiNhan = mail;
                                    thongBao.TrangThai = false;
                                    db.Entry(thongBao).State = EntityState.Added;
                                }
                            }
                        }
                        db.SaveChanges();
                    }
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        // GET: ThongBaos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThongBao thongBao = db.ThongBaos.Find(id);
            if (thongBao == null)
            {
                return HttpNotFound();
            }
            ViewBag.NguoiGui = new SelectList(db.AspNetUsers, "Id", "Email", thongBao.NguoiGui);
            return View(thongBao);
        }

        // POST: ThongBaos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,TieuDe,NoiDung,Ngay,NguoiGui,NguoiNhan")] ThongBao thongBao)
        {
            if (ModelState.IsValid)
            {
                db.Entry(thongBao).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.NguoiGui = new SelectList(db.AspNetUsers, "Id", "Email", thongBao.NguoiGui);
            return View(thongBao);
        }

        // GET: ThongBaos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThongBao thongBao = db.ThongBaos.Find(id);
            if (thongBao == null)
            {
                return HttpNotFound();
            }
            return View(thongBao);
        }

        // POST: ThongBaos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ThongBao thongBao = db.ThongBaos.Find(id);
            db.ThongBaos.Remove(thongBao);
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
