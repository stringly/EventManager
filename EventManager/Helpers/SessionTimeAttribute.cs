using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EventManager.Helpers
{
    public class SessionTimeoutAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;
            if (ctx.Cache["userID"] == null)
            {
                filterContext.Result = new RedirectResult("~/SessionTimeOut/Index");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}