using QuickTranslation.Utils;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;

namespace QuickTranslation.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}