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
    public class ChuNhiemLopController : Controller
    {
        private Cap24 db = new Cap24();
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
        public ActionResult DanhSachLop()
        {
            var mailGV = User.Identity.Name;
            var lopQuanLies = db.LopQuanLies.Include(l => l.KhoaDaoTao).Include(l => l.NganhDaoTao).Where(l => l.ChuNhiem == mailGV);
            var Lop = new string[lopQuanLies.Count()];
            var SiSo = new int[lopQuanLies.Count()];
            var idLop = new int[lopQuanLies.Count()];
            int stt = 0;
            foreach (var item in lopQuanLies)
            {
                Lop[stt] = item.TenLop;
                idLop[stt] = item.ID;
                stt++;
            }
            for (int i = 0; i < Lop.Length; i++)
            {
                foreach (var item in db.SinhViens.ToList())
                {
                    if (item.LopQuanLy.TenLop == Lop[i])
                    {
                        SiSo[i]++;
                    }
                }
            }
            ViewData["DanhSachLop"] = Lop;
            ViewData["SiSo"] = SiSo;
            ViewData["idLop"] = idLop;
            return View(lopQuanLies.ToList());
        }
        public ActionResult DanhSachSV(int idLop)
        {
            ViewData["DSTT"] = db.TinhTrangs.ToList();
            var danhSachSV = db.SinhViens.Where(s => s.LopQuanLy.ID == idLop).OrderBy(s => s.ID_TinhTrang).ToList();
            return View(danhSachSV);
        }
        public ActionResult DoiTinhTrang(int id)
        {
            Session["SinhVienTT"] = db.SinhViens.Find(id);
            return Redirect(Request.UrlReferrer.ToString());
        }
        [HttpPost]
        public ActionResult XacNhanDoiTinhTrang(int? tinhtrang, int? idSV)
        {
            if (idSV == null)
            {
                return HttpNotFound();
            }
            if (tinhtrang == null)
            {
                return HttpNotFound();
            }
            var sinhvien = db.SinhViens.Find(idSV);
            sinhvien.ID_TinhTrang = tinhtrang;
            db.Entry(sinhvien).State = EntityState.Modified;
            db.SaveChanges();
            Session["SinhVienTT"] = null;
            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult DanhSachDotChinhSua()
        {
            var chinhSuaThongTins = db.DotChinhSuaThongTins.Include(s => s.LopQuanLy);
            return View(chinhSuaThongTins.ToList());
        }
        public ActionResult TaoDotChinhSua()
        {
            var mailGV = User.Identity.Name;
            ViewData["LopQuanLy"] = db.LopQuanLies.Where(l => l.ChuNhiem == mailGV).ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TaoDotChinhSua(DotChinhSuaThongTin dotChinhSuaThongTin, int[] id)
        {
            if (ModelState.IsValid)
            {
                if (dotChinhSuaThongTin.NgayKetThuc > dotChinhSuaThongTin.NgayBatDau)
                {
                    for (int i = 0; i < id.Length; i++)
                    {
                        dotChinhSuaThongTin.NguoiTao = User.Identity.Name;
                        dotChinhSuaThongTin.DotChinhSua = "Đợt chỉnh sửa tháng " + dotChinhSuaThongTin.NgayBatDau.Month;
                        dotChinhSuaThongTin.TinhTrang = true;
                        dotChinhSuaThongTin.LopQuanLy = db.LopQuanLies.Find(id[i]);
                        db.Entry(dotChinhSuaThongTin).State = EntityState.Added;
                        db.SaveChanges();
                    }
                    return RedirectToAction("DanhSachDotChinhSua");
                }
            }
            return View(dotChinhSuaThongTin);
        }
        public ActionResult XemChiTietSV(int id)
        {
            var sv = db.SinhViens.Find(id);
            Session["sinhvien"] = sv;
            Session["tinhtrang"] = db.SinhViens.Find(id).TinhTrang.TenTinhTrang;
            var dcn = db.DotChinhSuaThongTins.Where(s => s.LopQuanLy.TenLop == sv.LopQuanLy.TenLop).ToList();
            Session["dotcapnhat"] = dcn;
            var capnhattt = new List<ChinhSuaThongTin>();
            if (dcn.Count != 0)
                foreach (var item in dcn)
                {
                    var cn = db.ChinhSuaThongTins.Where(s => s.ID_DotChinhSua == item.ID).Where(s => s.ID_SinhVien == sv.ID).ToList().OrderByDescending(s => s.ID);
                    if (cn.Count() != 0)
                        capnhattt.Add(cn.First());
                }
            if (capnhattt != null)
                Session["capnhattt"] = capnhattt;
            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult XemDiemSinhVien(int id)
        {
            var sinhvien = db.SinhViens.Find(id);
            var nganh = db.NganhDaoTaos.FirstOrDefault(s => s.ID == sinhvien.ID_Nganh);
            var khoa = db.KhoaDaoTaos.FirstOrDefault(s => s.ID == sinhvien.ID_Khoa);
            var ctdt = db.ChuongTrinhDaoTaos.Where(s => s.ID_Nganh == nganh.ID).FirstOrDefault(s => s.ID_Khoa == khoa.ID);
            var hocPhanDaoTaos = db.HocPhanDaoTaos.Where(s => s.KhoiKienThuc.ChuongTrinhDaoTao.ID == ctdt.ID).ToList();
            ViewData["NganhDaoTao"] = db.NganhDaoTaos.ToList();
            ViewData["KhoaDaoTao"] = db.KhoaDaoTaos.ToList();
            ViewData["HocKyDaoTao"] = db.HocKyDaoTaos.ToList();
            Session["HocPhan"] = db.HocPhanDaoTaos.ToList();
            var list = new List<DiemHocPhan>();
            if (db.DiemHocPhans.Where(s => s.MSSV == sinhvien.MSSV).Count() > 0)
                list = db.DiemHocPhans.Where(s => s.MSSV == sinhvien.MSSV).ToList();
            var listHK = new List<string>();
            var listHP = db.HocPhanDaoTaos.ToList();
            var diemso2 = new List<DiemHocPhan>();
            var l = new List<string>();
            foreach (var item in list.OrderByDescending(s => s.ID))
            {
                if (!CheckTonTai(item.HocKyChinhThuc.ToString(), listHK))
                    listHK.Add(item.HocKyChinhThuc.ToString());
                string s = item.HocPhan + item.MSSV + item.HocKyChinhThuc;
                if (!CheckTonTai(s, l))
                {
                    l.Add(s);
                    diemso2.Add(item);
                };
            }

            Session["sinhvien1"] = sinhvien;
            Session["diemso"] = list;
            var listdiem = new List<DiemHocPhan>();
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
                foreach (var item in diemso2)
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
            Session["listHK"] = listHK;
            ViewData["DiemTB"] = diemtb;
            ViewData["DiemTBChung"] = diemtbchung;
            Session["SoTC"] = sotinchi;
            ViewData["Tongsotinchi"] = tongsotinchi;
            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult XacNhanXoaSV(int id)
        {
            Session["XoaSV"] = db.SinhViens.Find(id).ID;
            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult XoaSVKhoiLop(int id)
        {
            var sv = db.SinhViens.Find(id);
            Session["sinhvien2"] = sv;
            //ViewBag.TinhTrang = db.TinhTrangs.ToList();
            if (sv.TinhTrang.TenTinhTrang == "Còn học")
            {
                var ChuyenLop = db.TinhTrangs.FirstOrDefault(t => t.TenTinhTrang == "Chuyển lớp");
                sv.ID_TinhTrang = ChuyenLop.ID;
                db.SaveChanges();
            }
            return Redirect(Request.UrlReferrer.ToString());
        }
        [HttpPost]
        public ActionResult ThemSinhVienVaoLop(int idLop, string SinhVien)
        {
            var sv = db.SinhViens.FirstOrDefault(s => s.MSSV == SinhVien);
            if (sv != null)
            {
                var ConHoc = db.TinhTrangs.FirstOrDefault(t => t.TenTinhTrang == "Còn học");
                sv.ID_TinhTrang = ConHoc.ID;
                sv.ID_Lop = idLop;
                db.SaveChanges();
            }
            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult CapNhatDCS(string txt, int id)
        {
            var dcs = db.DotChinhSuaThongTins.Find(id);
            if (txt == "Dong")
                dcs.TinhTrang = false;
            else
                dcs.TinhTrang = true;
            db.Entry(dcs).State = EntityState.Modified;
            db.SaveChanges();
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
