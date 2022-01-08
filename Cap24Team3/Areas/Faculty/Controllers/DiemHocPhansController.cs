using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Cap24Team3.Models;
using OfficeOpenXml;

namespace Cap24Team3.Areas.Faculty.Controllers
{
    public class DiemHocPhansController : Controller
    {
        private Cap24 db = new Cap24();

        [HttpPost]
        public ActionResult UploadDSDiem(FormCollection formCollection)
        {
            DiemHocPhan DSDiem = new DiemHocPhan();
            HttpPostedFileBase file = Request.Files["UploadedFile"];
            if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
            {

                string fileName = file.FileName;
                string fileContentType = file.ContentType;
                byte[] fileBytes = new byte[file.ContentLength];
                var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                using (var package = new ExcelPackage(file.InputStream))
                {
                    var currentSheet = package.Workbook.Worksheets;
                    var workSheet = currentSheet.First();
                    if (workSheet.Dimension != null)
                    {
                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;


                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {
                            DiemHocPhan savediem = new DiemHocPhan();
                            if (workSheet.Cells[rowIterator, 1].Value != null)
                            {
                                savediem.MSSV = workSheet.Cells[rowIterator, 1].Value.ToString();
                            }
                            if (workSheet.Cells[rowIterator, 3].Value != null)
                            {
                                savediem.HocKy = int.Parse(workSheet.Cells[rowIterator, 3].Value.ToString().Replace("HK", string.Empty));
                            }

                            if (workSheet.Cells[rowIterator, 4].Value != null)
                            {
                                savediem.HocPhan = workSheet.Cells[rowIterator, 4].Value.ToString();
                            }
                            if (workSheet.Cells[rowIterator, 6].Value != null)
                            {
                                savediem.TenHocPhan = workSheet.Cells[rowIterator, 6].Value.ToString();
                            }

                            if (workSheet.Cells[rowIterator, 7].Value != null)
                            {
                                savediem.SoTinChi = int.Parse(workSheet.Cells[rowIterator, 7].Value.ToString().Trim());
                            }

                            if (workSheet.Cells[rowIterator, 8].Value != null)
                            {
                                savediem.Diem10 = workSheet.Cells[rowIterator, 8].Value.ToString();
                            }
                            if (workSheet.Cells[rowIterator, 9].Value != null)
                            {
                                savediem.Diem4 = workSheet.Cells[rowIterator, 9].Value.ToString();
                            }
                            if (workSheet.Cells[rowIterator, 10].Value != null)
                            {
                                savediem.DiemChu = workSheet.Cells[rowIterator, 10].Value.ToString();
                            }
                            if (workSheet.Cells[rowIterator, 11].Value != null)
                            {
                                var quamon = workSheet.Cells[rowIterator, 11].Value.ToString().Trim();
                                if (quamon == "x")
                                {
                                    savediem.QuaMon = true;
                                }
                                else
                                {
                                    savediem.QuaMon = false;
                                }
                            }
                            //if (workSheet.Cells[rowIterator, 12].Value != null)
                            //{
                            //    var timelichsu = workSheet.Cells[rowIterator, 11].Value.ToString().Trim();
                            //    //savesv.LichSuUpLoad = db.LichSuUpLoads.FirstOrDefault(s => s.ThoiGian == timelichsu);
                            //}
                            db.DiemHocPhans.Add(savediem);
                            db.SaveChanges();
                        }

                    }
                }
            }

            //db.Configuration.AutoDetectChangesEnabled = false;
            //db.Configuration.ValidateOnSaveEnabled = false;
            //db.SaveChanges();
            return Redirect(Request.UrlReferrer.ToString());
        }

        // GET: Faculty/DiemHocPhans
        public ActionResult Index()
        {
            var diemHocPhans = db.DiemHocPhans.Include(d => d.LichSuUpLoad);
            return View(diemHocPhans.ToList());
        }

        // GET: Faculty/DiemHocPhans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiemHocPhan diemHocPhan = db.DiemHocPhans.Find(id);
            if (diemHocPhan == null)
            {
                return HttpNotFound();
            }
            return View(diemHocPhan);
        }

        // GET: Faculty/DiemHocPhans/Create
        public ActionResult Create()
        {
            ViewBag.LichSu = new SelectList(db.LichSuUpLoads, "ID", "NguoiUpload");
            return View();
        }

        // POST: Faculty/DiemHocPhans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,MSSV,HocPhan,TenHocPhan,HocKy,SoTinChi,Diem10,Diem4,DiemChu,QuaMon,LichSu")] DiemHocPhan diemHocPhan)
        {
            if (ModelState.IsValid)
            {
                db.DiemHocPhans.Add(diemHocPhan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LichSu = new SelectList(db.LichSuUpLoads, "ID", "NguoiUpload", diemHocPhan.LichSu);
            return View(diemHocPhan);
        }

        // GET: Faculty/DiemHocPhans/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiemHocPhan diemHocPhan = db.DiemHocPhans.Find(id);
            if (diemHocPhan == null)
            {
                return HttpNotFound();
            }
            ViewBag.LichSu = new SelectList(db.LichSuUpLoads, "ID", "NguoiUpload", diemHocPhan.LichSu);
            return View(diemHocPhan);
        }

        // POST: Faculty/DiemHocPhans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,MSSV,HocPhan,TenHocPhan,HocKy,SoTinChi,Diem10,Diem4,DiemChu,QuaMon,LichSu")] DiemHocPhan diemHocPhan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(diemHocPhan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LichSu = new SelectList(db.LichSuUpLoads, "ID", "NguoiUpload", diemHocPhan.LichSu);
            return View(diemHocPhan);
        }

        // GET: Faculty/DiemHocPhans/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiemHocPhan diemHocPhan = db.DiemHocPhans.Find(id);
            if (diemHocPhan == null)
            {
                return HttpNotFound();
            }
            return View(diemHocPhan);
        }

        // POST: Faculty/DiemHocPhans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DiemHocPhan diemHocPhan = db.DiemHocPhans.Find(id);
            db.DiemHocPhans.Remove(diemHocPhan);
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
