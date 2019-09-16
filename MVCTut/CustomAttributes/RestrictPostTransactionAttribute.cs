using Core;
using System.Web;
using System.Web.Mvc;

namespace MVCTut.CustomAttributes
{
    public class RestrictPostTransactionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!MvcApplication.IsBusinessOpen)
            {
                HttpContext.Current.Session["UserAccessInfo"] = "You cannot carry out Posting Transaction when business closed";
                filterContext.Result = new RedirectResult("~/Home/Index");
            }
            base.OnActionExecuting(filterContext);
        }
    }
    public class Ban : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!((User)HttpContext.Current.Session["User"]).IsAdmin)
            {
                HttpContext.Current.Session["Msg"] = "Unauthorized Access!";
                filterContext.Result = new RedirectResult("~/User/Login");
            }
            base.OnActionExecuting(filterContext);
        }
    }

}