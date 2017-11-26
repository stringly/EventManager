using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EventManager.Models;
using System.Diagnostics;


namespace EventManager.Controllers
{
    public class LandingController : Controller
    {
        // GET: Landing
        public ActionResult Index()
        {
            
            DBInteractions db = new DBInteractions();
            db.PushUserToCache();
            return View();
        }
    }
}