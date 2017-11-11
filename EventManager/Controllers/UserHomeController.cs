using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EventManager.Models;

namespace EventManager.Controllers
{
    public class UserHomeController : Controller
    {
        private EVENTS_MGR_TESTING_Entities _dc = new EVENTS_MGR_TESTING_Entities();
        
        // GET: UserHome
        public ActionResult Index()
        {
            string nameWithoutDomain = User.Identity.Name.Substring(User.Identity.Name.LastIndexOf(@"\") + 1);
            User user = _dc.Users.Where(u => u.LDAPName == nameWithoutDomain).FirstOrDefault();
            //return View(_dc.Events.Where(x => x.StartTime >= DateTime.Now)
            //    .OrderBy(x => x.StartTime)
            //    .ToList());
            return View(_dc.Events.Where(e => 
                                !_dc.Registrations.Any(r => r.UserID == user.UserId && r.EventID == e.EventID)
                                && e.StartTime >= DateTime.Now)
                .OrderBy(e => e.StartTime)
                .ToList());
        }
        public JsonResult GetEvents()
        {
            using (EVENTS_MGR_TESTING_Entities _dc = new EVENTS_MGR_TESTING_Entities())
            {
                _dc.Configuration.LazyLoadingEnabled = false;
                var events = _dc.Events.Where(x =>x.StartTime.Date >=DateTime.Now).GroupBy(x => x.StartTime.ToShortDateString()).ToList();
                return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
    }
}