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
            //TODO: Test behavior if the roleprovider cookie expires
            
            HttpContext ctx = HttpContext.Current;
            if (!ctx.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectResult("~/SessionTimeOut/Index");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}