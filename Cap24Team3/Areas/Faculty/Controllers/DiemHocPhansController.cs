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
        public ActionResult ThongKe(FormCollection formCollection)
        {
            var LuuDiem = new List<DiemHocPhan>();
            var LuuLichSu = new LichSuUpLoad();
            HttpPostedFileBase file = Request.Files["UploadedFile"];
            if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
            {
                var listError = KiemTraFile(file);
                if (listError != "")
                {
                    TempData["Alert"] = listError;
                    return Redirect(Request.UrlReferrer.ToString());
                }
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

                        LuuLichSu.ThoiGian = DateTime.Now;
                        LuuLichSu.NguoiUpload = User.Identity.Name;
                        var ListLS = db.LichSuUpLoads.ToList();
                        if (ListLS.Count != 0)
                        {
                            LuuLichSu.ID = ListLS.OrderByDescending(s => s.ID).First().ID + 1;
                        }
                        else
                        {
                            LuuLichSu.ID = 1;
                        }
                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {
                            DiemHocPhan savediem = new DiemHocPhan();
                            if (workSheet.Cells[rowIterator, 1].Value != null)
                            {
                                savediem.MSSV = workSheet.Cells[rowIterator, 1].Value.ToString();
                            }
                            if (workSheet.Cells[rowIterator, 5].Value != null)
                            {
                                var HocKy = int.Parse(workSheet.Cells[rowIterator, 5].Value.ToString().Substring(0, 3));
                                var mssv = workSheet.Cells[rowIterator, 1].Value.ToString();
                                var sinhvien = db.SinhViens.FirstOrDefault(s => s.MSSV == mssv);
                                var listhk = new List<HocKyDaoTao>();
                                var hockysv = 0;
                                var hockyhp = 0;
                                foreach (var item in db.HocKyDaoTaos.ToList())
                                {
                                    if (item.HocKy == sinhvien.HocKyBatDau)
                                        hockysv = item.STT;
                                    if (item.HocKy == HocKy)
                                        hockyhp = item.STT;
                                }
                                savediem.HocKy = hockyhp - hockysv + 1;
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
                            savediem.LichSu = LuuLichSu.ID;
                            LuuDiem.Add(savediem);
                        }
                    }
                }
            }
            else
            {
                TempData["Alert"] = "File bị trống, vui lòng thử lại!!";
            }

            Session["LuuDiem"] = LuuDiem;
            Session["LuuLichSu"] = LuuLichSu;
            Session["File"] = file;
            var ListSinhVien = new List<string>();
            foreach (var item in LuuDiem)
            {
                if (!CheckTonTai(item.MSSV, ListSinhVien))
                    ListSinhVien.Add(item.MSSV);
            }
            var ListHocPhan = new List<string>();
            foreach (var item in LuuDiem)
            {
                if (!CheckTonTai(item.HocPhan.ToLower(), ListHocPhan))
                    ListHocPhan.Add(item.HocPhan.ToLower());
            }
            string thongbao = "<table class='table table-hover mb-0'>";
            thongbao += "<tr>";
            thongbao += "<td>Số lượng sinh viên: " + ListSinhVien.Count + "</td>";
            thongbao += "</tr>";
            thongbao += "<tr>";
            thongbao += "<td>Số lượng học phần: " + ListHocPhan.Count + "</td>";
            thongbao += "</tr>";
            thongbao += "<tr>";
            thongbao += "<td>Số lượng điểm được lưu: " + LuuDiem.Count + "</td>";
            thongbao += "</tr>";
            thongbao += "</table>";
            Session["ThongBao"] = thongbao;
            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult TaiLenDiem()
        {
            var listDiem = Session["LuuDiem"] as List<DiemHocPhan>;
            var LichSu = Session["LuuLichSu"] as LichSuUpLoad;
            var File = Session["File"] as HttpPostedFileBase;
            db.LichSuUpLoads.Add(LichSu);
            foreach (var item in listDiem)
                db.DiemHocPhans.Add(item);
            db.Configuration.AutoDetectChangesEnabled = false;
            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();
            Session["ThongBao"] = null;
            return Redirect(Request.UrlReferrer.ToString());
        }
        [HttpPost]
        public ActionResult UploadDSDiem(FormCollection formCollection)
        {
            DiemHocPhan DSDiem = new DiemHocPhan();
            HttpPostedFileBase file = Request.Files["UploadedFile"];
            if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
            {
                var listError = KiemTraFile(file);
                if (listError != "")
                {
                    TempData["Alert"] = listError;
                    return Redirect(Request.UrlReferrer.ToString());
                }
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

                        var lichsu = new LichSuUpLoad();
                        lichsu.ThoiGian = DateTime.Now;
                        lichsu.NguoiUpload = User.Identity.Name;
                        var ListLS = db.LichSuUpLoads.ToList();
                        if (ListLS.Count != 0)
                        {
                            lichsu.ID = ListLS.OrderByDescending(s => s.ID).First().ID + 1;
                        }
                        else
                        {
                            lichsu.ID = 1;
                        }
                        db.LichSuUpLoads.Add(lichsu);
                        db.SaveChanges();
                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {
                            DiemHocPhan savediem = new DiemHocPhan();
                            if (workSheet.Cells[rowIterator, 1].Value != null)
                            {
                                savediem.MSSV = workSheet.Cells[rowIterator, 1].Value.ToString();
                            }
                            if (workSheet.Cells[rowIterator, 5].Value != null)
                            {
                                var HocKy = int.Parse(workSheet.Cells[rowIterator, 5].Value.ToString().Substring(0, 3));
                                var mssv = workSheet.Cells[rowIterator, 1].Value.ToString();
                                var sinhvien = db.SinhViens.FirstOrDefault(s => s.MSSV == mssv);
                                var listhk = new List<HocKyDaoTao>();
                                var hockysv = 0;
                                var hockyhp = 0;
                                foreach (var item in db.HocKyDaoTaos.ToList())
                                {
                                    if (item.HocKy == sinhvien.HocKyBatDau)
                                        hockysv = item.STT;
                                    if (item.HocKy == HocKy)
                                        hockyhp = item.STT;
                                }
                                savediem.HocKy = hockyhp - hockysv + 1;
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
                            savediem.LichSu = lichsu.ID;
                            db.DiemHocPhans.Add(savediem);
                        }
                    }
                }
            }
            else
            {
                TempData["Alert"] = "File bị trống, vui lòng thử lại!!";
            }

            db.Configuration.AutoDetectChangesEnabled = false;
            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();
            return Redirect(Request.UrlReferrer.ToString());
        }

        //Kiểm tra lỗi của file excel import vào.
        public string KiemTraFile(HttpPostedFileBase file)
        {
            string DanhSachLoi = "";
            byte[] fileBytes = new byte[file.ContentLength];
            var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
            using (var package = new ExcelPackage(file.InputStream))
            {
                var currentSheet = package.Workbook.Worksheets;
                var workSheet = currentSheet.First();
                if (workSheet.Dimension != null)
                {
                    var noOfRow = workSheet.Dimension.End.Row;
                    var noOfCol = workSheet.Dimension.End.Column;
                    if (noOfCol == 11 && noOfRow > 1)
                    {

                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {
                            DiemHocPhan savediem = new DiemHocPhan();
                            if (workSheet.Cells[rowIterator, 1].Value != null)
                            {
                                savediem.MSSV = workSheet.Cells[rowIterator, 1].Value.ToString();

                                if (savediem.MSSV.Length > 20)
                                {
                                    DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột A: Dữ liệu không được quá 20 ký tự, số ký tự của " + savediem.MSSV + " là " + savediem.MSSV.Length + "<p>";
                                }
                                if (savediem.MSSV.Replace(" ", string.Empty) == null)
                                {
                                    DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột A: Dữ liệu không được để trống <p>";
                                }


                            }
                            else
                            {
                                DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột A: Dữ liệu không được để trống <p>";
                            }
                            if (workSheet.Cells[rowIterator, 3].Value != null)
                            {
                                savediem.HocKy = int.Parse(workSheet.Cells[rowIterator, 3].Value.ToString().Replace("HK", string.Empty));
                                if (savediem.HocKy <= 0)
                                {
                                    DanhSachLoi += "<p> Học Kì đào tạo phải là số nguyên dương lớn hơn 0, khóa đào tạo bạn nhập là: " + savediem.HocKy + "</p>";
                                }
                                if (savediem.HocKy == null)
                                {
                                    DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột C: Dữ liệu không được để trống <p>";
                                }

                            }

                            if (workSheet.Cells[rowIterator, 4].Value != null)
                            {
                                savediem.HocPhan = workSheet.Cells[rowIterator, 4].Value.ToString();
                                if (savediem.HocPhan.Length > 7)
                                {
                                    DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột D: Dữ liệu không được quá 7 ký tự, số ký tự của " + savediem.HocPhan + " là " + savediem.HocPhan.Length + "<p>";
                                }
                                if (savediem.HocPhan.Replace(" ", string.Empty) == null)
                                {
                                    DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột D: Dữ liệu không được để trống <p>";
                                }


                            }
                            if (workSheet.Cells[rowIterator, 6].Value != null)
                            {
                                savediem.TenHocPhan = workSheet.Cells[rowIterator, 6].Value.ToString();
                                if (savediem.TenHocPhan.Replace(" ", string.Empty) == null)
                                {
                                    DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột F: Dữ liệu không được để trống <p>";
                                }
                            }

                            if (workSheet.Cells[rowIterator, 7].Value != null)
                            {
                                savediem.SoTinChi = int.Parse(workSheet.Cells[rowIterator, 7].Value.ToString().Trim());

                                if (savediem.SoTinChi == null)
                                {
                                    DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột G: Dữ liệu không được để trống <p>";
                                }
                                if (savediem.SoTinChi < 0)
                                {
                                    DanhSachLoi += "<p> Số tín chỉ phải là số nguyên dương lớn hơn 0, số tín chỉ bạn nhập là: " + savediem.SoTinChi + "</p>";
                                }

                            }

                            if (workSheet.Cells[rowIterator, 8].Value != null)
                            {
                                savediem.Diem10 = workSheet.Cells[rowIterator, 8].Value.ToString();
                                if (savediem.Diem10.Length > 3)
                                {
                                    DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột H: Dữ liệu không được quá 3 ký tự, số ký tự của " + savediem.Diem10 + " là " + savediem.Diem10.Length + "<p>";
                                }

                            }
                            if (workSheet.Cells[rowIterator, 9].Value != null)
                            {
                                savediem.Diem4 = workSheet.Cells[rowIterator, 9].Value.ToString();
                                if (savediem.Diem4.Length > 4)
                                {
                                    DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột I: Dữ liệu không được quá 4 ký tự, số ký tự của " + savediem.Diem4 + " là " + savediem.Diem4.Length + "<p>";
                                }

                            }
                            if (workSheet.Cells[rowIterator, 10].Value != null)
                            {
                                savediem.DiemChu = workSheet.Cells[rowIterator, 10].Value.ToString();
                                if (savediem.DiemChu.Length > 2)
                                {
                                    DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột J: Dữ liệu không được quá 2 ký tự, số ký tự của " + savediem.DiemChu + " là " + savediem.DiemChu.Length + "<p>";
                                }

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
            return DanhSachLoi;
        }

        //Check
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
