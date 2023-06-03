using System.Web;
using System.Web.Mvc;

namespace Punto_de_venta__POS_
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
