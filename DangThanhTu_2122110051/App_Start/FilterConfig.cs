using System.Web;
using System.Web.Mvc;

namespace DangThanhTu_2122110051
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
