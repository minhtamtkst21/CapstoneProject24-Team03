using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;

namespace Cap24Team3
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Trang chu",
                url: "trang-chu",
                defaults: new { controller = "ThongBaos", action = "NhanThongBao", id = UrlParameter.Optional },
                namespaces: new[] { "Cap24Team3.Controllers" }
            );
            routes.MapRoute(
                name: "Chuong trinh dao tao",
                url: "Chuong-trinh-dao-tao",
                defaults: new { controller = "SinhVien", action = "XemHocPhanDT", id = UrlParameter.Optional },
                namespaces: new[] { "Cap24Team3.Controllers" }
            );
            routes.MapRoute(
                name: "Xem diem",
                url: "Xem-diem",
                defaults: new { controller = "SinhVien", action = "XemDiemSV", id = UrlParameter.Optional },
                namespaces: new[] { "Cap24Team3.Controllers" }
            );
            routes.MapRoute(
                name: "Dang ky KHHT",
                url: "Dang-ky-ke-hoach-hoc-tap",
                defaults: new { controller = "SinhVien", action = "DangKyKHDT", id = UrlParameter.Optional },
                namespaces: new[] { "Cap24Team3.Controllers" }
            );
            routes.MapRoute(
                name: "Xem thong tin chi tiet",
                url: "Thong-tin-chi-tiet",
                defaults: new { controller = "SinhVien", action = "XemThongTinChiTiet", id = UrlParameter.Optional },
                namespaces: new[] { "Cap24Team3.Controllers" }
            );
            routes.MapRoute(
                name: "Cap nhat thong tin",
                url: "Cap-nhat-thong-tin-ca-nhan",
                defaults: new { controller = "SinhVien", action = "CapNhatThongTin", id = UrlParameter.Optional },
                namespaces: new[] { "Cap24Team3.Controllers" }
            );
            routes.MapRoute(
                name: "Danh sach lop chu nhiem",
                url: "Danh-sach-lop",
                defaults: new { controller = "ChuNhiemLop", action = "DanhSachLop", id = UrlParameter.Optional },
                namespaces: new[] { "Cap24Team3.Controllers" }
            );
            routes.MapRoute(
                name: "Danh sach sinh vien chu nhiem",
                url: "Danh-sach-sinh-vien-lop-{idLop}",
                defaults: new { controller = "ChuNhiemLop", action = "DanhSachSV", id = UrlParameter.Optional },
                namespaces: new[] { "Cap24Team3.Controllers" }
            );
            routes.MapRoute(
                name: "Danh sach note sinh vien chu nhiem",
                url: "Sinh-vien-theo-doi-lop-{idLop}",
                defaults: new { controller = "ChuNhiemLop", action = "DanhSachNoteSV", id = UrlParameter.Optional },
                namespaces: new[] { "Cap24Team3.Controllers" }
            );
            routes.MapRoute(
                name: "Xem diem thong ke",
                url: "Thong-ke-lop-{idLop}",
                defaults: new { controller = "ChuNhiemLop", action = "XemDiemThongKe", id = UrlParameter.Optional },
                namespaces: new[] { "Cap24Team3.Controllers" }
            );
            routes.MapRoute(
                name: "Quan ly dot chinh sua",
                url: "Quan-ly-dot-chinh-sua",
                defaults: new { controller = "ChuNhiemLop", action = "DanhSachDotChinhSua", id = UrlParameter.Optional },
                namespaces: new[] { "Cap24Team3.Controllers" }
            );
            routes.MapRoute(
                name: "Tao dot chinh sua",
                url: "Tao-dot-chinh-sua",
                defaults: new { controller = "ChuNhiemLop", action = "TaoDotChinhSua", id = UrlParameter.Optional },
                namespaces: new[] { "Cap24Team3.Controllers" }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "ThongBaos", action = "NhanThongBao", id = UrlParameter.Optional }
            );
        }
    }
}
