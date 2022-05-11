using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using Cap24Team3.Models;
using OfficeOpenXml;

namespace Cap24Team3.Areas.Faculty.Controllers
{
    [Authorize(Roles = "BCN Khoa")]
    public class DiemHocPhansController : Controller
    {
        private Cap24 db = new Cap24();
        [HttpPost]
        public ActionResult ThongKe(FormCollection formCollection)
        {
            int dem = 0;
            var ListSinhVien = new List<string>();
            Session["LuuDiem"] = new List<DiemHocPhan>();
            Session["LuuLichSu"] = new LichSuUpLoad();
            HttpPostedFileBase file = Request.Files["UploadedFile"];
            if (System.IO.File.Exists(Server.MapPath(UPLOAD_PATH) + "0.xlsx"))
            {
                System.IO.File.Delete(Server.MapPath(UPLOAD_PATH) + "0.xlsx");
            }
            string _FileName = Path.GetFileName(file.FileName);
            string _path = Path.Combine(Server.MapPath(UPLOAD_PATH), "0.xlsx");
            file.SaveAs(_path);
            if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
            {
                string DanhSachLoi = "";
                string mssvmoi = "";
                using (var package = new ExcelPackage(file.InputStream))
                {
                    var currentSheet = package.Workbook.Worksheets;
                    var workSheet = currentSheet.First();
                    if (workSheet.Dimension != null)
                    {
                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;

                        (Session["LuuLichSu"] as LichSuUpLoad).ThoiGian = DateTime.Now;
                        (Session["LuuLichSu"] as LichSuUpLoad).NguoiUpload = User.Identity.Name;
                        var ListLS = db.LichSuUpLoads.ToList();
                        if (ListLS.Count != 0)
                        {
                            (Session["LuuLichSu"] as LichSuUpLoad).ID = ListLS.OrderByDescending(s => s.ID).First().ID + 1;
                        }
                        else
                        {
                            (Session["LuuLichSu"] as LichSuUpLoad).ID = 1;
                        }
                        if (noOfCol == 11 && noOfRow > 1)
                        {
                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {
                                try
                                {
                                    DiemHocPhan savediem = new DiemHocPhan();
                                    savediem.MSSV = (workSheet.Cells[rowIterator, 1].Value == null) ? "" : workSheet.Cells[rowIterator, 1].Value.ToString().Trim();
                                    var HocKy = int.Parse(workSheet.Cells[rowIterator, 5].Value.ToString().Substring(0, 3));
                                    var sinhvien = db.SinhViens.FirstOrDefault(s => s.MSSV == savediem.MSSV);
                                    var listhk = new List<HocKyDaoTao>();
                                    var hockysv = 0;
                                    var hockyhp = 0;
                                    if (sinhvien != null)
                                    {
                                        hockysv = db.HocKyDaoTaos.FirstOrDefault(s => s.HocKy == sinhvien.HocKyBatDau).STT;
                                        hockyhp = (db.HocKyDaoTaos.FirstOrDefault(s => s.HocKy == HocKy) != null) ? db.HocKyDaoTaos.FirstOrDefault(s => s.HocKy == HocKy).STT : hockysv;
                                    }
                                    else
                                    {
                                        TempData["Alert"] += "Không có sinh viên " + savediem.MSSV + " trong danh sách sinh viên!!";
                                    }
                                    savediem.HocKyKeHoach = hockyhp - hockysv + 1;
                                    savediem.HocKyDangKy = hockyhp - hockysv + 1;
                                    savediem.HocPhan = (workSheet.Cells[rowIterator, 4].Value == null) ? null : workSheet.Cells[rowIterator, 4].Value.ToString().Trim();
                                    savediem.TenHocPhan = (workSheet.Cells[rowIterator, 6].Value == null) ? null : workSheet.Cells[rowIterator, 6].Value.ToString().Trim();
                                    savediem.SoTinChi = (workSheet.Cells[rowIterator, 7].Value == null) ? -1 : int.Parse(workSheet.Cells[rowIterator, 7].Value.ToString().Trim());
                                    savediem.Diem10 = (workSheet.Cells[rowIterator, 8].Value == null) ? null : workSheet.Cells[rowIterator, 8].Value.ToString().Trim();
                                    savediem.Diem4 = (workSheet.Cells[rowIterator, 9].Value == null) ? null : workSheet.Cells[rowIterator, 9].Value.ToString().Trim();
                                    savediem.DiemChu = (workSheet.Cells[rowIterator, 10].Value == null) ? null : workSheet.Cells[rowIterator, 10].Value.ToString().Trim();
                                    savediem.QuaMon = (workSheet.Cells[rowIterator, 11].Value == null || workSheet.Cells[rowIterator, 11].Value.ToString().Trim() != "x") ? false : true;
                                    savediem.LichSu = (Session["LuuLichSu"] as LichSuUpLoad).ID;
                                    if (mssvmoi != savediem.MSSV.ToLower())
                                    {
                                        dem++;
                                        mssvmoi = savediem.MSSV.ToLower();
                                    }
                                   (Session["LuuDiem"] as List<DiemHocPhan>).Add(savediem);
                                }
                                catch (Exception ex)
                                {
                                    TempData["Alert"] = "Lỗi, vui lòng thử lại!! " + ex.Message;
                                    return RedirectToAction("Index");
                                }
                            }
                        }
                        else if (noOfCol != 11)
                        {
                            DanhSachLoi += "<p> File bị sai định dạng, vui lòng thử lại!!</p>";
                        }
                        else
                        {
                            DanhSachLoi += "<p> File bị trống, vui lòng thử lại!!</p>";
                        }
                    }
                }
                if (DanhSachLoi != "")
                {
                    TempData["Alert"] = DanhSachLoi;
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["Alert"] = "File bị trống, vui lòng thử lại!!";
                return RedirectToAction("Index");
            }
            Session["File"] = file;
            string thongbao = "<table class='table table-hover mb-0'>";
            thongbao += "<tr>";
            thongbao += "<td>Số lượng sinh viên: " + dem + "</td>";
            thongbao += "</tr>";
            thongbao += "<tr>";
            thongbao += "<td>Số lượng điểm được lưu: " + (Session["LuuDiem"] as List<DiemHocPhan>).Count + "</td>";
            thongbao += "</tr>";
            thongbao += "</table>";
            Session["ThongBao"] = thongbao;
            return Redirect("Index");
        }
        [HttpPost]
        public ActionResult ThongKe2(FormCollection formCollection)
        {
            int dem = 0;
            var ListSinhVien = new List<string>();
            Session["LuuDiem"] = new List<DiemHocPhan>();
            Session["LuuLichSu"] = new LichSuUpLoad();
            HttpPostedFileBase file = Request.Files["UploadedFile"];
            if (System.IO.File.Exists(Server.MapPath(UPLOAD_PATH) + "0.xlsx"))
            {
                System.IO.File.Delete(Server.MapPath(UPLOAD_PATH) + "0.xlsx");
            }
            string _FileName = Path.GetFileName(file.FileName);
            string _path = Path.Combine(Server.MapPath(UPLOAD_PATH), "0.xlsx");
            file.SaveAs(_path);
            if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
            {
                string DanhSachLoi = "";
                string mssvmoi = "";
                using (var package = new ExcelPackage(file.InputStream))
                {
                    var currentSheet = package.Workbook.Worksheets;
                    var workSheet = currentSheet.First();
                    if (workSheet.Dimension != null)
                    {
                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;

                        (Session["LuuLichSu"] as LichSuUpLoad).ThoiGian = DateTime.Now;
                        (Session["LuuLichSu"] as LichSuUpLoad).NguoiUpload = User.Identity.Name;
                        var ListLS = db.LichSuUpLoads.ToList();
                        if (ListLS.Count != 0)
                        {
                            (Session["LuuLichSu"] as LichSuUpLoad).ID = ListLS.OrderByDescending(s => s.ID).First().ID + 1;
                        }
                        else
                        {
                            (Session["LuuLichSu"] as LichSuUpLoad).ID = 1;
                        }
                        if (noOfCol == 11 && noOfRow > 1)
                        {
                            Thread t = new Thread(() =>
                            {
                                for (int rowIterator = 2; rowIterator <= noOfRow/2; rowIterator++)
                                {
                                    try
                                    {
                                        DiemHocPhan savediem = new DiemHocPhan();
                                        savediem.MSSV = (workSheet.Cells[rowIterator, 1].Value == null) ? "" : workSheet.Cells[rowIterator, 1].Value.ToString().Trim();
                                        var HocKy = int.Parse(workSheet.Cells[rowIterator, 5].Value.ToString().Substring(0, 3));
                                        var sinhvien = db.SinhViens.FirstOrDefault(s => s.MSSV == savediem.MSSV);
                                        var listhk = new List<HocKyDaoTao>();
                                        var hockysv = 0;
                                        var hockyhp = 0;
                                        if (sinhvien != null)
                                        {
                                            hockysv = db.HocKyDaoTaos.FirstOrDefault(s => s.HocKy == sinhvien.HocKyBatDau).STT;
                                            hockyhp = (db.HocKyDaoTaos.FirstOrDefault(s => s.HocKy == HocKy) != null) ? db.HocKyDaoTaos.FirstOrDefault(s => s.HocKy == HocKy).STT : hockysv;
                                        }
                                        else
                                        {
                                            TempData["Alert"] += "Không có sinh viên " + savediem.MSSV + " trong danh sách sinh viên!!";
                                        }
                                        savediem.HocKyKeHoach = hockyhp - hockysv + 1;
                                        savediem.HocKyDangKy = hockyhp - hockysv + 1;
                                        savediem.HocPhan = (workSheet.Cells[rowIterator, 4].Value == null) ? null : workSheet.Cells[rowIterator, 4].Value.ToString().Trim();
                                        savediem.TenHocPhan = (workSheet.Cells[rowIterator, 6].Value == null) ? null : workSheet.Cells[rowIterator, 6].Value.ToString().Trim();
                                        savediem.SoTinChi = (workSheet.Cells[rowIterator, 7].Value == null) ? -1 : int.Parse(workSheet.Cells[rowIterator, 7].Value.ToString().Trim());
                                        savediem.Diem10 = (workSheet.Cells[rowIterator, 8].Value == null) ? null : workSheet.Cells[rowIterator, 8].Value.ToString().Trim();
                                        savediem.Diem4 = (workSheet.Cells[rowIterator, 9].Value == null) ? null : workSheet.Cells[rowIterator, 9].Value.ToString().Trim();
                                        savediem.DiemChu = (workSheet.Cells[rowIterator, 10].Value == null) ? null : workSheet.Cells[rowIterator, 10].Value.ToString().Trim();
                                        savediem.QuaMon = (workSheet.Cells[rowIterator, 11].Value == null || workSheet.Cells[rowIterator, 11].Value.ToString().Trim() != "x") ? false : true;
                                        savediem.LichSu = (Session["LuuLichSu"] as LichSuUpLoad).ID;
                                        if (mssvmoi != savediem.MSSV.ToLower())
                                        {
                                            dem++;
                                            mssvmoi = savediem.MSSV.ToLower();
                                        }
                                       (Session["LuuDiem"] as List<DiemHocPhan>).Add(savediem);
                                    }
                                    catch (Exception ex)
                                    {
                                        TempData["Alert"] = "Lỗi, vui lòng thử lại!! " + ex.Message;
                                    }
                                }
                            });
                            t.Start();
                            Thread t2 = new Thread(() =>
                            {
                                for (int rowIterator = noOfRow/2; rowIterator <= noOfRow; rowIterator++)
                                {
                                    try
                                    {
                                        DiemHocPhan savediem = new DiemHocPhan();
                                        savediem.MSSV = (workSheet.Cells[rowIterator, 1].Value == null) ? "" : workSheet.Cells[rowIterator, 1].Value.ToString().Trim();
                                        var HocKy = int.Parse(workSheet.Cells[rowIterator, 5].Value.ToString().Substring(0, 3));
                                        var sinhvien = db.SinhViens.FirstOrDefault(s => s.MSSV == savediem.MSSV);
                                        var listhk = new List<HocKyDaoTao>();
                                        var hockysv = 0;
                                        var hockyhp = 0;
                                        if (sinhvien != null)
                                        {
                                            hockysv = db.HocKyDaoTaos.FirstOrDefault(s => s.HocKy == sinhvien.HocKyBatDau).STT;
                                            hockyhp = (db.HocKyDaoTaos.FirstOrDefault(s => s.HocKy == HocKy) != null) ? db.HocKyDaoTaos.FirstOrDefault(s => s.HocKy == HocKy).STT : hockysv;
                                        }
                                        else
                                        {
                                            TempData["Alert"] += "Không có sinh viên " + savediem.MSSV + " trong danh sách sinh viên!!";
                                        }
                                        savediem.HocKyKeHoach = hockyhp - hockysv + 1;
                                        savediem.HocKyDangKy = hockyhp - hockysv + 1;
                                        savediem.HocPhan = (workSheet.Cells[rowIterator, 4].Value == null) ? null : workSheet.Cells[rowIterator, 4].Value.ToString().Trim();
                                        savediem.TenHocPhan = (workSheet.Cells[rowIterator, 6].Value == null) ? null : workSheet.Cells[rowIterator, 6].Value.ToString().Trim();
                                        savediem.SoTinChi = (workSheet.Cells[rowIterator, 7].Value == null) ? -1 : int.Parse(workSheet.Cells[rowIterator, 7].Value.ToString().Trim());
                                        savediem.Diem10 = (workSheet.Cells[rowIterator, 8].Value == null) ? null : workSheet.Cells[rowIterator, 8].Value.ToString().Trim();
                                        savediem.Diem4 = (workSheet.Cells[rowIterator, 9].Value == null) ? null : workSheet.Cells[rowIterator, 9].Value.ToString().Trim();
                                        savediem.DiemChu = (workSheet.Cells[rowIterator, 10].Value == null) ? null : workSheet.Cells[rowIterator, 10].Value.ToString().Trim();
                                        savediem.QuaMon = (workSheet.Cells[rowIterator, 11].Value == null || workSheet.Cells[rowIterator, 11].Value.ToString().Trim() != "x") ? false : true;
                                        savediem.LichSu = (Session["LuuLichSu"] as LichSuUpLoad).ID;
                                        if (mssvmoi != savediem.MSSV.ToLower())
                                        {
                                            dem++;
                                            mssvmoi = savediem.MSSV.ToLower();
                                        }
                                       (Session["LuuDiem"] as List<DiemHocPhan>).Add(savediem);
                                    }
                                    catch (Exception ex)
                                    {
                                        TempData["Alert"] = "Lỗi, vui lòng thử lại!! " + ex.Message;
                                    }
                                }
                            });
                            t2.Start();
                        }
                        else if (noOfCol != 11)
                        {
                            DanhSachLoi += "<p> File bị sai định dạng, vui lòng thử lại!!</p>";
                        }
                        else
                        {
                            DanhSachLoi += "<p> File bị trống, vui lòng thử lại!!</p>";
                        }
                    }
                }
                if (DanhSachLoi != "")
                {
                    TempData["Alert"] = DanhSachLoi;
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["Alert"] = "File bị trống, vui lòng thử lại!!";
                return RedirectToAction("Index");
            }
            Session["File"] = file;
            string thongbao = "<table class='table table-hover mb-0'>";
            thongbao += "<tr>";
            thongbao += "<td>Số lượng sinh viên: " + dem + "</td>";
            thongbao += "</tr>";
            thongbao += "<tr>";
            thongbao += "<td>Số lượng điểm được lưu: " + (Session["LuuDiem"] as List<DiemHocPhan>).Count + "</td>";
            thongbao += "</tr>";
            thongbao += "</table>";
            Session["ThongBao"] = thongbao;
            return Redirect("Index");
        }
        public ActionResult TaiLenDiem()
        {
            db.Configuration.AutoDetectChangesEnabled = false;
            db.Configuration.ValidateOnSaveEnabled = false;
            var listDiem = Session["LuuDiem"] as List<DiemHocPhan>;
            var LichSu = Session["LuuLichSu"] as LichSuUpLoad;
            var File = Session["File"] as HttpPostedFileBase;
            db.LichSuUpLoads.Add(LichSu);
            db.SaveChanges();
            foreach (var item in listDiem)
            {
                item.LichSu = db.LichSuUpLoads.OrderByDescending(s => s.ID).First().ID;
                db.DiemHocPhans.Add(item);
            }
            db.SaveChanges();
            System.IO.File.Move(Server.MapPath(UPLOAD_PATH) + "0.xlsx", Server.MapPath(UPLOAD_PATH) + LichSu.ID + ".xlsx");
            Session["ThongBao"] = null;
            return Redirect(Request.UrlReferrer.ToString());
        }
        private const string UPLOAD_PATH = "~/FileUpLoad/";
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

        // GET: Faculty/DiemHocPhans
        public ActionResult Index()
        {
            var diemHocPhans = db.LichSuUpLoads.ToList();
            return View(diemHocPhans.ToList());
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
