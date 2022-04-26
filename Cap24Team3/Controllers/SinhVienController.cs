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
        public ActionResult XemThongTinChiTiet()
        {
            var mail = User.Identity.Name;
            if (mail != null)
            {
                var sinhvien = db.SinhViens.FirstOrDefault(s => s.Email_1 == mail);
                if (sinhvien != null)
                {
                    return View(sinhvien);
                }
            }
            return Redirect(Request.UrlReferrer.ToString());
        }
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
            TempData["Alert3"] = "Chưa đến thời gian đăng ký";
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
                    foreach (var item in hocPhanDaoTaos.OrderBy(s => s.HocKy))
                    {
                        if (!CheckTonTai(item.HocKy.ToString(), listHK))
                        {
                            listHK.Add(item.HocKy.ToString());
                        }
                    }
                    //var listKKT = new List<string>();
                    //foreach (var item in hocPhanDaoTaos.OrderBy(s => s.ID_KhoiKienThuc))
                    //{
                    //    if (!CheckTonTai(item.KhoiKienThuc.TenKhoiKienThuc, listKKT))
                    //    {
                    //        listKKT.Add(item.KhoiKienThuc.TenKhoiKienThuc);
                    //    }
                    //}
                    ViewData["listHK"] = listHK;
                    ViewData["KhoiKienThucXem"] = db.KhoiKienThucs.Where(k => k.ID_ChuongTrinhDaoTao == ctdt.ID).ToList();
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
                    foreach (var item in list)
                    {
                        string s = item.HocPhan + item.MSSV + item.HocKyDangKy;
                        if (!CheckTonTai(s, diemso2))
                        {
                            diemso2.Add(s);
                            listdiem.Add(item);
                        };
                        if (!CheckTonTai(item.HocKyDangKy.ToString(), listHK))
                            listHK.Add(item.HocKyDangKy.ToString());
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
                            if (item.HocKyKeHoach.ToString() == listHK[i])
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
                    var trongct = new List<DiemHocPhan>();
                    var ngoaict = new List<DiemHocPhan>();
                    var check = new List<string>();
                    var ctdt = db.ChuongTrinhDaoTaos.Where(s => s.ID_Nganh == nganh.ID).FirstOrDefault(s => s.ID_Khoa == khoa.ID);
                    var khoikienthucmoi = new List<string>();
                    foreach (var item in db.KhoiKienThucs.Where(s => s.ID_ChuongTrinhDaoTao == ctdt.ID).ToList())
                        if (!CheckTonTai(item.MaKhoiKienThuc, khoikienthucmoi))
                            khoikienthucmoi.Add(item.MaKhoiKienThuc);
                    var tongsotinchi = 0;
                    var khoikienthuc = new List<string>();
                    var stcktt = new List<int>();
                    foreach (var item in khoikienthucmoi)
                    {
                        var ktt = db.KhoiKienThucs.FirstOrDefault(s => s.MaKhoiKienThuc == item);
                        foreach (var hocphan in db.HocPhanDaoTaos.Where(s => s.ID_KhoiKienThuc == ktt.ID).ToList())
                        {
                            if (hocphan.ID_HocPhanTuChon == null)
                                tongsotinchi += int.Parse(hocphan.SoTinChi.Split('T')[0]);
                            if (hocphan.MaHocPhan != null)
                                check.Add(hocphan.MaHocPhan.Trim());
                        }
                        khoikienthuc.Add(ktt.TenKhoiKienThuc);
                    }
                    foreach (var item in listdiem)
                    {
                        if (item.HocPhan != null)
                            if (CheckTonTai(item.HocPhan.Trim(), check))
                                trongct.Add(item);
                            else
                                ngoaict.Add(item);
                        else trongct.Add(item);
                    }
                    foreach (var item in khoikienthuc)
                    {
                        int sl = db.HocPhanDaoTaos.Where(s => s.KhoiKienThuc.TenKhoiKienThuc == item).Count();
                        stcktt.Add(sl);
                    }
                    ViewData["listHK"] = listHK;
                    ViewData["DiemTB"] = diemtb;
                    ViewData["DiemTBChung"] = diemtbchung;
                    ViewData["SoTC"] = sotinchi;
                    ViewData["Tongsotinchi"] = tongsotinchi;
                    TempData["Khoikienthuc"] = khoikienthuc;
                    TempData["sotc"] = stcktt;
                    TempData["soluong"] = "0:" + (khoikienthuc.Count() - 1) + ":1";
                    TempData["trong"] = trongct;
                    TempData["ngoai"] = ngoaict;
                }
            }
            return View(listdiem);
        }

        public ActionResult DangKyKHDT()
        {
            string ListLoi = "";
            var mailsv = User.Identity.Name;
            var sinhvien = db.SinhViens.FirstOrDefault(s => s.Email_1 == mailsv);
            string tshk = db.Thamsoes.FirstOrDefault(s => s.Ma == "HocKyHienTai").Giatri;
            int hockyhientai = db.HocKyDaoTaos.FirstOrDefault(s => s.HocKy.ToString() == tshk).STT - db.HocKyDaoTaos.FirstOrDefault(s => s.HocKy == sinhvien.HocKyBatDau).STT + 1;
            if (sinhvien != null)
            {
                var nganh = db.NganhDaoTaos.FirstOrDefault(s => s.ID == sinhvien.ID_Nganh);
                var khoa = db.KhoaDaoTaos.FirstOrDefault(s => s.ID == sinhvien.ID_Khoa);
                var diemhp = db.DiemHocPhans.Where(s => s.MSSV == sinhvien.MSSV).ToList();
                if (diemhp == null)
                {
                    ListLoi += "<p> Chương trình đào tạo của Khóa K" + khoa.Khoa + " - Ngành " + nganh.Nganh + " bị trống</p>";
                    TempData["Alert"] = ListLoi;
                    return View();
                }
                else
                {
                    ViewData["NganhDaoTao"] = db.NganhDaoTaos.ToList();
                    ViewData["KhoaDaoTao"] = db.KhoaDaoTaos.ToList();
                    ViewData["HocKyDaoTao"] = db.HocKyDaoTaos.ToList();
                    ViewData["RangBuocHocPhan"] = db.RangBuocHocPhans.ToList();
                    var listHK = new List<hk>();
                    var listHK2 = new List<string>();
                    var listHP = db.HocPhanDaoTaos.ToList();
                    var listHPDiem = new List<string>();
                    var ListHKDK = new List<string>();
                    int maxhk = 18;
                    if (maxhk - hockyhientai >= 6) maxhk = 15;
                    else if (maxhk - hockyhientai >= 3) maxhk = 12;
                    else maxhk = 18;
                    for (int i = hockyhientai + 1; i <= maxhk; i++)
                    {
                        string hkc = db.HocKyDaoTaos.FirstOrDefault(s => s.STT == (i + db.HocKyDaoTaos.FirstOrDefault(t => t.HocKy == sinhvien.HocKyBatDau).STT - 1)).HocKy.ToString();
                        ListHKDK.Add(hkc);
                    }
                    var listDiem = new List<string>();
                    foreach (var item in diemhp.OrderByDescending(s => s.ID).ToList())
                    {
                        if (!CheckTonTai(item.HocKyDangKy.ToString(), listHK2))
                        {
                            listHK2.Add(item.HocKyDangKy.ToString());
                            hk hk = new hk();
                            hk.stt = item.HocKyDangKy;
                            hk.HocKy = item.HocKyDangKy.ToString();
                            var hk2 = item.HocKyDangKy + db.HocKyDaoTaos.FirstOrDefault(i => i.HocKy == sinhvien.HocKyBatDau).STT - 1;
                            hk.HocKy1 = db.HocKyDaoTaos.FirstOrDefault(s => s.STT == hk2).HocKy.ToString();
                            listHK.Add(hk);
                        }
                        string check = item.HocPhan.Trim() + item.HocKyDangKy.ToString();
                        if (CheckTonTai(check, listDiem))
                        {
                            db.DiemHocPhans.Remove(item);
                            db.SaveChanges();
                        }
                        else
                        {
                            listDiem.Add(check);
                        }
                        listHPDiem.Add(item.HocPhan.Trim());
                    }
                    var ctdt = db.ChuongTrinhDaoTaos.Where(s => s.NganhDaoTao.Nganh == nganh.Nganh).FirstOrDefault(s => s.KhoaDaoTao.Khoa == khoa.Khoa);
                    var hocphan = new List<HocPhanDaoTao>();
                    foreach (var item in db.KhoiKienThucs.Where(s => s.ID_ChuongTrinhDaoTao == ctdt.ID).ToList())
                    {
                        foreach (var hp in db.HocPhanDaoTaos.Where(s => s.ID_KhoiKienThuc == item.ID).ToList())
                        {
                            hocphan.Add(hp);
                        }
                    }
                    foreach (var item in hocphan)
                    {
                        if (item.MaHocPhan != null)
                            if (!CheckTonTai(item.MaHocPhan.Trim(), listHPDiem))
                            {
                                DiemHocPhan dhp = new DiemHocPhan();
                                dhp.MSSV = sinhvien.MSSV;
                                dhp.HocPhan = item.MaHocPhan.Trim();
                                dhp.TenHocPhan = item.TenHocPhan;
                                dhp.HocKyDangKy = hockyhientai + 1;
                                dhp.HocKyKeHoach = hockyhientai + 1;
                                if (item.HocPhanDaoTao2 == null)
                                    dhp.BBTC = true;
                                else
                                    dhp.BBTC = false;
                                dhp.SoTinChi = int.Parse(item.SoTinChi.Split('T').First());
                                dhp.LichSu = db.LichSuUpLoads.OrderByDescending(s => s.ID).First().ID;
                                dhp.QuaMon = false;
                                db.DiemHocPhans.Add(dhp);
                                db.SaveChanges();
                            }
                    }
                    ViewData["listHK"] = listHK.OrderBy(s => s.stt).ToList();
                    ViewData["HocKyHienTai"] = hockyhientai;
                    TempData["ListHKDK"] = ListHKDK.ToArray();
                    ViewData["maxhk"] = maxhk;
                    TempData["Diemso"] = diemhp;
                    TempData["Sinhvien"] = sinhvien;
                    return View(diemhp.ToList());
                }
            }
            else
            {
                TempData["Alert"] = "Bạn chưa đăng nhập!";
                return View();
            }
        }

        public ActionResult LuuHP(string hocky)
        {
            var diem = db.DiemHocPhans.Find((Session["HocPhanDC"] as DiemHocPhan).ID);
            diem.HocKyDangKy = int.Parse(hocky);
            db.Entry(diem).State = EntityState.Modified;
            db.SaveChanges();
            Session["HocPhanDC"] = null;
            return RedirectToAction("DangKyKHDT");
        }
        public ActionResult SuaHK(int id)
        {
            Session["HocPhanDC"] = db.DiemHocPhans.Find(id);
            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}