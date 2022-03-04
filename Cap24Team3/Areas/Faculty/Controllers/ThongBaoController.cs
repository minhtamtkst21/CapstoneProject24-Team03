using Cap24Team3.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Cap24Team3.Areas.Faculty.Controllers
{
    public class ThongBaoController : Controller
    {
        private Cap24 db = new Cap24();
        // GET: Faculty/ThongBao
        public ActionResult Index()
        {
            var thongBaos = db.ThongBaos.ToList();
            return View(thongBaos.ToList());
        }
        public ActionResult TaoThongBao()
        {
            var mail = User.Identity.Name;
            if (mail != "")
            {
                ViewData["DSLop"] = db.LopQuanLies.OrderByDescending(l => l.ID_Khoa).ToList();
                ViewData["DSKhoa"] = db.KhoaDaoTaos.OrderByDescending(l => l.ID).ToList();
                ViewData["DSNganh"] = db.NganhDaoTaos.ToList();
                
                return View();
            }
            else
            {
                TempData["AlertCreate"] = "Yêu cầu đăng nhập";
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
                    if(idLop != null && idNganh != null && idKhoa != null)
                    {
                        TempData["AlertCreate"] = "Vui lòng không chọn cả ba cột người nhận";
                        return Redirect(Request.UrlReferrer.ToString());
                    }
                    else if (idLop != null)
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
                    else if (idNganh != null && idKhoa != null)
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
                    else if(idNganh != null && idKhoa == null)
                    {
                        for (int i = 0; i < idNganh.Length; i++)
                        {
                            var listNganh = db.NganhDaoTaos.Find(idNganh[i]);
                            var listsv = db.SinhViens.Where(s => s.NganhDaoTao.ID == listNganh.ID).ToList();
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
                    else if(idNganh == null && idKhoa != null)
                    {
                        TempData["AlertCreate"] = "Vui lòng chọn ngành theo khóa mà bạn muốn gửi thông báo";
                        return Redirect(Request.UrlReferrer.ToString());
                    }
                    else
                    {
                        TempData["AlertCreate"] = "Vui lòng chọn khóa ngành hoặc chọn lớp muốn gửi thông báo";
                        return Redirect(Request.UrlReferrer.ToString());
                    }
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
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