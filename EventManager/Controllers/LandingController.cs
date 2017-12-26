using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EventManager.Models;
using EventManager.Helpers;
using System.Diagnostics;


namespace EventManager.Controllers
{
    [Authorize]
    [SessionTimeout]
    public class LandingController : Controller
    {
        // GET: Landing
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (User.IsInRole("Development"))
            {
                return RedirectToAction("DevOpsHome", "Landing");
            }
            else if (User.IsInRole("Administrator"))
            {
                return RedirectToAction("AdminHome", "Landing");
            }
            else if (User.IsInRole("User"))
            {
                return RedirectToAction("UserHome", "Landing");
            }
            else
            {
                return View();
            }
                    
        }

        [Authorize(Roles = "User")]
        public ActionResult UserHome()
        {
            return View();
        }
        [Authorize(Roles = "Administrator")]
        public ActionResult AdminHome()
        {
            return View();
        }

        [Authorize(Roles = "Development")]
        public ActionResult DevOpsHome()
        {
            return View();
        }        
    }
}