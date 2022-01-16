using Cap24Team3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cap24Team3.Controllers
{
    public class DiemSinhVienController : Controller
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
        public ActionResult XemDiemSV()
        {
            var mail = User.Identity.Name;
            var listdiem = new List<DiemHocPhan>();
            var listHK = new List<string>();
            if (mail != null)
            {
                var sinhvien = db.SinhViens.FirstOrDefault(s => s.Email_1 == mail);
                if (sinhvien != null)
                {
                    var list = db.DiemHocPhans.Where(s => s.MSSV == sinhvien.MSSV).ToList();
                    foreach (var item in list)
                    {
                        listdiem.Add(item);
                        if (!CheckTonTai(item.HocKy.ToString(), listHK))
                            listHK.Add(item.HocKy.ToString());
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
                        foreach (var item in list)
                        {
                            if (item.HocKy.ToString() == listHK[i])
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
                    ViewData["listHK"] = listHK;
                    ViewData["DiemTB"] = diemtb;
                    ViewData["DiemTBChung"] = diemtbchung;
                    ViewData["SoTC"] = sotinchi;
                }
            }
            return View(listdiem);
        }
    }
}