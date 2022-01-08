using System.Web.Mvc;

namespace Cap24Team3.Areas.Faculty
{
    public class FacultyAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Faculty";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Faculty_default",
                "Faculty/{controller}/{action}/{id}",
                new { action = "ListCTDaoTao", controller = "ChuongTrinhDaoTao", id = UrlParameter.Optional }
            );
        }
    }
}