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
            //TODO: Change this to check for > 0 Roles in RoleProvider?
            
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