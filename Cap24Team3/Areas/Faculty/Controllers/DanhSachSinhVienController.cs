using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using System.Net;
using OfficeOpenXml;
using Cap24Team3.Models;

namespace Cap24Team3.Areas.Faculty.Controllers
{
    public class DanhSachSinhVienController : Controller
    {
        public Cap24 db = new Cap24();

        public static string RemoveUnicode(string text)
        {
            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
                "đ",
                "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",
                "í","ì","ỉ","ĩ","ị",
                "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
                "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",
                "ý","ỳ","ỷ","ỹ","ỵ",};
            string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
                "d",
                "e","e","e","e","e","e","e","e","e","e","e",
                "i","i","i","i","i",
                "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",
                "u","u","u","u","u","u","u","u","u","u","u",
                "y","y","y","y","y",};
            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
                text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
            }
            return text;
        }
        public ActionResult TimKiemSinhVien(string texttimkiem)
        {
            var model = db.SinhViens.ToList();
            if (texttimkiem == null)
            {
                return View(model);
            }
            var text = RemoveUnicode(texttimkiem);
            var ho = "";
            var ten = "";
            var splittext = text.Split(' ');
            if (splittext.Length == 1)
            {
                ten = splittext[0];
            }
            else
            {
                ten = splittext[splittext.Length - 1];
                ho = splittext[0];
                for (int i = 1; i < splittext.Length - 1; i++)
                    ho += " " + splittext[i];
            }
            if (ten != "")
            {
                model = model.Where(p => RemoveUnicode(p.Ten.ToLower()) == ten.ToLower()).ToList();
            }
            if (ho != "")
            {
                model = model.Where(p => RemoveUnicode(p.Ho.ToLower()).Contains(ho.ToLower())).ToList();
            }
            var checkmssv = db.SinhViens.Where(p => p.MSSV.ToLower().Contains(texttimkiem.ToLower())).ToList();
            if (checkmssv != null)
                foreach (var item in checkmssv)
                    model.Add(item);
            var checkho = db.SinhViens.ToList().Where(p => RemoveUnicode(p.Ho.ToLower()).Contains(text.ToLower())).ToList();
            if (checkho != null)
                foreach (var item in checkho)
                    model.Add(item);
            ViewBag.Keyword = texttimkiem;
            return View(model);
        }
        public string KiemTraLop(string tenLop)
        {
            string ListLoi = "";

            if (tenLop.Length > 20)
            {
                ListLoi += "<p> Tên lớp không được quá 20 ký tự, số ký tự của lớp bạn nhập là: " + tenLop.Length + "</p>";
            }
            var listDBLop = db.LopQuanLies.ToList();
            var listLop = new List<string>();
            foreach (var item in listDBLop)
            {
                listLop.Add(item.TenLop);
            }
            if (CheckTonTai(tenLop, listLop))
            {
                ListLoi += "<p> Lớp đã tồn tại trong hệ thống, vui lòng thử lại!</p>";
            }
            return ListLoi;
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
        public ActionResult ListLop()
        {
            var lopQuanLies = db.LopQuanLies.Include(l => l.KhoaDaoTao).Include(l => l.NganhDaoTao);
            return View(lopQuanLies.ToList());
        }
        // GET: Faculty/LopQuanLies/Create
        public ActionResult TaoMoiLop()
        {
            ViewBag.ID_Khoa = new SelectList(db.KhoaDaoTaos, "ID", "ID");
            ViewBag.ID_Nganh = new SelectList(db.NganhDaoTaos, "ID", "Nganh");
            return View();
        }

        public ActionResult Details(int ID)
        {
            var model = db.SinhViens.Find(ID);
            if (model == null)
            {
                return HttpNotFound();

            }
            return View(model);
        }
        public ActionResult KhoaSinhVien()
        {
            var lop = db.LopQuanLies.OrderByDescending(l => l.ID_Khoa).ToList();
            var khoa = new List<KhoaDaoTao>();
            var stringkhoa = new List<string>();
            foreach (var item in lop)
            {
                if (!CheckTonTai(item.KhoaDaoTao.Khoa.ToString(), stringkhoa))
                {
                    khoa.Add(item.KhoaDaoTao);
                    stringkhoa.Add(item.KhoaDaoTao.Khoa.ToString());
                }
            }
            return View(khoa);
        }

        public ActionResult NganhSinhVien(int ID)
        {
            var khoa = db.KhoaDaoTaos.Find(ID);
            var lop = db.LopQuanLies.Where(s => s.KhoaDaoTao.ID == khoa.ID).ToList();
            var nganh = new List<NganhDaoTao>();
            var stringnganh = new List<string>();
            if (lop != null)
                foreach (var item in lop)
                {
                    if (!CheckTonTai(item.NganhDaoTao.Nganh, stringnganh))
                    {
                        nganh.Add(item.NganhDaoTao);
                        stringnganh.Add(item.NganhDaoTao.Nganh);
                    }
                }
            ViewData["KhoaDaoTao"] = khoa;
            return View(nganh);
        }
        public ActionResult LopSinhVien(int idnganh, int idkhoa)
        {
            var khoa = db.KhoaDaoTaos.Find(idkhoa);
            var nganh = db.NganhDaoTaos.Find(idnganh);
            var lop = db.LopQuanLies.Where(s => s.NganhDaoTao.ID == idnganh).Where(s => s.KhoaDaoTao.ID == idkhoa).OrderBy(l => l.TenLop).ToList();
            return View(lop);
        }
        // GET: Faculty/DanhSachSinhVien
        public ActionResult ListSinhVien(int id)
        {
            var sinhvien = db.SinhViens.OrderBy(s => s.TinhTrang.DoUuTien).Where(s => s.LopQuanLy.ID == id).ToList();
            return View(sinhvien);
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
                    var noOfRow = workSheet.Dimension.End.Row;
                    var noOfCol = workSheet.Dimension.End.Column;
                    if (noOfCol == 15 && noOfRow > 1)
                    {
                        var listMSSV = new List<string>();
                        var listEmail = new List<string>();
                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {
                            if (workSheet.Cells[rowIterator, 1].Value != null)
                            {
                                var MSSV = workSheet.Cells[rowIterator, 1].Value.ToString();
                                if (CheckTonTai(MSSV, listMSSV))
                                {
                                    DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột A: Dữ liệu bị trùng, dữ liệu trùng: " + MSSV + "</p>";
                                }
                                if (MSSV.Length > 20)
                                {
                                    DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột A: Dữ liệu không được quá 20 ký tự, số ký tự của " + MSSV + " là " + MSSV.Length + "<p>";
                                }
                                if (MSSV.Replace(" ", string.Empty) == null)
                                {
                                    DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột A: Dữ liệu không được để trống <p>";
                                }
                                listMSSV.Add(MSSV);
                            }
                            else
                            {
                                DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột A: Dữ liệu không được để trống <p>";
                            }
                            if (workSheet.Cells[rowIterator, 2].Value != null)
                            {
                                var Ho = workSheet.Cells[rowIterator, 2].Value.ToString();
                                if (Ho.Length > 100)
                                {
                                    DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột B: Dữ liệu không được quá 100 ký tự, số ký tự của " + Ho + " là " + Ho.Length + "<p>";
                                }
                                if (Ho.Replace(" ", string.Empty) == null)
                                {
                                    DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột B: Dữ liệu không được để trống <p>";
                                }
                            }
                            else
                            {
                                DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột B: Dữ liệu không được để trống <p>";
                            }
                            if (workSheet.Cells[rowIterator, 3].Value != null)
                            {
                                var Ten = workSheet.Cells[rowIterator, 3].Value.ToString();
                                if (Ten.Length > 100)
                                {
                                    DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột C: Dữ liệu không được quá 100 ký tự, số ký tự của " + Ten + " là " + Ten.Length + "<p>";
                                }
                                if (Ten.Replace(" ", string.Empty) == null)
                                {
                                    DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột C: Dữ liệu không được để trống <p>";
                                }
                            }
                            else
                            {
                                DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột C: Dữ liệu không được để trống <p>";
                            }
                            if (workSheet.Cells[rowIterator, 4].Value != null)
                            {
                                var NgaySinh = workSheet.Cells[rowIterator, 4].Value.ToString();
                                if (NgaySinh.Length > 10)
                                {
                                    DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột D: Dữ liệu không được quá 10 ký tự, số ký tự của " + NgaySinh + " là " + NgaySinh.Length + "<p>";
                                }
                                if (NgaySinh.Replace(" ", string.Empty) == null)
                                {
                                    DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột D: Dữ liệu không được để trống <p>";
                                }
                            }
                            else
                            {
                                DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột D: Dữ liệu không được để trống <p>";
                            }
                            if (workSheet.Cells[rowIterator, 5].Value != null)
                            {
                                var GT = workSheet.Cells[rowIterator, 5].Value.ToString();
                                if (GT.Length > 10)
                                {
                                    DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột E: Dữ liệu không được quá 10 ký tự, số ký tự của " + GT + " là " + GT.Length + "<p>";
                                }
                                if (GT.Replace(" ", string.Empty) == null)
                                {
                                    DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột E: Dữ liệu không được để trống <p>";
                                }
                            }
                            else
                            {
                                DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột E: Dữ liệu không được để trống <p>";
                            }
                            if (workSheet.Cells[rowIterator, 6].Value != null)
                            {
                                var TinhTrang = workSheet.Cells[rowIterator, 6].Value.ToString();
                                if (TinhTrang.Length > 100)
                                {
                                    DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột F: Dữ liệu không được quá 100 ký tự, số ký tự của " + TinhTrang + " là " + TinhTrang.Length + "<p>";
                                }
                                if (TinhTrang.Replace(" ", string.Empty) == null)
                                {
                                    DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột F: Dữ liệu không được để trống <p>";
                                }
                            }
                            else
                            {
                                DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột F: Dữ liệu không được để trống <p>";

                            }
                            if (workSheet.Cells[rowIterator, 8].Value != null)
                            {
                                var Lop = workSheet.Cells[rowIterator, 8].Value.ToString();
                                if (Lop.Length > 20)
                                {
                                    DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột H: Dữ liệu không được quá 20 ký tự, số ký tự của " + Lop + " là " + Lop.Length + "<p>";
                                }
                                if (Lop.Replace(" ", string.Empty) == null)
                                {
                                    DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột H: Dữ liệu không được để trống <p>";
                                }
                            }
                            else
                            {
                                DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột H: Dữ liệu không được để trống <p>";
                            }
                            if (workSheet.Cells[rowIterator, 9].Value != null)
                            {
                                var Email_1 = workSheet.Cells[rowIterator, 9].Value.ToString();
                                if (CheckTonTai(Email_1, listEmail))
                                {
                                    DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột I: Dữ liệu bị trùng, dữ liệu trùng: " + Email_1 + "</p>";
                                }
                                if (Email_1.Length > 100)
                                {
                                    DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột I: Dữ liệu không được quá 100 ký tự, số ký tự của " + Email_1 + " là " + Email_1.Length + "<p>";
                                }
                                if (Email_1.Replace(" ", string.Empty) == null)
                                {
                                    DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột I: Dữ liệu không được để trống <p>";
                                }
                                listEmail.Add(Email_1);
                            }
                            else
                            {
                                DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột I: Dữ liệu không được để trống <p>";
                            }
                            if (workSheet.Cells[rowIterator, 10].Value != null)
                            {
                                var Email_2 = workSheet.Cells[rowIterator, 10].Value.ToString();
                                if (Email_2.Length > 100)
                                {
                                    DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột J: Dữ liệu không được quá 100 ký tự, số ký tự của " + Email_2 + " là " + Email_2.Length + "<p>";
                                }
                            }
                            if (workSheet.Cells[rowIterator, 12].Value != null)
                            {
                                var DTDD = workSheet.Cells[rowIterator, 12].Value.ToString();
                                if (DTDD.Length > 100)
                                {
                                    DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột L: Dữ liệu không được quá 100 ký tự, số ký tự của " + DTDD + " là " + DTDD.Length + "<p>";
                                }
                            }
                            if (workSheet.Cells[rowIterator, 13].Value != null)
                            {
                                var DTCha = workSheet.Cells[rowIterator, 13].Value.ToString();
                                if (DTCha.Length > 100)
                                {
                                    DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột M: Dữ liệu không được quá 100 ký tự, số ký tự của " + DTCha + " là " + DTCha.Length + "<p>";
                                }
                            }
                            if (workSheet.Cells[rowIterator, 14].Value != null)
                            {
                                var DTMe = workSheet.Cells[rowIterator, 14].Value.ToString();
                                if (DTMe.Length > 100)
                                {
                                    DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột N: Dữ liệu không được quá 100 ký tự, số ký tự của " + DTMe + " là " + DTMe.Length + "<p>";
                                }
                            }
                            if (workSheet.Cells[rowIterator, 15].Value != null)
                            {
                                var DiaChi = workSheet.Cells[rowIterator, 15].Value.ToString();
                                if (DiaChi.Replace(" ", string.Empty) == null)
                                {
                                    DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột O: Dữ liệu không được để trống <p>";
                                }
                            }
                            if (workSheet.Cells[rowIterator, 11].Value != null)
                            {
                                var listNganh = db.NganhDaoTaos.ToList();
                                var listNganhMoi = new List<string>();
                                var maNganh = workSheet.Cells[rowIterator, 11].Value.ToString();
                                foreach (var item in listNganh)
                                {
                                    listNganhMoi.Add(item.MaNganh.ToString());
                                }
                                if (CheckTonTai(maNganh, listNganhMoi) == false)
                                {
                                    DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột K: Mã ngành " + maNganh + " chưa có trong hệ thống <p>";
                                }
                            }
                            else
                            {
                                DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột K: Dữ liệu không được để trống <p>";
                            }
                            if (workSheet.Cells[rowIterator, 7].Value != null)
                            {
                                var listKhoa = db.KhoaDaoTaos.ToList();
                                var listKhoaMoi = new List<string>();
                                foreach (var item in listKhoa)
                                {
                                    listKhoaMoi.Add(item.Khoa.ToString());
                                }
                                var tenKhoa = workSheet.Cells[rowIterator, 7].Value.ToString().Replace("K", string.Empty);
                                if (CheckTonTai(tenKhoa, listKhoaMoi) == false)
                                {
                                    DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột G: Khóa K" + tenKhoa + " chưa có trong hệ thống <p>";
                                }
                            }
                            else
                            {
                                DanhSachLoi += "<p> Lỗi ở dòng " + rowIterator + ", cột G: Dữ liệu không được để trống <p>";
                            }
                        }
                    }
                    else if (noOfCol != 15)
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
        [HttpPost]
        public ActionResult XemTruocThongKe(FormCollection formCollection)
        {
            try
            {
                var LuuSinhVien = new List<SinhVien>();
                var LuuLop = new List<LopQuanLy>();
                var LuuTinhTrang = new List<TinhTrang>();
                var soluong = 0;
                if (Request != null)
                {
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
                                if (noOfCol == 15 && noOfRow > 1)
                                {
                                    soluong = noOfRow - 1;
                                    var listTenLop = new List<string>();
                                    foreach (var item in LuuLop)
                                    {
                                        listTenLop.Add(item.TenLop);
                                    }
                                    var listTinhTrangMoi = new List<string>();
                                    foreach (var item in LuuTinhTrang)
                                    {
                                        listTinhTrangMoi.Add(item.TenTinhTrang);
                                    }
                                    for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                                    {
                                        if (workSheet.Cells[rowIterator, 8].Value != null)
                                        {
                                            var tenLop = workSheet.Cells[rowIterator, 8].Value.ToString();
                                            var maNganh = workSheet.Cells[rowIterator, 11].Value.ToString();
                                            var maKhoa = workSheet.Cells[rowIterator, 7].Value.ToString().Replace("K", string.Empty);

                                            if (!CheckTonTai(tenLop, listTenLop))
                                            {
                                                var lopQL = new LopQuanLy();
                                                lopQL.TenLop = tenLop;
                                                lopQL.ID_Nganh = db.NganhDaoTaos.FirstOrDefault(s => s.MaNganh.ToString() == maNganh).ID;
                                                lopQL.ID_Khoa = db.KhoaDaoTaos.FirstOrDefault(s => s.Khoa.ToString() == maKhoa).ID;
                                                LuuLop.Add(lopQL);
                                                listTenLop.Add(tenLop);
                                            }
                                        }
                                        if (workSheet.Cells[rowIterator, 6].Value != null)
                                        {
                                            var tenTinhTrang = workSheet.Cells[rowIterator, 6].Value.ToString();

                                            if (!CheckTonTai(tenTinhTrang, listTinhTrangMoi))
                                            {
                                                var TinhTrangMoi = new TinhTrang();
                                                TinhTrangMoi.TenTinhTrang = tenTinhTrang;
                                                TinhTrangMoi.DoUuTien = 1;
                                                LuuTinhTrang.Add(TinhTrangMoi);
                                                listTinhTrangMoi.Add(tenTinhTrang);
                                            }
                                        }
                                    }
                                    for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                                    {
                                        SinhVien SaveSV = new SinhVien();
                                        if (workSheet.Cells[rowIterator, 1].Value != null)
                                        {
                                            var mssv = workSheet.Cells[rowIterator, 1].Value.ToString();
                                            SaveSV.MSSV = mssv;
                                            int hk;
                                            var HocKyBatDau = int.TryParse(mssv.Substring(0, 2), out hk);
                                            if (HocKyBatDau)
                                                SaveSV.HocKyBatDau = hk * 10 + 1;
                                            else
                                                SaveSV.HocKyBatDau = 171;
                                        }
                                        if (workSheet.Cells[rowIterator, 2].Value != null)
                                        {
                                            SaveSV.Ho = workSheet.Cells[rowIterator, 2].Value.ToString();
                                        }
                                        if (workSheet.Cells[rowIterator, 3].Value != null)
                                        {
                                            SaveSV.Ten = workSheet.Cells[rowIterator, 3].Value.ToString();
                                        }
                                        if (workSheet.Cells[rowIterator, 4].Value != null)
                                        {
                                            SaveSV.NgaySinh = workSheet.Cells[rowIterator, 4].Value.ToString();
                                        }
                                        if (workSheet.Cells[rowIterator, 5].Value != null)
                                        {
                                            SaveSV.GioiTinh = workSheet.Cells[rowIterator, 5].Value.ToString();
                                        }
                                        if (workSheet.Cells[rowIterator, 7].Value != null)
                                        {
                                            var khoa = workSheet.Cells[rowIterator, 7].Value.ToString().Replace("K", string.Empty);
                                            SaveSV.ID_Khoa = db.KhoaDaoTaos.FirstOrDefault(s => s.Khoa.ToString() == khoa).ID;
                                        }
                                        if (workSheet.Cells[rowIterator, 9].Value != null)
                                        {
                                            SaveSV.Email_1 = workSheet.Cells[rowIterator, 9].Value.ToString();
                                        }
                                        if (workSheet.Cells[rowIterator, 10].Value != null)
                                        {
                                            SaveSV.Email_2 = workSheet.Cells[rowIterator, 10].Value.ToString();
                                        }
                                        if (workSheet.Cells[rowIterator, 11].Value != null)
                                        {
                                            var nganh = workSheet.Cells[rowIterator, 11].Value.ToString();
                                            SaveSV.ID_Nganh = db.NganhDaoTaos.FirstOrDefault(s => s.MaNganh.ToString() == nganh).ID;
                                        }
                                        if (workSheet.Cells[rowIterator, 12].Value != null)
                                        {
                                            SaveSV.DTDD = workSheet.Cells[rowIterator, 12].Value.ToString();
                                        }
                                        if (workSheet.Cells[rowIterator, 13].Value != null)
                                        {
                                            SaveSV.DTCha = workSheet.Cells[rowIterator, 13].Value.ToString();
                                        }
                                        if (workSheet.Cells[rowIterator, 14].Value != null)
                                        {
                                            SaveSV.DTMe = workSheet.Cells[rowIterator, 14].Value.ToString();
                                        }
                                        if (workSheet.Cells[rowIterator, 15].Value != null)
                                        {
                                            SaveSV.DiaChi = workSheet.Cells[rowIterator, 15].Value.ToString();
                                        }
                                        if (workSheet.Cells[rowIterator, 8].Value != null)
                                        {
                                            var tenLop = workSheet.Cells[rowIterator, 8].Value.ToString();
                                            SaveSV.LopQuanLy = LuuLop.FirstOrDefault(s => s.TenLop == tenLop);
                                        }
                                        if (workSheet.Cells[rowIterator, 6].Value != null)
                                        {
                                            var tinhtrang = workSheet.Cells[rowIterator, 6].Value.ToString();
                                            SaveSV.TinhTrang = LuuTinhTrang.FirstOrDefault(s => s.TenTinhTrang == tinhtrang);
                                        }
                                        LuuSinhVien.Add(SaveSV);
                                    }
                                }
                            }
                        }
                    }
                }
                Session["LuuSinhVien"] = LuuSinhVien;
                Session["LuuLop"] = LuuLop;
                Session["LuuTinhTrang"] = LuuTinhTrang;
                //Session["ThongBao"]
                string thongbao = "<table class='table table-hover mb-0'>";
                thongbao += "<tr>";
                thongbao += "<td>Số lượng sinh viên: " + soluong + "</td>";
                thongbao += "</tr>";
                foreach (var khoa in db.KhoaDaoTaos)
                {
                    thongbao += "<tr>";
                    thongbao += "<td>Lớp thuộc khóa " + khoa.Khoa + " là: " + LuuLop.Where(s => s.ID_Khoa == khoa.ID).Count() + "</td>";
                    thongbao += "<td></td><td>Sinh viên khóa " + khoa.Khoa + " là: " + LuuSinhVien.Where(s => s.LopQuanLy.ID_Khoa == khoa.ID).Count() + "</td>";
                    thongbao += "<td></td></tr>";
                    foreach (var nganh in db.NganhDaoTaos)
                    {
                        thongbao += "<tr>";
                        thongbao += "<td></td><td>Lớp thuộc ngành " + nganh.Nganh + " là: " + LuuLop.Where(s => s.ID_Khoa == khoa.ID).Where(s => s.ID_Nganh == nganh.ID).Count() + "</td>";
                        thongbao += "<td></td><td>Sinh viên ngành " + nganh.Nganh + " là: " + LuuSinhVien.Where(s => s.LopQuanLy.ID_Khoa == khoa.ID).Where(s => s.LopQuanLy.ID_Nganh == nganh.ID).Count() + "</td>";
                        thongbao += "</tr>";
                        foreach (var lop in LuuLop.Where(s => s.ID_Khoa == khoa.ID).Where(s => s.ID_Nganh == nganh.ID))
                        {
                            thongbao += "<tr>";
                            thongbao += "<td></td><td></td><td></td><td>Sinh viên lớp " + lop.TenLop + " là: " + LuuSinhVien.Where(s => s.LopQuanLy.ID_Khoa == khoa.ID).Where(s => s.LopQuanLy.ID_Nganh == nganh.ID).Where(s => s.LopQuanLy.TenLop == lop.TenLop).Count() + "</td>";
                            thongbao += "</tr>";
                        }
                    }
                }
                thongbao += "</table>";
                Session["ThongBao"] = thongbao;
                return Redirect(Request.UrlReferrer.ToString());
            }
            catch (Exception)
            {
                TempData["Alert"] = "File bị sai định dạng, vui lòng thử lại";
                return Redirect(Request.UrlReferrer.ToString());
            }
        }
        public ActionResult XemChiTiet(int id)
        {
            Session["sinhvien"] = db.SinhViens.Find(id);
            Session["tinhtrang"] = db.SinhViens.Find(id).TinhTrang.TenTinhTrang;
            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult XemDiemSinhVien(int id)
        {
            var sinhvien = db.SinhViens.Find(id);
            Session["sinhvien1"] = sinhvien;
            Session["diemso"] = db.DiemHocPhans.Where(s => s.MSSV == sinhvien.MSSV).ToList();
            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult TaiLenSinhVien()
        {
            db.Configuration.AutoDetectChangesEnabled = false;
            db.Configuration.ValidateOnSaveEnabled = false;
            var listMoiSV = Session["LuuSinhVien"] as List<SinhVien>;
            var listMoiLop = Session["LuuLop"] as List<LopQuanLy>;
            var listMoiTinhTrang = Session["LuuTinhTrang"] as List<TinhTrang>;
            var listSV = new List<string>();
            var listLop = new List<string>();
            var listTinhTrang = new List<string>();
            foreach (var item in db.TinhTrangs.ToList())
                listTinhTrang.Add(item.TenTinhTrang);
            foreach (var item in db.LopQuanLies.ToList())
                listLop.Add(item.TenLop);
            foreach (var item in db.SinhViens.ToList())
                listSV.Add(item.MSSV);
            foreach (var item in listMoiTinhTrang)
            {
                if (!CheckTonTai(item.TenTinhTrang, listTinhTrang))
                {
                    var TinhTrangMoi = new TinhTrang();
                    TinhTrangMoi.TenTinhTrang = item.TenTinhTrang;
                    var douutien = 0;
                    if (db.TinhTrangs.ToList().Count != 0)
                    {
                        douutien = db.TinhTrangs.OrderByDescending(s => s.DoUuTien).ToList().First().DoUuTien;
                    }
                    TinhTrangMoi.DoUuTien = douutien + 1;
                    db.TinhTrangs.Add(TinhTrangMoi);
                    listTinhTrang.Add(item.TenTinhTrang);
                    db.SaveChanges();
                }
            }
            foreach (var item in listMoiLop)
            {
                if (!CheckTonTai(item.TenLop, listLop))
                {
                    db.LopQuanLies.Add(item);
                    listLop.Add(item.TenLop);
                    db.SaveChanges();
                }
            }
            foreach (var item in listMoiSV)
            {
                if (!CheckTonTai(item.MSSV, listSV))
                {
                    item.TinhTrang = db.TinhTrangs.FirstOrDefault(s => s.TenTinhTrang == item.TinhTrang.TenTinhTrang);
                    listSV.Add(item.MSSV);
                    db.SinhViens.Add(item);
                }
            }
            db.SaveChanges();
            Session["ThongBao"] = null;
            return Redirect(Request.UrlReferrer.ToString());
        }
        [HttpPost]
        public ActionResult UploadSinhVien(FormCollection formCollection)
        {
            if (Request != null)
            {
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
                            if (noOfCol == 15 && noOfRow > 1)
                            {
                                var listLop = db.LopQuanLies.ToList();
                                var listTenLop = new List<string>();
                                foreach (var item in listLop)
                                {
                                    listTenLop.Add(item.TenLop);
                                }
                                var listTinhTrang = db.TinhTrangs.ToList();
                                var listTinhTrangMoi = new List<string>();
                                foreach (var item in listTinhTrang)
                                {
                                    listTinhTrangMoi.Add(item.TenTinhTrang);
                                }
                                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                                {
                                    if (workSheet.Cells[rowIterator, 8].Value != null)
                                    {
                                        var tenLop = workSheet.Cells[rowIterator, 8].Value.ToString();
                                        var maNganh = workSheet.Cells[rowIterator, 11].Value.ToString();
                                        var maKhoa = workSheet.Cells[rowIterator, 7].Value.ToString().Replace("K", string.Empty);

                                        if (!CheckTonTai(tenLop, listTenLop))
                                        {
                                            var lopQL = new LopQuanLy();
                                            lopQL.TenLop = tenLop;
                                            lopQL.NganhDaoTao = db.NganhDaoTaos.FirstOrDefault(s => s.MaNganh.ToString() == maNganh);
                                            lopQL.KhoaDaoTao = db.KhoaDaoTaos.FirstOrDefault(s => s.Khoa.ToString() == maKhoa);
                                            db.LopQuanLies.Add(lopQL);
                                            listTenLop.Add(tenLop);
                                            db.SaveChanges();
                                        }
                                    }
                                    if (workSheet.Cells[rowIterator, 6].Value != null)
                                    {
                                        var tenTinhTrang = workSheet.Cells[rowIterator, 6].Value.ToString();

                                        if (!CheckTonTai(tenTinhTrang, listTinhTrangMoi))
                                        {
                                            var TinhTrangMoi = new TinhTrang();
                                            TinhTrangMoi.TenTinhTrang = tenTinhTrang;
                                            var douutien = 0;
                                            if (db.TinhTrangs.ToList().Count != 0)
                                            {
                                                douutien = db.TinhTrangs.OrderByDescending(s => s.DoUuTien).ToList().First().DoUuTien;
                                            }
                                            TinhTrangMoi.DoUuTien = douutien + 1;
                                            db.TinhTrangs.Add(TinhTrangMoi);
                                            db.SaveChanges();
                                            listTinhTrangMoi.Add(tenTinhTrang);
                                        }
                                    }
                                }
                                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                                {
                                    SinhVien SaveSV = new SinhVien();
                                    if (workSheet.Cells[rowIterator, 1].Value != null)
                                    {
                                        SaveSV.MSSV = workSheet.Cells[rowIterator, 1].Value.ToString();
                                    }
                                    if (workSheet.Cells[rowIterator, 2].Value != null)
                                    {
                                        SaveSV.Ho = workSheet.Cells[rowIterator, 2].Value.ToString();
                                    }
                                    if (workSheet.Cells[rowIterator, 3].Value != null)
                                    {
                                        SaveSV.Ten = workSheet.Cells[rowIterator, 3].Value.ToString();
                                    }
                                    if (workSheet.Cells[rowIterator, 4].Value != null)
                                    {
                                        SaveSV.NgaySinh = workSheet.Cells[rowIterator, 4].Value.ToString();
                                    }
                                    if (workSheet.Cells[rowIterator, 5].Value != null)
                                    {
                                        SaveSV.GioiTinh = workSheet.Cells[rowIterator, 5].Value.ToString();
                                    }
                                    if (workSheet.Cells[rowIterator, 7].Value != null)
                                    {
                                        var khoa = workSheet.Cells[rowIterator, 7].Value.ToString().Replace("K", string.Empty);
                                        SaveSV.ID_Khoa = db.KhoaDaoTaos.FirstOrDefault(s => s.Khoa.ToString() == khoa).ID;
                                    }
                                    if (workSheet.Cells[rowIterator, 9].Value != null)
                                    {
                                        SaveSV.Email_1 = workSheet.Cells[rowIterator, 9].Value.ToString();
                                    }
                                    if (workSheet.Cells[rowIterator, 10].Value != null)
                                    {
                                        SaveSV.Email_2 = workSheet.Cells[rowIterator, 10].Value.ToString();
                                    }
                                    if (workSheet.Cells[rowIterator, 11].Value != null)
                                    {
                                        var nganh = workSheet.Cells[rowIterator, 11].Value.ToString();
                                        SaveSV.ID_Nganh = db.NganhDaoTaos.FirstOrDefault(s => s.MaNganh.ToString() == nganh).ID;
                                    }
                                    if (workSheet.Cells[rowIterator, 12].Value != null)
                                    {
                                        SaveSV.DTDD = workSheet.Cells[rowIterator, 12].Value.ToString();
                                    }
                                    if (workSheet.Cells[rowIterator, 13].Value != null)
                                    {
                                        SaveSV.DTCha = workSheet.Cells[rowIterator, 13].Value.ToString();
                                    }
                                    if (workSheet.Cells[rowIterator, 14].Value != null)
                                    {
                                        SaveSV.DTMe = workSheet.Cells[rowIterator, 14].Value.ToString();
                                    }
                                    if (workSheet.Cells[rowIterator, 15].Value != null)
                                    {
                                        SaveSV.DiaChi = workSheet.Cells[rowIterator, 15].Value.ToString();
                                    }
                                    if (workSheet.Cells[rowIterator, 8].Value != null)
                                    {
                                        var tenLop = workSheet.Cells[rowIterator, 8].Value.ToString();
                                        SaveSV.LopQuanLy = db.LopQuanLies.FirstOrDefault(s => s.TenLop == tenLop);
                                    }
                                    if (workSheet.Cells[rowIterator, 6].Value != null)
                                    {
                                        var tinhtrang = workSheet.Cells[rowIterator, 6].Value.ToString();
                                        SaveSV.TinhTrang = db.TinhTrangs.FirstOrDefault(s => s.TenTinhTrang == tinhtrang);
                                    }
                                    db.SinhViens.Add(SaveSV);

                                }
                            }
                        }
                    }
                }
                else
                {
                    TempData["Alert"] = "File bị trống, vui lòng thử lại!!";
                }
            }
            db.Configuration.AutoDetectChangesEnabled = false;
            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();
            return RedirectToAction("KhoaSinhVien");
        }

        public ActionResult XuatDSSV(int? idnganh, int? idkhoa, int? idlop)
        {
            ExcelPackage ep = new ExcelPackage();
            var sheet = ep.Workbook.Worksheets.Add("Danh Sách Sinh Viên");
            var sinhvien = db.SinhViens.ToList();
            if (idkhoa != null)
            {
                sinhvien = sinhvien.Where(s => s.LopQuanLy.ID_Khoa == idkhoa).ToList();
            }
            if (idnganh != null)
            {
                sinhvien = sinhvien.Where(s => s.LopQuanLy.ID_Nganh == idnganh).ToList();
            }
            if (idlop != null)
            {
                sinhvien = sinhvien.Where(s => s.LopQuanLy.ID == idlop).ToList();
            }
            sheet.Cells["A1"].Value = "MSSV";
            sheet.Cells["B1"].Value = "Họ";
            sheet.Cells["C1"].Value = "Tên";
            sheet.Cells["D1"].Value = "Ngày Sinh";
            sheet.Cells["E1"].Value = "Giới tính";
            sheet.Cells["F1"].Value = "Tình Trạng";
            sheet.Cells["G1"].Value = "Khóa";
            sheet.Cells["H1"].Value = "Lớp";
            sheet.Cells["I1"].Value = "Email 1";
            sheet.Cells["J1"].Value = "Email 2";
            sheet.Cells["K1"].Value = "Mã Ngành";
            sheet.Cells["L1"].Value = "ĐTDĐ";
            sheet.Cells["M1"].Value = "ĐT Cha";
            sheet.Cells["N1"].Value = "ĐT Mẹ";
            sheet.Cells["O1"].Value = "Địa chỉ";
            sheet.Cells["A1:O1"].Style.Font.Bold = true;

            int row = 2;
            foreach (var item in sinhvien)
            {
                sheet.Cells[string.Format("A{0}", row)].Value = item.MSSV;
                sheet.Cells[string.Format("B{0}", row)].Value = item.Ho;
                sheet.Cells[string.Format("C{0}", row)].Value = item.Ten;
                sheet.Cells[string.Format("D{0}", row)].Value = item.NgaySinh;
                sheet.Cells[string.Format("E{0}", row)].Value = item.GioiTinh;
                sheet.Cells[string.Format("F{0}", row)].Value = item.TinhTrang.TenTinhTrang;
                sheet.Cells[string.Format("I{0}", row)].Value = item.Email_1;
                sheet.Cells[string.Format("J{0}", row)].Value = item.Email_2;
                sheet.Cells[string.Format("L{0}", row)].Value = item.DTDD;
                sheet.Cells[string.Format("M{0}", row)].Value = item.DTCha;
                sheet.Cells[string.Format("N{0}", row)].Value = item.DTMe;
                sheet.Cells[string.Format("O{0}", row)].Value = item.DiaChi;
                sheet.Cells[string.Format("H{0}", row)].Value = item.LopQuanLy.TenLop;
                sheet.Cells[string.Format("G{0}", row)].Value = "K" + item.LopQuanLy.KhoaDaoTao.Khoa;
                sheet.Cells[string.Format("K{0}", row)].Value = item.LopQuanLy.NganhDaoTao.MaNganh;
                row++;
            }
            sheet.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment; filename=" + "Danh sách sinh viên.xlsx");
            Response.BinaryWrite(ep.GetAsByteArray());
            Response.End();
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult XacNhanXoaKhoa(int id)
        {
            Session["KhoaSV"] = db.KhoaDaoTaos.Find(id).Khoa;
            return Redirect(Request.UrlReferrer.ToString());
        }
        [HttpPost]
        public ActionResult XoaKhoaSV(int IDKhoa)
        {
            var lopsv = db.LopQuanLies.Where(s => s.KhoaDaoTao.Khoa == IDKhoa).ToList();
            foreach (var item in lopsv)
            {
                var list = db.SinhViens.Where(s => s.ID_Lop == item.ID).ToList();
                foreach (var sv in list)
                {
                    db.SinhViens.Remove(sv);
                    db.SaveChanges();
                }
                db.LopQuanLies.Remove(item);
                db.SaveChanges();
            }
            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult XacNhanXoaNganh(int idnganh)
        {
            Session["NganhSV"] = db.NganhDaoTaos.Find(idnganh).Nganh;
            return Redirect(Request.UrlReferrer.ToString());
        }
        [HttpPost]
        public ActionResult XoaNganhSV(string idnganh, int idkhoa)
        {
            var lopsv = db.LopQuanLies.Where(s => s.KhoaDaoTao.ID == idkhoa).Where(s => s.NganhDaoTao.Nganh == idnganh).ToList();
            foreach (var item in lopsv)
            {
                var list = db.SinhViens.Where(s => s.ID_Lop == item.ID).ToList();
                foreach (var sv in list)
                {
                    db.SinhViens.Remove(sv);
                    db.SaveChanges();
                }
                db.LopQuanLies.Remove(item);
                db.SaveChanges();
            }
            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult XacNhanXoaLop(int idlop)
        {
            Session["LopSV"] = db.LopQuanLies.Find(idlop).TenLop;
            return Redirect(Request.UrlReferrer.ToString());
        }
        [HttpPost]
        public ActionResult XoaLopSV(string idlop)
        {
            var lopsv = db.LopQuanLies.Where(s => s.TenLop == idlop).ToList();
            foreach (var item in lopsv)
            {
                var list = db.SinhViens.Where(s => s.ID_Lop == item.ID).ToList();
                foreach (var sv in list)
                {
                    db.SinhViens.Remove(sv);
                    db.SaveChanges();
                }
                db.LopQuanLies.Remove(item);
                db.SaveChanges();
            }
            return Redirect(Request.UrlReferrer.ToString());
        }
        [HttpPost]
        public ActionResult UpdateSinhVien(FormCollection formCollection)
        {
            if (Request != null)
            {
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
                            if (noOfCol == 15 && noOfRow > 1)
                            {
                                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                                {
                                    if (workSheet.Cells[rowIterator, 1].Value != null)
                                    {
                                        var MSSV = workSheet.Cells[rowIterator, 1].Value.ToString();
                                        XoaSinhVien(MSSV);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                var listMoiSV = Session["LuuSinhVien"] as List<SinhVien>;
                foreach (var item in listMoiSV)
                    XoaSinhVien(item.MSSV);
            }
            if (Session["LuuSinhVien"] != null)
                TaiLenSinhVien();
            else
                UploadSinhVien(formCollection);
            Session["ThongBao"] = null;
            return RedirectToAction("KhoaSinhVien");
        }
        public ActionResult XoaSinhVien(string MSSV)
        {
            var SinhVien = db.SinhViens.FirstOrDefault(s => s.MSSV == MSSV);
            if (SinhVien != null)
            {
                db.SinhViens.Remove(SinhVien);
                db.SaveChanges();
            }
            return RedirectToAction("KhoaSinhVien");
        }
        public ActionResult LuuChuNhiem(int id)
        {
            Session["Lop"] = db.LopQuanLies.Find(id);
            return Redirect(Request.UrlReferrer.ToString());
        }
        [HttpPost]
        public ActionResult LuuChuNhiem(string chunhiem, int? idlop)
        {
            if (chunhiem == null)
            {
                return HttpNotFound();
            }
            if (idlop == null)
            {
                return HttpNotFound();
            }
            var Lop = db.LopQuanLies.Find(idlop);
            Lop.ChuNhiem = chunhiem;
            db.Entry(Lop).State = EntityState.Modified;
            db.SaveChanges();
            Session["Lop"] = null;
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