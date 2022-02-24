using Cap24Team3.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cap24Team3.Controllers
{
    public class SinhVienController : Controller
    {
        private Cap24 db = new Cap24();
        public ActionResult CapNhatThongTin()
        {
            var mail = User.Identity.Name;
            if (mail != null)
            {
                var sinhvien = db.SinhViens.FirstOrDefault(s => s.Email_1 == mail);
                if (sinhvien != null)
                {
                    var deadline = db.DotChinhSuaThongTins.Where(s => s.Lop == sinhvien.ID_Lop).Where(s => s.TinhTrang == true).ToList();
                    if (deadline.Count != 0)
                    {
                        foreach (var item in deadline)
                        {
                            if (DateTimeOffset.Now >= item.NgayBatDau && DateTimeOffset.Now <= item.NgayKetThuc)
                            {
                                ViewData["CN"] = item;
                                return View(sinhvien);
                            }
                        }
                    }
                }
            }
            TempData["Alert"] = "Chưa đến thời gian đăng ký";
            return Redirect(Request.UrlReferrer.ToString());
        }
        [HttpPost]
        public ActionResult CapNhatThongTin(int? capnhat, string mail, int? sdt, int? dtcha, int? dtme, string diachi)
        {
            var user = User.Identity.Name;
            if (user != null)
            {
                var sinhvien = db.SinhViens.FirstOrDefault(s => s.Email_1 == user);
                var thongtin = new ChinhSuaThongTin();
                if (sinhvien != null)
                {
                    if (mail != null)
                        thongtin.MailCaNhan = mail;
                    if (sdt != null)
                        thongtin.DTDD = sdt.ToString();
                    if (dtcha != null)
                        thongtin.DTCha = dtcha.ToString();
                    if (dtme != null)
                        thongtin.DTMe = dtme.ToString();
                    if (diachi != null)
                        thongtin.DiaChi = diachi;
                    thongtin.SinhVien = sinhvien;
                    thongtin.DotChinhSuaThongTin = db.DotChinhSuaThongTins.Find(capnhat);
                    db.Entry(thongtin).State = EntityState.Added;
                    db.SaveChanges();
                    return RedirectToAction("XemHocPhanDT");
                }
            }
            return RedirectToAction("XemHocPhanDT");
        }
        public ActionResult XemHocPhanDT()
        {
            string ListLoi = "";
            var mailsv = User.Identity.Name;
            var sinhvien = db.SinhViens.FirstOrDefault(s => s.Email_1 == mailsv);
            if (sinhvien != null)
            {
                var nganh = db.NganhDaoTaos.FirstOrDefault(s => s.ID == sinhvien.ID_Nganh);
                var khoa = db.KhoaDaoTaos.FirstOrDefault(s => s.ID == sinhvien.ID_Khoa);
                var ctdt = db.ChuongTrinhDaoTaos.Where(s => s.ID_Nganh == nganh.ID).FirstOrDefault(s => s.ID_Khoa == khoa.ID);
                if (ctdt == null)
                {
                    ListLoi += "<p> Chương trình đào tạo của Khóa K" + khoa.Khoa + " - Ngành " + nganh.Nganh + " bị trống</p>";
                    TempData["Alert"] = ListLoi;
                    return View();
                }
                else
                {
                    var hocPhanDaoTaos = db.HocPhanDaoTaos.Include(h => h.KhoiKienThuc).Include(h => h.HocPhanDaoTao2).Where(s => s.KhoiKienThuc.ChuongTrinhDaoTao.ID == ctdt.ID).ToList();
                    ViewData["NganhDaoTao"] = db.NganhDaoTaos.ToList();
                    ViewData["KhoaDaoTao"] = db.KhoaDaoTaos.ToList();
                    ViewData["HocKyDaoTao"] = db.HocKyDaoTaos.ToList();
                    ViewData["RangBuocHocPhan"] = db.RangBuocHocPhans.ToList();
                    var listHK = new List<string>();
                    var listHP = db.HocPhanDaoTaos.ToList();
                    foreach (var item in hocPhanDaoTaos)
                        if (!CheckTonTai(item.HocKy.ToString(), listHK))
                            listHK.Add(item.HocKy.ToString());
                    ViewData["listHK"] = listHK;
                    return View(hocPhanDaoTaos.ToList());
                }
            }
            else
            {
                TempData["Alert"] = "Bạn chưa đăng nhập!";
                return View();
            }
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
        public ActionResult XemDiemSV()
        {
            var mail = User.Identity.Name;
            var listdiem = new List<DiemHocPhan>();
            var listHK = new List<string>();
            var diemso2 = new List<string>();
            if (mail != null)
            {
                var sinhvien = db.SinhViens.FirstOrDefault(s => s.Email_1 == mail);
                if (sinhvien != null && db.DiemHocPhans.Where(s => s.MSSV == sinhvien.MSSV).Count() > 0)
                {
                    var list = db.DiemHocPhans.Where(s => s.MSSV == sinhvien.MSSV).ToList();
                    foreach (var item in list.OrderByDescending(s => s.ID))
                    {
                        string s = item.HocPhan + item.MSSV + item.HocKyChinhThuc;
                        if (!CheckTonTai(s, diemso2))
                        {
                            diemso2.Add(s);
                            listdiem.Add(item);
                        };
                        if (!CheckTonTai(item.HocKyChinhThuc.ToString(), listHK))
                            listHK.Add(item.HocKyChinhThuc.ToString());
                    }
                    var diemtb = new double[listHK.Count];
                    var diemtbchung = new double[listHK.Count];
                    var diemtong = new double[listHK.Count];
                    var sotinchi = new int[listHK.Count];
                    var somon = new int[listHK.Count];
                    for (int i = 0; i < listHK.Count; i++)
                    {
                        diemtb[i] = 0;
                        diemtong[i] = 0;
                        sotinchi[i] = 0;
                        somon[i] = 0;
                    }
                    for (int i = 0; i < listHK.Count; i++)
                    {
                        foreach (var item in listdiem)
                        {
                            if (item.HocKyChinhThuc.ToString() == listHK[i])
                            {
                                if (double.TryParse(item.Diem10, out double diem10))
                                {
                                    diemtong[i] += diem10;
                                    if (item.QuaMon == true)
                                        sotinchi[i] += (int)item.SoTinChi;
                                    somon[i]++;
                                }
                            }
                        }
                    }
                    for (int i = 0; i < diemtb.Length; i++)
                    {
                        diemtb[i] = Math.Round(diemtong[i] / somon[i], 2);
                    }
                    double Somon = 0;
                    double DiemTong = 0;
                    for (int i = 0; i < diemtb.Length; i++)
                    {
                        Somon += somon[i];
                        DiemTong += diemtong[i];
                        diemtbchung[i] = Math.Round(DiemTong / Somon, 2);
                    }
                    var nganh = db.NganhDaoTaos.FirstOrDefault(s => s.ID == sinhvien.ID_Nganh);
                    var khoa = db.KhoaDaoTaos.FirstOrDefault(s => s.ID == sinhvien.ID_Khoa);
                    var ctdt = db.ChuongTrinhDaoTaos.Where(s => s.ID_Nganh == nganh.ID).FirstOrDefault(s => s.ID_Khoa == khoa.ID);
                    var khoikienthucmoi = new List<string>();
                    foreach (var item in db.KhoiKienThucs.Where(s => s.ID_ChuongTrinhDaoTao == ctdt.ID).ToList())
                        if (!CheckTonTai(item.MaKhoiKienThuc, khoikienthucmoi))
                            khoikienthucmoi.Add(item.MaKhoiKienThuc);
                    var tongsotinchi = 0;
                    foreach (var item in khoikienthucmoi)
                    {
                        var ktt = db.KhoiKienThucs.FirstOrDefault(s => s.MaKhoiKienThuc == item);
                        foreach (var hocphan in db.HocPhanDaoTaos.Where(s => s.ID_KhoiKienThuc == ktt.ID).ToList())
                        {
                            if (hocphan.ID_HocPhanTuChon == null)
                                tongsotinchi += int.Parse(hocphan.SoTinChi.Split('T')[0]);
                        }
                    }
                    ViewData["listHK"] = listHK;
                    ViewData["DiemTB"] = diemtb;
                    ViewData["DiemTBChung"] = diemtbchung;
                    ViewData["SoTC"] = sotinchi;
                    ViewData["Tongsotinchi"] = tongsotinchi;
                }
            }
            return View(listdiem);
        }

        public ActionResult DangKyKHDT()
        {
            string ListLoi = "";
            var mailsv = User.Identity.Name;
            var sinhvien = db.SinhViens.FirstOrDefault(s => s.Email_1 == mailsv);
            if (sinhvien != null)
            {
                var nganh = db.NganhDaoTaos.FirstOrDefault(s => s.ID == sinhvien.ID_Nganh);
                var khoa = db.KhoaDaoTaos.FirstOrDefault(s => s.ID == sinhvien.ID_Khoa);
                var ctdt = db.ChuongTrinhDaoTaos.Where(s => s.ID_Nganh == nganh.ID).FirstOrDefault(s => s.ID_Khoa == khoa.ID);
                if (ctdt == null)
                {
                    ListLoi += "<p> Chương trình đào tạo của Khóa K" + khoa.Khoa + " - Ngành " + nganh.Nganh + " bị trống</p>";
                    TempData["Alert"] = ListLoi;
                    return View();
                }
                else
                {
                    var hocPhanDaoTaos = db.HocPhanDaoTaos.Include(h => h.KhoiKienThuc).Include(h => h.HocPhanDaoTao2).Where(s => s.KhoiKienThuc.ChuongTrinhDaoTao.ID == ctdt.ID).ToList();
                    ViewData["NganhDaoTao"] = db.NganhDaoTaos.ToList();
                    ViewData["KhoaDaoTao"] = db.KhoaDaoTaos.ToList();
                    ViewData["HocKyDaoTao"] = db.HocKyDaoTaos.ToList();
                    ViewData["RangBuocHocPhan"] = db.RangBuocHocPhans.ToList();
                    var listHK = new List<string>();
                    var listHP = db.HocPhanDaoTaos.ToList();
                    foreach (var item in hocPhanDaoTaos)
                        if (!CheckTonTai(item.HocKy.ToString(), listHK))
                            listHK.Add(item.HocKy.ToString());
                    ViewData["listHK"] = listHK;
                    TempData["Diemso"] = db.DiemHocPhans.Where(s => s.MSSV == sinhvien.MSSV);
                    TempData["Sinhvien"] = sinhvien;
                    return View(hocPhanDaoTaos.ToList());
                }
            }
            else
            {
                TempData["Alert"] = "Bạn chưa đăng nhập!";
                return View();
            }
        }
    }
}