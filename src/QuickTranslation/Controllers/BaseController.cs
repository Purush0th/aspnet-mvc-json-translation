using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace QuickTranslation.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext != null && filterContext.HttpContext != null && filterContext.HttpContext.Request != null)
            {
                string acceptLanguage = filterContext.HttpContext.Request.Headers["Accept-Language"];
                string cookieLanguage = filterContext.HttpContext.Request.Cookies["lang"]?.Value;

                // Use a method to prioritize language preferences
                string selectedCulture = GetPreferredCulture(cookieLanguage, acceptLanguage);

                // Set the culture for the current thread
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(selectedCulture);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(selectedCulture);
            }

            base.OnActionExecuting(filterContext);
        }

        private string GetPreferredCulture(string cookieLanguage, string acceptLanguage)
        {
            // Your logic to prioritize language preferences
            // For simplicity, let's assume the cookie value takes precedence over Accept-Language header
            return cookieLanguage ?? acceptLanguage.Split(',')[0]?.Trim() ?? "en-US";
        }
    }
}