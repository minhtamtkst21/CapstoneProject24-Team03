using Cap24Team3.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Cap24Team3.Areas.Faculty.Controllers
{
    [Authorize(Roles = "BCN Khoa")]
    public class ChuongTrinhDaoTaoController : Controller
    {
        private Cap24 db = new Cap24();
        public string KiemTraHK(int HK)
        {
            string ListLoi = "";
            if (HK < 100 || HK >= 1000)
            {
                ListLoi += "<p> Học kỳ phải có 3 chữ số </p>";
            }
            var listDBHK = db.HocKyDaoTaos.ToList();
            var listHK = new List<string>();
            foreach (var item in listDBHK)
            {
                listHK.Add(item.HocKy.ToString());
            }
            if (CheckTonTai(HK.ToString(), listHK))
            {
                ListLoi += "<p> Học kỳ đào tạo đã tồn tại trong hệ thống, vui lòng thử lại!</p>";
            }
            return ListLoi;
        }
        public ActionResult ListHocKyDT()
        {
            return View(db.HocKyDaoTaos.ToList());
        }

        public ActionResult TaoMoiHocKy()
        {
            return View();
        }
        // POST: Faculty/Nganh/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TaoMoiHocKy(HocKyDaoTao hocKyDaoTao)
        {
            if (ModelState.IsValid)
            {
                var ListLoi = KiemTraHK(hocKyDaoTao.HocKy);
                if (ListLoi != "")
                {
                    TempData["Alert"] = ListLoi;
                    return RedirectToAction("TaoMoiHocKy");
                }
                db.HocKyDaoTaos.Add(hocKyDaoTao);
                db.SaveChanges();
                TempData["ThongBao"] = "Tạo mới học kỳ thành công";
                return RedirectToAction("ListHocKyDT");
            }
            return View(hocKyDaoTao);
        }

        public ActionResult SuaHocKyDT(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HocKyDaoTao hocKyDaoTao = db.HocKyDaoTaos.Find(id);
            if (hocKyDaoTao == null)
            {
                return HttpNotFound();
            }
            return View(hocKyDaoTao);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuaHocKyDT(HocKyDaoTao hocKyDaoTao)
        {
            if (ModelState.IsValid)
            {
                var ListLoi = KiemTraHK(hocKyDaoTao.HocKy);
                if (ListLoi != "")
                {
                    TempData["Alert"] = ListLoi;
                    return RedirectToAction("SuaHocKyDT", new { id = hocKyDaoTao.ID });
                }
                db.Entry(hocKyDaoTao).State = EntityState.Modified;
                db.SaveChanges();
                TempData["ThongBao"] = "Sửa học kỳ thành công";
                return RedirectToAction("ListHocKyDT");
            }
            return View(hocKyDaoTao);
        }

        public ActionResult XoaHocKyDT(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HocKyDaoTao hocKyDaoTao = db.HocKyDaoTaos.Find(id);
            if (hocKyDaoTao == null)
            {
                return HttpNotFound();
            }
            return View(hocKyDaoTao);
        }

        // POST: Faculty/NganhDaoTaos/Delete/5
        [HttpPost, ActionName("XoaHocKyDT")]
        [ValidateAntiForgeryToken]
        public ActionResult XacNhanXoaHocKyDT(int id)
        {
            HocKyDaoTao hocKyDaoTao = db.HocKyDaoTaos.Find(id);
            db.HocKyDaoTaos.Remove(hocKyDaoTao);
            db.SaveChanges();
            TempData["ThongBao"] = "Xóa học kỳ thành công";
            return RedirectToAction("ListHocKyDT");
        }

        public string KiemTraKhoa(int khoa)
        {
            string ListLoi = "";
            if (khoa <= 0)
            {
                ListLoi += "<p> Khóa đào tạo phải là số nguyên dương lớn hơn 0, khóa đào tạo bạn nhập là: " + khoa + "</p>";
            }
            var listDBKhoa = db.KhoaDaoTaos.ToList();
            var listKhoa = new List<string>();
            foreach (var item in listDBKhoa)
            {
                listKhoa.Add(item.Khoa.ToString());
            }
            if (CheckTonTai(khoa.ToString(), listKhoa))
            {
                ListLoi += "<p> Khóa đào tạo đã tồn tại trong hệ thống, vui lòng thử lại!</p>";
            }
            return ListLoi;
        }
        public ActionResult ListKhoaDT()
        {
            return View(db.KhoaDaoTaos.ToList());
        }

        public ActionResult TaoMoiKhoa()
        {
            return View();
        }
        // POST: Faculty/Nganh/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TaoMoiKhoa(KhoaDaoTao khoaDaoTao)
        {
            if (ModelState.IsValid)
            {
                var ListLoi = KiemTraKhoa(khoaDaoTao.Khoa);
                if (ListLoi != "")
                {
                    TempData["Alert"] = ListLoi;
                    return RedirectToAction("TaoMoiKhoa");
                }
                db.KhoaDaoTaos.Add(khoaDaoTao);
                db.SaveChanges();
                TempData["ThongBao"] = "Tạo mới khóa thành công";
                return RedirectToAction("ListKhoaDT");
            }
            return View(khoaDaoTao);
        }

        public ActionResult SuaKhoaDT(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhoaDaoTao khoaDaoTao = db.KhoaDaoTaos.Find(id);
            if (khoaDaoTao == null)
            {
                return HttpNotFound();
            }
            return View(khoaDaoTao);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuaKhoaDT(KhoaDaoTao khoaDaoTao)
        {
            if (ModelState.IsValid)
            {
                var ListLoi = KiemTraKhoa(khoaDaoTao.Khoa);
                if (ListLoi != "")
                {
                    TempData["Alert"] = ListLoi;
                    return RedirectToAction("SuaKhoaDT", new { id = khoaDaoTao.ID });
                }
                db.Entry(khoaDaoTao).State = EntityState.Modified;
                db.SaveChanges();
                TempData["ThongBao"] = "Sửa khóa thành công";
                return RedirectToAction("ListKhoaDT");
            }
            return View(khoaDaoTao);
        }

        public ActionResult XoaKhoaDT(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhoaDaoTao khoaDaoTao = db.KhoaDaoTaos.Find(id);
            if (khoaDaoTao == null)
            {
                return HttpNotFound();
            }
            return View(khoaDaoTao);
        }

        // POST: Faculty/NganhDaoTaos/Delete/5
        [HttpPost, ActionName("XoaKhoaDT")]
        [ValidateAntiForgeryToken]
        public ActionResult XacNhanXoaKhoaDT(int id)
        {
            KhoaDaoTao khoaDaoTao = db.KhoaDaoTaos.Find(id);
            db.KhoaDaoTaos.Remove(khoaDaoTao);
            db.SaveChanges();
            TempData["ThongBao"] = "Xóa khóa thành công";
            return RedirectToAction("ListKhoaDT");
        }

        public string KiemTraNganh(string nganh)
        {
            string ListLoi = "";

            if (nganh.Length > 100)
            {
                ListLoi += "<p> Ngành đào tạo không được lớn hơn 100 ký tự, số ký tự của ngành đào tạo bạn nhập là: " + nganh.Length + "</p>";
            }
            var listDBNganh = db.NganhDaoTaos.ToList();
            var listNganh = new List<string>();
            foreach (var item in listDBNganh)
            {
                listNganh.Add(item.Nganh);
            }
            if (CheckTonTai(nganh, listNganh))
            {
                ListLoi += "<p> Ngành đào tạo đã tồn tại trong hệ thống, vui lòng thử lại!</p>";
            }
            return ListLoi;
        }

        public ActionResult ListNganhDT()
        {
            return View(db.NganhDaoTaos.ToList());
        }

        public ActionResult TaoMoiNganh()
        {
            return View();
        }
        // POST: Faculty/Nganh/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TaoMoiNganh(NganhDaoTao nganhDaoTao)
        {
            if (ModelState.IsValid)
            {
                var ListLoi = KiemTraNganh(nganhDaoTao.Nganh);
                if (ListLoi != "")
                {
                    TempData["Alert"] = ListLoi;
                    return RedirectToAction("TaoMoiNganh");
                }
                db.NganhDaoTaos.Add(nganhDaoTao);
                db.SaveChanges();
                TempData["ThongBao"] = "Tạo mới ngành thành công";
                return RedirectToAction("ListNganhDT");
            }
            return View(nganhDaoTao);
        }

        public ActionResult SuaNganhDT(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NganhDaoTao nganhDaoTao = db.NganhDaoTaos.Find(id);
            if (nganhDaoTao == null)
            {
                return HttpNotFound();
            }
            return View(nganhDaoTao);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuaNganhDT(NganhDaoTao nganhDaoTao)
        {
            if (ModelState.IsValid)
            {
                var ListLoi = KiemTraNganh(nganhDaoTao.Nganh);
                if (ListLoi != "")
                {
                    TempData["Alert"] = ListLoi;
                    return RedirectToAction("SuaNganhDT", new { id = nganhDaoTao.ID });
                }
                db.Entry(nganhDaoTao).State = EntityState.Modified;
                db.SaveChanges();
                TempData["ThongBao"] = "Sửa ngành thành công";
                return RedirectToAction("ListNganhDT");
            }
            return View(nganhDaoTao);
        }

        public ActionResult XoaNganhDT(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NganhDaoTao nganhDaoTao = db.NganhDaoTaos.Find(id);
            if (nganhDaoTao == null)
            {
                return HttpNotFound();
            }
            return View(nganhDaoTao);
        }

        // POST: Faculty/NganhDaoTaos/Delete/5
        [HttpPost, ActionName("XoaNganhDT")]
        [ValidateAntiForgeryToken]
        public ActionResult XacNhanXoaNganhDT(int id)
        {
            NganhDaoTao nganhDaoTao = db.NganhDaoTaos.Find(id);
            db.NganhDaoTaos.Remove(nganhDaoTao);
            db.SaveChanges();
            TempData["ThongBao"] = "Xóa ngành thành công";
            return RedirectToAction("ListNganhDT");
        }

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
                    var noOfCol = workSheet.Dimension.End.Column;
                    var noOfRow = workSheet.Dimension.End.Row;
                    if (noOfCol == 9 && noOfRow > 1)
                    {
                        var listmahp = new List<string>();
                        var listtenhp = new List<string>();
                        var listhp = new List<string>();
                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {
                            for (int i = 2; i <= 3; i++)
                            {
                                if (workSheet.Cells[rowIterator, i].Value != null)
                                {
                                    var hocphan = workSheet.Cells[rowIterator, i].Value.ToString().Replace(" ", string.Empty);
                                    listhp.Add(hocphan);
                                }
                            }
                        }
                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {
                            if (workSheet.Cells[rowIterator, 2].Value != null)
                            {
                                var item = workSheet.Cells[rowIterator, 2].Value.ToString();
                                if (workSheet.Cells[rowIterator, 1].Value != null)
                                {
                                    var stt = workSheet.Cells[rowIterator, 1].Value.ToString();
                                    if (int.TryParse(stt, out int i))
                                    {
                                        if (CheckTonTai(item, listmahp))
                                        {
                                            DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột B: Dữ liệu bị trùng, dữ liệu trùng: " + item + "</p>";
                                        }
                                        if (item.Length > 20)
                                        {
                                            DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột B: Độ dài không quá 20 ký tự, độ dài ký tự: " + item + " là: " + item.Length + "</p>";
                                        }
                                        listmahp.Add(item);
                                    }
                                    else
                                    {
                                        if (stt.Length != 2)
                                        {
                                            DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột A: Mã khối kiến thức bị sai, độ dài ký tự: " + stt + " là: " + stt.Length + "</p>";
                                        }
                                        if (workSheet.Cells[rowIterator, 2].Value != null)
                                        {
                                            var tenktt = workSheet.Cells[rowIterator, 2].Value.ToString();
                                            if (tenktt.Length > 200)
                                            {
                                                DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột B: Tên khối kiến thức không được quá 200 ký tự, độ dài ký tự: " + stt + " là: " + stt.Length + "</p>";
                                            }
                                            if (tenktt.Replace(" ", string.Empty) == null)
                                            {
                                                DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột B: Tên kiến thức bắt buộc phải có, vui lòng thử lại!!</p>";
                                            }
                                        }
                                        else
                                        {
                                            DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột B: Tên kiến thức bắt buộc phải có, vui lòng thử lại!!</p>";
                                        }
                                        listmahp.Clear();
                                    }
                                }
                            }
                            else
                            {
                                if (workSheet.Cells[rowIterator, 1].Value != null)
                                {
                                    var stt = workSheet.Cells[rowIterator, 1].Value.ToString();
                                    if (!int.TryParse(stt, out int i))
                                    {
                                        DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột B: Tên kiến thức bắt buộc phải có, vui lòng thử lại!!</p>";
                                    }
                                }
                            }
                            if (workSheet.Cells[rowIterator, 3].Value != null)
                            {
                                var item = workSheet.Cells[rowIterator, 3].Value.ToString();
                                if (workSheet.Cells[rowIterator, 1].Value != null)
                                {
                                    var stt = workSheet.Cells[rowIterator, 1].Value.ToString();
                                    if (int.TryParse(stt, out int i))
                                    {
                                        if (item != "Chuyên ngành tự chọn")
                                        {
                                            if (CheckTonTai(item, listtenhp))
                                            {
                                                DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột C: Dữ liệu bị trùng, dữ liệu trùng: " + item + "</p>";
                                            }
                                            if (item.Length > 200)
                                            {
                                                DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột B: Độ dài không quá 200 ký tự, độ dài ký tự: " + item + " là: " + item.Length + "</p>";
                                            }
                                            listtenhp.Add(item);
                                        }
                                    }
                                    else
                                    {
                                        listtenhp.Clear();
                                    }
                                }
                            }
                            else if (workSheet.Cells[rowIterator, 1].Value != null)
                            {
                                var stt = workSheet.Cells[rowIterator, 1].Value.ToString();
                                if (int.TryParse(stt, out int i))
                                {
                                    if (workSheet.Cells[rowIterator, 3].Value == null)
                                    {
                                        DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột C: Tên học phần bắt buộc phải có!</p>";
                                    }
                                    else if (workSheet.Cells[rowIterator, 3].Value.ToString().Replace(" ", string.Empty) == null)
                                    {
                                        DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột C: Tên học phần bắt buộc phải có!</p>";
                                    }
                                }
                            }
                            if (workSheet.Cells[rowIterator, 5].Value != null)
                            {
                                var item = workSheet.Cells[rowIterator, 5].Value.ToString().Replace(" ", string.Empty);
                                if (item == "BB" || item == "TC") { }
                                else
                                {
                                    DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột E: Học phần chỉ chứa ký tự 'BB' hoặc 'TC', dữ liệu bị sai:" + item + "</p>";
                                }
                            }
                            if (workSheet.Cells[rowIterator, 6].Value != null)
                            {
                                var rangbuoc = workSheet.Cells[rowIterator, 6].Value.ToString().Replace(" ", string.Empty).Split(',');
                                foreach (var element in rangbuoc)
                                    if (!CheckHamCon(element, listhp))
                                    {
                                        DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột F: Học phần không tồn tại trong danh sách, học phần bị sai: " + element + "</p>";
                                    }
                            }
                            if (workSheet.Cells[rowIterator, 7].Value != null)
                            {
                                var rangbuoc = workSheet.Cells[rowIterator, 7].Value.ToString().Replace(" ", string.Empty).Split(',');
                                foreach (var element in rangbuoc)
                                    if (!CheckHamCon(element, listhp))
                                    {
                                        DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột G: Học phần không tồn tại trong danh sách, học phần bị sai: " + element + "</p>";
                                    }
                            }
                            if (workSheet.Cells[rowIterator, 8].Value != null)
                            {
                                var rangbuoc = workSheet.Cells[rowIterator, 8].Value.ToString().Replace(" ", string.Empty).Split(',');
                                foreach (var element in rangbuoc)
                                    if (!CheckHamCon(element, listhp))
                                    {
                                        DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột H: Học phần không tồn tại trong danh sách, học phần bị sai: " + element + "</p>";
                                    }
                            }
                            if (workSheet.Cells[rowIterator, 9].Value != null)
                            {
                                if (int.TryParse(workSheet.Cells[rowIterator, 9].Value.ToString(), out int i))
                                {
                                    var item = int.Parse(workSheet.Cells[rowIterator, 9].Value.ToString());
                                    if (item <= 0)
                                    {
                                        DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột I: Học kỳ phải là số nguyên dương, học kỳ bị sai:" + item + "</p>";
                                    }
                                }
                                else
                                {
                                    var item = workSheet.Cells[rowIterator, 9].Value.ToString();
                                    DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột I: Học kỳ phải là số nguyên dương, học kỳ bị sai:" + item + "</p>";
                                }
                            }
                        }
                    }
                    else if (noOfCol != 9)
                    {
                        DanhSachLoi += "<p> File bị sai định dạng, vui lòng thử lại!!</p>";
                    }
                    else
                    {
                        DanhSachLoi += "<p> File bị trống, vui lòng thử lại!!</p>";
                    }
                }
                else
                {
                    DanhSachLoi += "<p> File bị trống, vui lòng thử lại!!</p>";
                }
            }
            return DanhSachLoi;
        }
        public bool CheckHamCon(string element, List<string> list)
        {
            foreach (var item in list)
            {
                if (item.Contains(element))
                {
                    return true;
                }
            }
            return false;
        }
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
        public ActionResult ListCTDaoTao()
        {
            var ListChuongTrinhDaoTao = db.ChuongTrinhDaoTaos.OrderByDescending(h => h.KhoaDaoTao.Khoa).Include(h => h.NganhDaoTao).Include(h => h.KhoaDaoTao).Include(h => h.HocKyDaoTao);
            ViewData["NganhDaoTao"] = db.NganhDaoTaos.ToList();
            ViewData["KhoaDaoTao"] = db.KhoaDaoTaos.ToList();
            ViewData["HocKyDaoTao"] = db.HocKyDaoTaos.ToList();
            return View(ListChuongTrinhDaoTao.ToList());
        }
        [HttpPost]
        public ActionResult XemTruocThongKe(FormCollection formCollection, string Nganh, string Khoa, string HocKy, string dieuchinh)
        {
            if (Nganh is null)
            {
                return RedirectToAction("ListCTDaoTao");
            }

            if (Khoa is null)
            {
                return RedirectToAction("ListCTDaoTao");
            }

            if (HocKy is null)
            {
                return RedirectToAction("ListCTDaoTao");
            }
            try
            {
                var LuuCtdt = new ChuongTrinhDaoTao();
                var LuuKhoikienthuc = new List<KhoiKienThuc>();
                var LuuHocPhan = new List<HocPhanDaoTao>();
                var LuuRangBuoc = new List<RangBuocHocPhan>();
                var ctdt = db.ChuongTrinhDaoTaos.Where(s => s.KhoaDaoTao.Khoa.ToString() == Khoa.ToString()).FirstOrDefault(s => s.NganhDaoTao.Nganh == Nganh);
                if (Request != null && (ctdt == null || dieuchinh == "1"))
                {
                    ChuongTrinhDaoTao chuongTrinhDaoTao = new ChuongTrinhDaoTao();
                    chuongTrinhDaoTao.ID_Khoa = db.KhoaDaoTaos.ToList().FirstOrDefault(s => s.Khoa.ToString() == Khoa.ToString()).ID;
                    chuongTrinhDaoTao.ID_Nganh = db.NganhDaoTaos.ToList().FirstOrDefault(s => s.Nganh == Nganh).ID;
                    chuongTrinhDaoTao.ID_HocKyBatDau = db.HocKyDaoTaos.ToList().FirstOrDefault(s => s.HocKy.ToString() == HocKy).ID;
                    HttpPostedFileBase file = Request.Files["UploadedFile"];
                    if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                    {
                        var listError = KiemTraFile(file);
                        if (listError != "")
                        {
                            TempData["Alert"] = listError;
                            return RedirectToAction("ListCTDaoTao");
                        }
                        LuuCtdt = chuongTrinhDaoTao;
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
                                if (noOfCol == 9 && noOfRow > 1)
                                {
                                    string KHOIKIENTHUC = "";
                                    string HPBATBUOC = "";
                                    string khoikienthuc = "";
                                    for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                                    {
                                        if (workSheet.Cells[rowIterator, 1].Value != null)
                                        {
                                            khoikienthuc = workSheet.Cells[rowIterator, 1].Value.ToString();
                                        }
                                        if (int.TryParse(khoikienthuc, out int stt))
                                        {
                                            HocPhanDaoTao hocPhanDaoTao = new HocPhanDaoTao();
                                            if (workSheet.Cells[rowIterator, 2].Value != null)
                                            {
                                                hocPhanDaoTao.MaHocPhan = workSheet.Cells[rowIterator, 2].Value.ToString();
                                            }
                                            if (workSheet.Cells[rowIterator, 3].Value != null)
                                            {
                                                hocPhanDaoTao.TenHocPhan = workSheet.Cells[rowIterator, 3].Value.ToString();
                                            }
                                            if (workSheet.Cells[rowIterator, 4].Value != null)
                                            {
                                                hocPhanDaoTao.SoTinChi = workSheet.Cells[rowIterator, 4].Value.ToString();
                                            }
                                            if (workSheet.Cells[rowIterator, 5].Value != null)
                                            {
                                                var LoaiHP = workSheet.Cells[rowIterator, 5].Value.ToString();
                                                if (LoaiHP == "TC")
                                                {
                                                    hocPhanDaoTao.HocPhanDaoTao2 = LuuHocPhan.FirstOrDefault(s => s.TenHocPhan == HPBATBUOC);
                                                }
                                                else
                                                {
                                                    HPBATBUOC = workSheet.Cells[rowIterator, 3].Value.ToString();
                                                }
                                            }
                                            else
                                            {
                                                HPBATBUOC = workSheet.Cells[rowIterator, 3].Value.ToString();
                                            }
                                            if (workSheet.Cells[rowIterator, 9].Value != null)
                                            {
                                                hocPhanDaoTao.HocKy = int.Parse(workSheet.Cells[rowIterator, 9].Value.ToString());
                                            }
                                            hocPhanDaoTao.KhoiKienThuc = LuuKhoikienthuc.FirstOrDefault(s => s.TenKhoiKienThuc == KHOIKIENTHUC);
                                            LuuHocPhan.Add(hocPhanDaoTao);
                                        }
                                        else
                                        {
                                            KhoiKienThuc taoKKT = new KhoiKienThuc();
                                            taoKKT.MaKhoiKienThuc = khoikienthuc;
                                            taoKKT.TenKhoiKienThuc = workSheet.Cells[rowIterator, 2].Value.ToString();
                                            taoKKT.ChuongTrinhDaoTao = LuuCtdt;
                                            LuuKhoikienthuc.Add(taoKKT);
                                            KHOIKIENTHUC = taoKKT.TenKhoiKienThuc;
                                        }
                                    }
                                    for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                                    {
                                        if (workSheet.Cells[rowIterator, 6].Value != null)
                                        {
                                            RangBuocHocPhan rangBuocHocPhan = new RangBuocHocPhan();
                                            string HocPhanRangBuoc = workSheet.Cells[rowIterator, 6].Value.ToString();
                                            string HocPhan = workSheet.Cells[rowIterator, 3].Value.ToString();
                                            string[] ListHP = HocPhanRangBuoc.Split(new char[] { ',' });
                                            foreach (string HP in ListHP)
                                            {
                                                var mahocphan = LuuHocPhan.FirstOrDefault(s => s.MaHocPhan == HP);
                                                if (mahocphan == null)
                                                {
                                                    var tenhocphan = LuuHocPhan.FirstOrDefault(s => s.TenHocPhan.Contains(HP));
                                                    if (tenhocphan != null)
                                                    {
                                                        rangBuocHocPhan.HocPhanDaoTao = tenhocphan;
                                                        rangBuocHocPhan.HocPhanDaoTao1 = LuuHocPhan.FirstOrDefault(s => s.TenHocPhan == HocPhan);
                                                        rangBuocHocPhan.LoaiRangBuoc = "Tiên quyết";
                                                        LuuRangBuoc.Add(rangBuocHocPhan);
                                                    }
                                                }
                                                else
                                                {
                                                    rangBuocHocPhan.HocPhanDaoTao = mahocphan;
                                                    rangBuocHocPhan.HocPhanDaoTao1 = LuuHocPhan.FirstOrDefault(s => s.TenHocPhan == HocPhan);
                                                    rangBuocHocPhan.LoaiRangBuoc = "Tiên quyết";
                                                    LuuRangBuoc.Add(rangBuocHocPhan);
                                                }
                                            }
                                        }
                                        if (workSheet.Cells[rowIterator, 7].Value != null)
                                        {
                                            RangBuocHocPhan rangBuocHocPhan = new RangBuocHocPhan();
                                            string HocPhanRangBuoc = workSheet.Cells[rowIterator, 7].Value.ToString();
                                            string HocPhan = workSheet.Cells[rowIterator, 3].Value.ToString();
                                            string[] ListHP = HocPhanRangBuoc.Split(new char[] { ',' });
                                            foreach (string HP in ListHP)
                                            {
                                                var mahocphan = LuuHocPhan.FirstOrDefault(s => s.MaHocPhan == HP);
                                                if (mahocphan == null)
                                                {
                                                    var tenhocphan = LuuHocPhan.FirstOrDefault(s => s.TenHocPhan.Contains(HP));
                                                    if (tenhocphan != null)
                                                    {
                                                        rangBuocHocPhan.HocPhanDaoTao = tenhocphan;
                                                        rangBuocHocPhan.HocPhanDaoTao1 = LuuHocPhan.FirstOrDefault(s => s.TenHocPhan == HocPhan);
                                                        rangBuocHocPhan.LoaiRangBuoc = "Học trước";
                                                        LuuRangBuoc.Add(rangBuocHocPhan);
                                                    }
                                                }
                                                else
                                                {
                                                    rangBuocHocPhan.HocPhanDaoTao = mahocphan;
                                                    rangBuocHocPhan.HocPhanDaoTao1 = LuuHocPhan.FirstOrDefault(s => s.TenHocPhan == HocPhan);
                                                    rangBuocHocPhan.LoaiRangBuoc = "Học trước";
                                                    LuuRangBuoc.Add(rangBuocHocPhan);
                                                }
                                            }
                                        }
                                        if (workSheet.Cells[rowIterator, 8].Value != null)
                                        {
                                            RangBuocHocPhan rangBuocHocPhan = new RangBuocHocPhan();
                                            string HocPhanRangBuoc = workSheet.Cells[rowIterator, 8].Value.ToString();
                                            string HocPhan = workSheet.Cells[rowIterator, 3].Value.ToString();
                                            string[] ListHP = HocPhanRangBuoc.Split(new char[] { ',' });
                                            foreach (string HP in ListHP)
                                            {
                                                var mahocphan = LuuHocPhan.FirstOrDefault(s => s.MaHocPhan == HP);
                                                if (mahocphan == null)
                                                {
                                                    var tenhocphan = LuuHocPhan.FirstOrDefault(s => s.TenHocPhan.Contains(HP));
                                                    if (tenhocphan != null)
                                                    {
                                                        rangBuocHocPhan.HocPhanDaoTao = tenhocphan;
                                                        rangBuocHocPhan.HocPhanDaoTao1 = LuuHocPhan.FirstOrDefault(s => s.TenHocPhan == HocPhan);
                                                        rangBuocHocPhan.LoaiRangBuoc = "Song hành";
                                                        LuuRangBuoc.Add(rangBuocHocPhan);
                                                    }
                                                }
                                                else
                                                {
                                                    rangBuocHocPhan.HocPhanDaoTao = mahocphan;
                                                    rangBuocHocPhan.HocPhanDaoTao1 = LuuHocPhan.FirstOrDefault(s => s.TenHocPhan == HocPhan);
                                                    rangBuocHocPhan.LoaiRangBuoc = "Song hành";
                                                    LuuRangBuoc.Add(rangBuocHocPhan);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }
                    else
                    {
                        TempData["Alert"] = "File bị trống, vui lòng thử lại!!";
                        return Redirect(Request.UrlReferrer.ToString());
                    }
                }
                else
                {
                    TempData["Alert"] = "Chương trình đào tạo bị trùng, vui lòng thử lại!!";
                    return Redirect(Request.UrlReferrer.ToString());
                }
                Session["CTDT"] = LuuCtdt;
                Session["Khoikienthuc"] = LuuKhoikienthuc;
                Session["HocPhan"] = LuuHocPhan;
                Session["RangBuoc"] = LuuRangBuoc;
                string thongbao = "";
                int STT = 0;
                foreach (var khoikt in LuuKhoikienthuc)
                {
                    thongbao += "<table class='table table-hover mb-0'>";
                    thongbao += "<h4>" + khoikt.TenKhoiKienThuc + "</h4>";
                    thongbao += "<table class='table table-hover mb-0'><tr style='background-color:lightblue'>";
                    thongbao += "<th>STT</th>";
                    thongbao += "<th>Mã học phần</th>";
                    thongbao += "<th>Tên học phần</th>";
                    thongbao += "<th>Số tín chỉ</th>";
                    thongbao += "<th>BB/TC</th>";
                    thongbao += "<th>Tiên quyết</th>";
                    thongbao += "<th>Học trước</th>";
                    thongbao += "<th>Song hành</th>";
                    thongbao += "<th>Học kỳ</th></tr>";
                    foreach (var hocphan in LuuHocPhan)
                    {
                        if (hocphan.KhoiKienThuc.TenKhoiKienThuc == khoikt.TenKhoiKienThuc)
                        {
                            thongbao += "<tr>";
                            if (hocphan.HocPhanDaoTao2 == null)
                            {
                                STT++;
                                thongbao += "<td>" + STT + "</td>";
                            }
                            else
                            {
                                thongbao += "<td></td>";
                            }
                            thongbao += "<td>" + hocphan.MaHocPhan + "</td>";
                            thongbao += "<td>" + hocphan.TenHocPhan + "</td>";
                            thongbao += "<td>" + hocphan.SoTinChi + "</td>";
                            if (hocphan.HocPhanDaoTao2 == null)
                            {
                                thongbao += "<td>BB</td>";
                            }
                            else
                            {
                                thongbao += "<td>TC</td>";
                            }
                            thongbao += "<td>";
                            foreach (var rangbuoc in LuuRangBuoc)
                            {
                                if (rangbuoc.LoaiRangBuoc == "Tiên quyết" && rangbuoc.HocPhanDaoTao1 == hocphan)
                                {
                                    if (rangbuoc.HocPhanDaoTao.MaHocPhan != null)
                                    {
                                        thongbao += rangbuoc.HocPhanDaoTao.MaHocPhan;
                                    }
                                    else
                                    {
                                        thongbao += rangbuoc.HocPhanDaoTao.TenHocPhan;
                                    }
                                }
                            }
                            thongbao += "</td><td>";
                            foreach (var rangbuoc in LuuRangBuoc)
                            {
                                if (rangbuoc.LoaiRangBuoc == "Học trước" && rangbuoc.HocPhanDaoTao1 == hocphan)
                                {
                                    if (rangbuoc.HocPhanDaoTao.MaHocPhan != null)
                                    {
                                        thongbao += rangbuoc.HocPhanDaoTao.MaHocPhan;
                                    }
                                    else
                                    {
                                        thongbao += rangbuoc.HocPhanDaoTao.TenHocPhan;
                                    }
                                }
                            }
                            thongbao += "</td><td>";
                            foreach (var rangbuoc in LuuRangBuoc)
                            {
                                if (rangbuoc.LoaiRangBuoc == "Song hành" && rangbuoc.HocPhanDaoTao1 == hocphan)
                                {
                                    if (rangbuoc.HocPhanDaoTao.MaHocPhan != null)
                                    {
                                        thongbao += rangbuoc.HocPhanDaoTao.MaHocPhan;
                                    }
                                    else
                                    {
                                        thongbao += rangbuoc.HocPhanDaoTao.TenHocPhan;
                                    }
                                }
                            }
                            thongbao += "</td>";
                            thongbao += "<td>" + hocphan.HocKy + "</td>";
                            thongbao += "</tr>";
                        }
                    }
                    thongbao += "</table>";
                }
                Session["Thongbao"] = thongbao;
                return Redirect(Request.UrlReferrer.ToString());
            }
            catch
            {
                TempData["Alert"] = "File bị sai định dạng, vui lòng thử lại";
                return Redirect(Request.UrlReferrer.ToString());
            }
        }
        public ActionResult TaiLenCTDT()
        {
            db.Configuration.AutoDetectChangesEnabled = false;
            db.Configuration.ValidateOnSaveEnabled = false;
            var ctdt = Session["CTDT"] as ChuongTrinhDaoTao;
            var khoikienthuc = Session["Khoikienthuc"] as List<KhoiKienThuc>;
            var hocphan = Session["HocPhan"] as List<HocPhanDaoTao>;
            var rangbuoc = Session["RangBuoc"] as List<RangBuocHocPhan>;
            db.ChuongTrinhDaoTaos.Add(ctdt);
            db.SaveChanges();
            foreach (var item in khoikienthuc)
            {
                db.KhoiKienThucs.Add(item);
            }
            db.SaveChanges();
            foreach (var item in hocphan)
            {
                db.HocPhanDaoTaos.Add(item);
                db.SaveChanges();
            }
            foreach (var item in rangbuoc)
            {
                db.RangBuocHocPhans.Add(item);
            }
            db.SaveChanges();
            TempData["ThongBao"] = "Thêm mới thành công chương trình đào tạo";
            Session["Thongbao"] = null;
            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult SuaCTDT(int ID)
        {
            XoaCTDT(ID);
            db.Configuration.AutoDetectChangesEnabled = false;
            db.Configuration.ValidateOnSaveEnabled = false;
            var ctdt = Session["CTDT"] as ChuongTrinhDaoTao;
            var khoikienthuc = Session["Khoikienthuc"] as List<KhoiKienThuc>;
            var hocphan = Session["HocPhan"] as List<HocPhanDaoTao>;
            var rangbuoc = Session["RangBuoc"] as List<RangBuocHocPhan>;
            db.ChuongTrinhDaoTaos.Add(ctdt);
            db.SaveChanges();
            foreach (var item in khoikienthuc)
            {
                db.KhoiKienThucs.Add(item);
            }
            db.SaveChanges();
            foreach (var item in hocphan)
            {
                db.HocPhanDaoTaos.Add(item);
                db.SaveChanges();
            }
            foreach (var item in rangbuoc)
            {
                db.RangBuocHocPhans.Add(item);
            }
            db.SaveChanges();
            TempData["ThongBao"] = "Chỉnh sửa thành công chương trình đào tạo";
            Session["Thongbao"] = null;
            return Redirect(Request.UrlReferrer.ToString());
        }
        [HttpPost]
        public ActionResult UploadCTDaoTao(FormCollection formCollection, string Nganh, string Khoa, string HocKy)
        {
            if (Nganh is null)
            {
                return RedirectToAction("ListCTDaoTao");
            }

            if (Khoa is null)
            {
                return RedirectToAction("ListCTDaoTao");
            }

            if (HocKy is null)
            {
                return RedirectToAction("ListCTDaoTao");
            }
            try
            {
                var ctdt = db.ChuongTrinhDaoTaos.Where(s => s.KhoaDaoTao.Khoa.ToString() == Khoa.ToString()).FirstOrDefault(s => s.NganhDaoTao.Nganh == Nganh);
                if (Request != null && ctdt == null)
                {
                    ChuongTrinhDaoTao chuongTrinhDaoTao = new ChuongTrinhDaoTao();
                    chuongTrinhDaoTao.KhoaDaoTao = db.KhoaDaoTaos.ToList().FirstOrDefault(s => s.Khoa.ToString() == Khoa.ToString());
                    chuongTrinhDaoTao.NganhDaoTao = db.NganhDaoTaos.ToList().FirstOrDefault(s => s.Nganh == Nganh);
                    chuongTrinhDaoTao.HocKyDaoTao = db.HocKyDaoTaos.ToList().FirstOrDefault(s => s.HocKy.ToString() == HocKy);
                    HttpPostedFileBase file = Request.Files["UploadedFile"];
                    if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                    {
                        var listError = KiemTraFile(file);
                        if (listError != "")
                        {
                            TempData["Alert"] = listError;
                            return RedirectToAction("ListCTDaoTao");
                        }
                        db.ChuongTrinhDaoTaos.Add(chuongTrinhDaoTao);
                        db.SaveChanges();
                        var CHUONGTRINHDAOTAO = db.ChuongTrinhDaoTaos.Where(s => s.NganhDaoTao.Nganh == Nganh).FirstOrDefault(s => s.KhoaDaoTao.Khoa.ToString() == Khoa.ToString());
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
                                if (noOfCol == 9 && noOfRow > 1)
                                {
                                    string KHOIKIENTHUC = "";
                                    string HPBATBUOC = "";
                                    string khoikienthuc = "";
                                    for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                                    {
                                        if (workSheet.Cells[rowIterator, 1].Value != null)
                                        {
                                            khoikienthuc = workSheet.Cells[rowIterator, 1].Value.ToString();
                                        }
                                        if (int.TryParse(khoikienthuc, out int stt))
                                        {
                                            HocPhanDaoTao hocPhanDaoTao = new HocPhanDaoTao();
                                            if (workSheet.Cells[rowIterator, 2].Value != null)
                                            {
                                                hocPhanDaoTao.MaHocPhan = workSheet.Cells[rowIterator, 2].Value.ToString();
                                            }
                                            if (workSheet.Cells[rowIterator, 3].Value != null)
                                            {
                                                hocPhanDaoTao.TenHocPhan = workSheet.Cells[rowIterator, 3].Value.ToString();
                                            }
                                            if (workSheet.Cells[rowIterator, 4].Value != null)
                                            {
                                                hocPhanDaoTao.SoTinChi = workSheet.Cells[rowIterator, 4].Value.ToString();
                                            }
                                            if (workSheet.Cells[rowIterator, 5].Value != null)
                                            {
                                                var LoaiHP = workSheet.Cells[rowIterator, 5].Value.ToString();
                                                if (LoaiHP == "TC")
                                                {
                                                    hocPhanDaoTao.HocPhanDaoTao2 = db.HocPhanDaoTaos.Where(s => s.KhoiKienThuc.ChuongTrinhDaoTao.ID == CHUONGTRINHDAOTAO.ID).Where(s => s.KhoiKienThuc.TenKhoiKienThuc == KHOIKIENTHUC).FirstOrDefault(s => s.TenHocPhan == HPBATBUOC);
                                                }
                                                else
                                                {
                                                    HPBATBUOC = workSheet.Cells[rowIterator, 3].Value.ToString();
                                                }
                                            }
                                            else
                                            {
                                                HPBATBUOC = workSheet.Cells[rowIterator, 3].Value.ToString();
                                            }
                                            if (workSheet.Cells[rowIterator, 9].Value != null)
                                            {
                                                hocPhanDaoTao.HocKy = int.Parse(workSheet.Cells[rowIterator, 9].Value.ToString());
                                            }
                                            hocPhanDaoTao.KhoiKienThuc = db.KhoiKienThucs.Where(s => s.ChuongTrinhDaoTao.ID == CHUONGTRINHDAOTAO.ID).FirstOrDefault(s => s.TenKhoiKienThuc == KHOIKIENTHUC);
                                            db.HocPhanDaoTaos.Add(hocPhanDaoTao);
                                            db.SaveChanges();
                                        }
                                        else
                                        {
                                            KhoiKienThuc taoKKT = new KhoiKienThuc();
                                            taoKKT.MaKhoiKienThuc = khoikienthuc;
                                            taoKKT.TenKhoiKienThuc = workSheet.Cells[rowIterator, 2].Value.ToString();
                                            taoKKT.ChuongTrinhDaoTao = CHUONGTRINHDAOTAO;
                                            db.KhoiKienThucs.Add(taoKKT);
                                            db.SaveChanges();
                                            KHOIKIENTHUC = taoKKT.TenKhoiKienThuc;
                                        }
                                    }
                                    for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                                    {
                                        if (workSheet.Cells[rowIterator, 6].Value != null)
                                        {
                                            RangBuocHocPhan rangBuocHocPhan = new RangBuocHocPhan();
                                            string HocPhanRangBuoc = workSheet.Cells[rowIterator, 6].Value.ToString();
                                            string HocPhan = workSheet.Cells[rowIterator, 3].Value.ToString();
                                            string[] ListHP = HocPhanRangBuoc.Split(new char[] { ',' });
                                            foreach (string HP in ListHP)
                                            {
                                                var mahocphan = db.HocPhanDaoTaos.Where(s => s.KhoiKienThuc.ChuongTrinhDaoTao.ID == CHUONGTRINHDAOTAO.ID).FirstOrDefault(s => s.MaHocPhan == HP);
                                                if (mahocphan == null)
                                                {
                                                    var tenhocphan = db.HocPhanDaoTaos.Where(s => s.KhoiKienThuc.ChuongTrinhDaoTao.ID == CHUONGTRINHDAOTAO.ID).FirstOrDefault(s => s.TenHocPhan.Contains(HP));
                                                    if (tenhocphan != null)
                                                    {
                                                        rangBuocHocPhan.HocPhanDaoTao = tenhocphan;
                                                        rangBuocHocPhan.HocPhanDaoTao1 = db.HocPhanDaoTaos.Where(s => s.KhoiKienThuc.ChuongTrinhDaoTao.ID == CHUONGTRINHDAOTAO.ID).FirstOrDefault(s => s.TenHocPhan == HocPhan);
                                                        rangBuocHocPhan.LoaiRangBuoc = "Tiên quyết";
                                                        db.RangBuocHocPhans.Add(rangBuocHocPhan);
                                                        db.SaveChanges();
                                                    }
                                                }
                                                else
                                                {
                                                    rangBuocHocPhan.HocPhanDaoTao = mahocphan;
                                                    rangBuocHocPhan.HocPhanDaoTao1 = db.HocPhanDaoTaos.Where(s => s.KhoiKienThuc.ChuongTrinhDaoTao.ID == CHUONGTRINHDAOTAO.ID).FirstOrDefault(s => s.TenHocPhan == HocPhan);
                                                    rangBuocHocPhan.LoaiRangBuoc = "Tiên quyết";
                                                    db.RangBuocHocPhans.Add(rangBuocHocPhan);
                                                    db.SaveChanges();
                                                }
                                            }
                                        }
                                        if (workSheet.Cells[rowIterator, 7].Value != null)
                                        {
                                            RangBuocHocPhan rangBuocHocPhan = new RangBuocHocPhan();
                                            string HocPhanRangBuoc = workSheet.Cells[rowIterator, 7].Value.ToString();
                                            string HocPhan = workSheet.Cells[rowIterator, 3].Value.ToString();
                                            string[] ListHP = HocPhanRangBuoc.Split(new char[] { ',' });
                                            foreach (string HP in ListHP)
                                            {
                                                var mahocphan = db.HocPhanDaoTaos.Where(s => s.KhoiKienThuc.ChuongTrinhDaoTao.ID == CHUONGTRINHDAOTAO.ID).FirstOrDefault(s => s.MaHocPhan == HP);
                                                if (mahocphan == null)
                                                {
                                                    var tenhocphan = db.HocPhanDaoTaos.Where(s => s.KhoiKienThuc.ChuongTrinhDaoTao.ID == CHUONGTRINHDAOTAO.ID).FirstOrDefault(s => s.TenHocPhan.Contains(HP));
                                                    if (tenhocphan != null)
                                                    {
                                                        rangBuocHocPhan.HocPhanDaoTao = tenhocphan;
                                                        rangBuocHocPhan.HocPhanDaoTao1 = db.HocPhanDaoTaos.Where(s => s.KhoiKienThuc.ChuongTrinhDaoTao.ID == CHUONGTRINHDAOTAO.ID).FirstOrDefault(s => s.TenHocPhan == HocPhan);
                                                        rangBuocHocPhan.LoaiRangBuoc = "Học trước";
                                                        db.RangBuocHocPhans.Add(rangBuocHocPhan);
                                                        db.SaveChanges();
                                                    }
                                                }
                                                else
                                                {
                                                    rangBuocHocPhan.HocPhanDaoTao = mahocphan;
                                                    rangBuocHocPhan.HocPhanDaoTao1 = db.HocPhanDaoTaos.Where(s => s.KhoiKienThuc.ChuongTrinhDaoTao.ID == CHUONGTRINHDAOTAO.ID).FirstOrDefault(s => s.TenHocPhan == HocPhan);
                                                    rangBuocHocPhan.LoaiRangBuoc = "Học trước";
                                                    db.RangBuocHocPhans.Add(rangBuocHocPhan);
                                                    db.SaveChanges();
                                                }
                                            }
                                        }
                                        if (workSheet.Cells[rowIterator, 8].Value != null)
                                        {
                                            RangBuocHocPhan rangBuocHocPhan = new RangBuocHocPhan();
                                            string HocPhanRangBuoc = workSheet.Cells[rowIterator, 8].Value.ToString();
                                            string HocPhan = workSheet.Cells[rowIterator, 3].Value.ToString();
                                            string[] ListHP = HocPhanRangBuoc.Split(new char[] { ',' });
                                            foreach (string HP in ListHP)
                                            {
                                                var mahocphan = db.HocPhanDaoTaos.Where(s => s.KhoiKienThuc.ChuongTrinhDaoTao.ID == CHUONGTRINHDAOTAO.ID).FirstOrDefault(s => s.MaHocPhan == HP);
                                                if (mahocphan == null)
                                                {
                                                    var tenhocphan = db.HocPhanDaoTaos.Where(s => s.KhoiKienThuc.ChuongTrinhDaoTao.ID == CHUONGTRINHDAOTAO.ID).FirstOrDefault(s => s.TenHocPhan.Contains(HP));
                                                    if (tenhocphan != null)
                                                    {
                                                        rangBuocHocPhan.HocPhanDaoTao = tenhocphan;
                                                        rangBuocHocPhan.HocPhanDaoTao1 = db.HocPhanDaoTaos.Where(s => s.KhoiKienThuc.ChuongTrinhDaoTao.ID == CHUONGTRINHDAOTAO.ID).FirstOrDefault(s => s.TenHocPhan == HocPhan);
                                                        rangBuocHocPhan.LoaiRangBuoc = "Song hành";
                                                        db.RangBuocHocPhans.Add(rangBuocHocPhan);
                                                        db.SaveChanges();
                                                    }
                                                }
                                                else
                                                {
                                                    rangBuocHocPhan.HocPhanDaoTao = mahocphan;
                                                    rangBuocHocPhan.HocPhanDaoTao1 = db.HocPhanDaoTaos.Where(s => s.KhoiKienThuc.ChuongTrinhDaoTao.ID == CHUONGTRINHDAOTAO.ID).FirstOrDefault(s => s.TenHocPhan == HocPhan);
                                                    rangBuocHocPhan.LoaiRangBuoc = "Song hành";
                                                    db.RangBuocHocPhans.Add(rangBuocHocPhan);
                                                    db.SaveChanges();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        TempData["ThongBao"] = "Thêm mới thành công chương trình đào tạo";
                        return RedirectToAction("ChitietCTDaoTao", new { id = CHUONGTRINHDAOTAO.ID });
                    }
                    else
                    {
                        TempData["Alert"] = "File bị trống, vui lòng thử lại!!";
                    }
                }
                else
                {
                    TempData["Alert"] = "Chương trình đào tạo bị trùng, vui lòng thử lại!!";
                    return RedirectToAction("ListCTDaoTao");
                }
                return RedirectToAction("ListCTDaoTao");
            }
            catch (Exception)
            {
                TempData["Alert"] = "File bị sai định dạng, vui lòng thử lại";
                return Redirect(Request.UrlReferrer.ToString());
            }
        }

        [HttpPost]
        public ActionResult ChinhSuaCTDaoTao(FormCollection formCollection, int CTDT, string Nganh, string Khoa, string HK)
        {
            if (Request != null)
            {
                HttpPostedFileBase file = Request.Files["UploadedFile"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    var listLoi = KiemTraFile(file);
                    if (listLoi != "")
                    {
                        TempData["Alert"] = listLoi;
                        return RedirectToAction("ChitietCTDaoTao", new { id = CTDT });
                    }
                    XoaCTDT(CTDT);
                    UploadCTDaoTao(formCollection, Nganh, Khoa, HK);
                    var CHUONGTRINHDAOTAO = db.ChuongTrinhDaoTaos.Where(s => s.NganhDaoTao.Nganh == Nganh).FirstOrDefault(s => s.KhoaDaoTao.Khoa.ToString() == Khoa.ToString());
                    return RedirectToAction("ChitietCTDaoTao", new { id = CHUONGTRINHDAOTAO.ID });
                }
            }
            return RedirectToAction("ChitietCTDaoTao", new { id = CTDT });
        }
        public ActionResult ChiTietCTDaoTao(int? id)
        {
            if (id is null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            var ChuongTrinhDaoTao = db.ChuongTrinhDaoTaos.Find(id);
            if (ChuongTrinhDaoTao == null)
            {
                return RedirectToAction("ListCTDaoTao");
            }
            ViewData["KhoiKienThuc"] = db.KhoiKienThucs.Where(s => s.ChuongTrinhDaoTao.ID == id).ToList();
            ViewData["RangBuocHocPhan"] = db.RangBuocHocPhans.Where(s => s.HocPhanDaoTao.KhoiKienThuc.ChuongTrinhDaoTao.ID == id).ToList();
            ViewData["HocPhan"] = db.HocPhanDaoTaos.ToList();
            return View(ChuongTrinhDaoTao);
        }

        public ActionResult XuatCTDaoTao(int? id)
        {
            if (id is null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            var chuongTrinhDaoTao = db.ChuongTrinhDaoTaos.Find(id);
            var khoiKienThuc = db.KhoiKienThucs.Where(s => s.ChuongTrinhDaoTao.ID == chuongTrinhDaoTao.ID);
            ExcelPackage ep = new ExcelPackage();
            var sheet = ep.Workbook.Worksheets.Add("Ngành " + chuongTrinhDaoTao.NganhDaoTao.Nganh + " Khóa " + chuongTrinhDaoTao.KhoaDaoTao.Khoa);

            sheet.Cells["A1"].Value = "STT";
            sheet.Cells["B1"].Value = "Mã HP";
            sheet.Cells["C1"].Value = "TÊN HỌC PHẦN";
            sheet.Cells["D1"].Value = "SỐ TC";
            sheet.Cells["E1"].Value = "BB/TC";
            sheet.Cells["F1"].Value = "Tiên quyết";
            sheet.Cells["G1"].Value = "Học trước";
            sheet.Cells["H1"].Value = "Song hành";
            sheet.Cells["I1"].Value = "Học kỳ";
            sheet.Cells["A1:I1"].Style.Font.Bold = true;

            int row = 2;
            int STT = 0;
            foreach (var khoikt in khoiKienThuc)
            {
                sheet.Cells[string.Format("A{0}", row)].Value = khoikt.MaKhoiKienThuc;
                sheet.Cells[string.Format("B{0}", row)].Value = khoikt.TenKhoiKienThuc;
                sheet.Cells[string.Format("A{0}:B{0}", row)].Style.Font.Bold = true;
                row++;
                var hocPhan = db.HocPhanDaoTaos.Where(s => s.KhoiKienThuc.ID == khoikt.ID);
                var rangBuocHocPhan = db.RangBuocHocPhans.ToList();
                foreach (var hp in hocPhan)
                {
                    if (hp.ID_HocPhanTuChon == null)
                    {
                        STT++;
                        sheet.Cells[string.Format("A{0}", row)].Value = STT;
                        sheet.Cells[string.Format("E{0}", row)].Value = "BB";
                    }
                    else
                    {
                        sheet.Cells[string.Format("E{0}", row)].Value = "TC";
                    }
                    sheet.Cells[string.Format("B{0}", row)].Value = hp.MaHocPhan;
                    sheet.Cells[string.Format("C{0}", row)].Value = hp.TenHocPhan;
                    sheet.Cells[string.Format("D{0}", row)].Value = hp.SoTinChi;
                    sheet.Cells[string.Format("I{0}", row)].Value = hp.HocKy;

                    string HPTienQuyet = "";
                    string HPHocTruoc = "";
                    string HPSongHanh = "";
                    foreach (var rbHocPhan in rangBuocHocPhan)
                    {
                        if (rbHocPhan.LoaiRangBuoc == "Tiên quyết" && rbHocPhan.HocPhanDaoTao1.ID == hp.ID)
                        {
                            if (HPTienQuyet != "")
                            {
                                HPTienQuyet += ",";
                            }
                            if (rbHocPhan.HocPhanDaoTao.MaHocPhan != null)
                            {
                                HPTienQuyet += rbHocPhan.HocPhanDaoTao.MaHocPhan;
                            }
                            else
                            {
                                HPTienQuyet += rbHocPhan.HocPhanDaoTao.TenHocPhan;
                            }
                        }
                        if (rbHocPhan.LoaiRangBuoc == "Học trước" && rbHocPhan.HocPhanDaoTao1.ID == hp.ID)
                        {
                            if (HPHocTruoc != "")
                            {
                                HPHocTruoc += ",";
                            }
                            if (rbHocPhan.HocPhanDaoTao.MaHocPhan != null)
                            {
                                HPHocTruoc += rbHocPhan.HocPhanDaoTao.MaHocPhan;
                            }
                            else
                            {
                                HPHocTruoc += rbHocPhan.HocPhanDaoTao.TenHocPhan;
                            }
                        }
                        if (rbHocPhan.LoaiRangBuoc == "Song hành" && rbHocPhan.HocPhanDaoTao1.ID == hp.ID)
                        {
                            if (HPSongHanh != "")
                            {
                                HPSongHanh += ",";
                            }
                            if (rbHocPhan.HocPhanDaoTao.MaHocPhan != null)
                            {
                                HPSongHanh += rbHocPhan.HocPhanDaoTao.MaHocPhan;
                            }
                            else
                            {
                                HPSongHanh += rbHocPhan.HocPhanDaoTao.TenHocPhan;
                            }
                        }
                    }
                    sheet.Cells[string.Format("F{0}", row)].Value = HPTienQuyet;
                    sheet.Cells[string.Format("G{0}", row)].Value = HPHocTruoc;
                    sheet.Cells[string.Format("H{0}", row)].Value = HPSongHanh;
                    row++;
                }
            }
            sheet.Cells["A:A"].AutoFitColumns();
            sheet.Cells["C:E"].AutoFitColumns();
            sheet.Cells["I:AZ"].AutoFitColumns();

            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment; filename=" + "Xuất học phần.xlsx");
            Response.BinaryWrite(ep.GetAsByteArray());
            Response.End();

            return RedirectToAction("ChiTietCTDaoTao", new { id });
        }

        public ActionResult InCTDaoTao(int? id)
        {
            if (id is null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            var ChuongTrinhDaoTao = db.ChuongTrinhDaoTaos.Find(id);
            if (ChuongTrinhDaoTao == null)
            {
                return RedirectToAction("ListCTDaoTao");
            }
            ViewData["KhoiKienThuc"] = db.KhoiKienThucs.Where(s => s.ChuongTrinhDaoTao.ID == id).ToList();
            ViewData["RangBuocHocPhan"] = db.RangBuocHocPhans.Where(s => s.HocPhanDaoTao.KhoiKienThuc.ChuongTrinhDaoTao.ID == id).ToList();
            ViewData["HocPhan"] = db.HocPhanDaoTaos.ToList();
            return View(ChuongTrinhDaoTao);
        }

        public ActionResult XoaCTDT(int id)
        {
            var ChuongTrinhDaoTao = db.ChuongTrinhDaoTaos.Find(id);
            if (ChuongTrinhDaoTao == null)
            {
                return RedirectToAction("ListCTDaoTao");
            }
            var listkkt = db.KhoiKienThucs.Where(s => s.ChuongTrinhDaoTao.ID == ChuongTrinhDaoTao.ID);
            if (listkkt != null)
                foreach (var khoikienthuc in listkkt.ToList())
                {
                    var listhocphan = db.HocPhanDaoTaos.Where(s => s.KhoiKienThuc.ID == khoikienthuc.ID);
                    if (listhocphan != null)
                    {
                        foreach (var hocphan in listhocphan.ToList())
                        {
                            var listRangBuocHP = db.RangBuocHocPhans.Where(s => s.ID_HocPhan == hocphan.ID);
                            if (listRangBuocHP != null)
                                foreach (var rangbuoc in listRangBuocHP.ToList())
                                {
                                    db.RangBuocHocPhans.Remove(rangbuoc);
                                    db.SaveChanges();
                                }
                        }
                        foreach (var hocphan in listhocphan.OrderByDescending(s => s.ID).ToList())
                        {
                            if (hocphan.ID_HocPhanTuChon != null)
                            {
                                db.HocPhanDaoTaos.Remove(hocphan);
                                db.SaveChanges();
                            }
                        }
                    }
                }
            foreach (var khoikienthuc in listkkt.ToList())
            {
                var listhocphan = db.HocPhanDaoTaos.Where(s => s.KhoiKienThuc.ID == khoikienthuc.ID);
                if (listhocphan != null)
                {
                    foreach (var hocphan in listhocphan.ToList())
                    {
                        var listRangBuocHP = db.RangBuocHocPhans.Where(s => s.ID_HocPhan == hocphan.ID);
                        if (listRangBuocHP != null)
                            foreach (var rangbuoc in listRangBuocHP.ToList())
                            {
                                db.RangBuocHocPhans.Remove(rangbuoc);
                                db.SaveChanges();
                            }
                    }
                    foreach (var hocphan in listhocphan.OrderByDescending(s => s.ID).ToList())
                    {
                        db.HocPhanDaoTaos.Remove(hocphan);
                        db.SaveChanges();
                    }
                }
                db.KhoiKienThucs.Remove(khoikienthuc);
                db.SaveChanges();
            }
            db.ChuongTrinhDaoTaos.Remove(ChuongTrinhDaoTao);
            db.SaveChanges();
            TempData["ThongBao"] = "Xóa thành công chương trình đào tạo";
            return RedirectToAction("ChiTietCTDaoTao", new { id });
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
