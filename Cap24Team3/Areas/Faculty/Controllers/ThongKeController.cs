using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cap24Team3.Models;
using OfficeOpenXml;

namespace Cap24Team3.Areas.Faculty.Controllers
{
    [Authorize(Roles = "BCN Khoa")]
    public class ThongKeController : Controller
    {
        private Cap24 db = new Cap24();
        // GET: Faculty/ThongKe
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
        public ActionResult ThongKe()
        {
            var khoa = db.KhoaDaoTaos.ToList();
            var nganh = db.NganhDaoTaos.ToList();

            TempData["khoa"] = khoa;
            TempData["nganh"] = nganh;
            return View();
        }

        [HttpPost]
        public ActionResult XuatThongKe(int? khoa, int? nganh)
        {
            ExcelPackage ep = new ExcelPackage();
            var sheet = ep.Workbook.Worksheets.Add("Số liệu thống kê");
            var sv = db.SinhViens.ToList();
            if (khoa != null)
            {
                sv = sv.Where(s => s.LopQuanLy.ID_Khoa == khoa).ToList();
            }
            if (nganh != null)
            {
                sv = sv.Where(s => s.LopQuanLy.ID_Nganh == nganh).ToList();
            }
            sheet.Cells["A1"].Value = "Ngành";
            sheet.Cells["A2"].Value = "Khóa";
            sheet.Cells["B1"].Value = db.NganhDaoTaos.FirstOrDefault(n => n.ID == nganh).Nganh;
            sheet.Cells["B2"].Value = db.KhoaDaoTaos.FirstOrDefault(k => k.ID == khoa).Khoa;
            sheet.Cells["A3"].Value = "Môn học";
            sheet.Cells["B3"].Value = "Số lượng";

            string tshk = db.Thamsoes.FirstOrDefault(s => s.Ma == "HocKyHienTai").Giatri;
            int hkht = db.HocKyDaoTaos.FirstOrDefault(s => s.HocKy.ToString() == tshk).STT;
            var listsv = db.SinhViens.Where(s => s.KhoaDaoTao.ID == khoa).Where(s => s.NganhDaoTao.ID == nganh).ToList();
            var listthongke = new List<chitietthongke>();
            var thongke = new List<thongkehocphan>();
            var checkhp = new List<string>();
            var listhk = new List<string>();
            int d = 0;
            int row = 4;
            foreach (var sinhvien in listsv)
            {
                d++;
                int hksv = hkht - db.HocKyDaoTaos.FirstOrDefault(s => s.HocKy == sinhvien.HocKyBatDau).STT + 1;
                var listhp = db.DiemHocPhans.Where(s => s.HocKyDangKy > hksv).Where(s => s.MSSV == sinhvien.MSSV).ToList();
                foreach (var item in listhp)
                {
                    var cttk = new chitietthongke();
                    cttk.MSSV = sinhvien.MSSV;
                    cttk.tensv = sinhvien.Ho + " " + sinhvien.Ten;
                    cttk.mail = sinhvien.Email_1;
                    cttk.TenHP = item.TenHocPhan;
                    cttk.HocKy = db.HocKyDaoTaos.FirstOrDefault(s => s.STT == (item.HocKyDangKy + db.HocKyDaoTaos.FirstOrDefault(t => t.HocKy == sinhvien.HocKyBatDau).STT - 1)).HocKy.ToString();
                    if (item.HocKyDangKy > hksv)
                    {
                        listthongke.Add(cttk);
                    }
                }
            }
            foreach (var item in listthongke)
            {
                if (!CheckTonTai(item.TenHP, checkhp))
                {
                    var tk = new thongkehocphan();
                    tk.TenHP = item.TenHP;
                    tk.HocKy = item.HocKy;
                    tk.soluong = listthongke.Where(s => s.TenHP == item.TenHP).Count();
                    thongke.Add(tk);
                    checkhp.Add(item.TenHP);
                }
                if (!CheckTonTai(item.HocKy, listhk))
                {
                    listhk.Add(item.HocKy);
                }
            }
            foreach (var hk in listhk)
            {
                sheet.Cells[string.Format("A{0}", row)].Value = "Học kỳ";
                sheet.Cells[string.Format("B{0}", row)].Value = hk.ToString();
                row++;
                foreach (var item in thongke)
                {
                    if (item.HocKy == hk)
                    {
                        sheet.Cells[string.Format("A{0}", row)].Value = item.TenHP;
                        sheet.Cells[string.Format("B{0}", row)].Value = item.soluong;
                        row++;
                    }
                }
            }
            sheet.Cells["A:DZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment; filename=" + "Thống kê.xlsx");
            Response.BinaryWrite(ep.GetAsByteArray());
            Response.End();
            return Redirect(Request.UrlReferrer.ToString());
        }
        [HttpPost]
        public ActionResult LuuThongKe(int? khoa, int? nganh)
        {
            if (khoa is null)
            {
                throw new ArgumentNullException(nameof(khoa));
            }

            if (nganh is null)
            {
                throw new ArgumentNullException(nameof(nganh));
            }

            string tshk = db.Thamsoes.FirstOrDefault(s => s.Ma == "HocKyHienTai").Giatri;
            int hkht = db.HocKyDaoTaos.FirstOrDefault(s => s.HocKy.ToString() == tshk).STT;
            var listsv = db.SinhViens.Where(s => s.KhoaDaoTao.ID == khoa).Where(s => s.NganhDaoTao.ID == nganh).ToList();
            var listthongke = new List<chitietthongke>();
            var thongke = new List<thongkehocphan>();
            var checkhp = new List<string>();
            var listhk = new List<string>();
            int d = 0;
            foreach (var sinhvien in listsv)
            {
                d++;
                int hksv = hkht - db.HocKyDaoTaos.FirstOrDefault(s => s.HocKy == sinhvien.HocKyBatDau).STT + 1;
                var listhp = db.DiemHocPhans.Where(s => s.HocKyDangKy > hksv).Where(s => s.MSSV == sinhvien.MSSV).ToList();
                foreach (var item in listhp)
                {
                    var cttk = new chitietthongke();
                    cttk.MSSV = sinhvien.MSSV;
                    cttk.tensv = sinhvien.Ho + " " + sinhvien.Ten;
                    cttk.mail = sinhvien.Email_1;
                    cttk.TenHP = item.TenHocPhan;
                    cttk.HocKy = db.HocKyDaoTaos.FirstOrDefault(s => s.STT == (item.HocKyDangKy + db.HocKyDaoTaos.FirstOrDefault(t => t.HocKy == sinhvien.HocKyBatDau).STT - 1)).HocKy.ToString();
                    if (item.HocKyDangKy > hksv)
                    {
                        listthongke.Add(cttk);
                    }
                }
            }
            foreach (var item in listthongke)
            {
                if (!CheckTonTai(item.TenHP + item.HocKy, checkhp))
                {
                    var tk = new thongkehocphan();
                    tk.TenHP = item.TenHP;
                    tk.HocKy = item.HocKy;
                    tk.soluong = listthongke.Where(s => s.TenHP == item.TenHP).Where(s=>s.HocKy==item.HocKy).Count();
                    thongke.Add(tk);
                    checkhp.Add(item.TenHP + item.HocKy);
                }
                if (!CheckTonTai(item.HocKy, listhk))
                {
                    listhk.Add(item.HocKy);
                }
            }
            Session["Khoa"] = khoa;
            Session["Nganh"] = nganh;
            Session["listhk"] = listhk;
            Session["thongke"] = thongke;
            Session["chitiet"] = listthongke;
            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult XemChiTiet(string tenhp, string hk)
        {
            TempData["hk"] = hk;
            TempData["tenhp"] = tenhp;
            return View();
        }
    }
}