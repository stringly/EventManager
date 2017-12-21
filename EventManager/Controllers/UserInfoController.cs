using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EventManager.Models;
using System.Diagnostics;
using EventManager.Helpers;

namespace EventManager.Controllers
{
    
    public class UserInfoController : Controller
    {
        // GET: UserInfo
        public ActionResult Index()
        {
            if (System.Web.HttpContext.Current.Cache["userID"] == null)
            {
                string nameWithoutDomain = User.Identity.Name.Substring(User.Identity.Name.LastIndexOf(@"\") + 1);
                User u = new User();
                u.UserId = 0;
                u.LDAPName = nameWithoutDomain;
                u.Rank = 1;
                u.Email = nameWithoutDomain + "@co.pg.md.us";
                return View(u); 
            }
            else
            {
                return RedirectToAction("KnownUser", Convert.ToInt32(System.Web.HttpContext.Current.Cache.Get("userID")));
            }
                         
        }
        [SessionTimeout]
        public ActionResult KnownUser(int id)
        {
            DBInteractions _db = new DBInteractions();
            User u = _db.GetUserByID(id);
            return View(u);
        }
        
        public JsonResult GetUser()
        {
            try
            {
                using (EVENTS_MGR_TESTING_Entities _dc = new EVENTS_MGR_TESTING_Entities())
                {
                    _dc.Configuration.LazyLoadingEnabled = false;
                    string nameWithoutDomain = User.Identity.Name.Substring(User.Identity.Name.LastIndexOf(@"\") + 1);
                    var u = _dc.Users.Where(a => a.LDAPName == nameWithoutDomain).FirstOrDefault();

                    if (u == null)
                    {
                        u = new User();
                        u.UserId = 0;
                        u.LDAPName = nameWithoutDomain;
                        u.Rank = 1;
                        u.Email = nameWithoutDomain + "@co.pg.md.us";
                        
                    } 
                    return new JsonResult { Data = u, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
            }
            catch(Exception x)
            {
                Debug.WriteLine(x.Message);
                return new JsonResult { Data = new { status = false } };
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save([Bind(Include = "EventID,EventName,StartTime,EndTime,Description,MaxStaff,MinStaff,FundCenter,EnteredBy")] User @user)
        {
            return View(@user);
        }

        [HttpPost]
        public JsonResult SaveUser(User u)
        {
            var status = false;
            DBInteractions db = new DBInteractions();
            if (u.UserId > 0)
            {
                //Existing User, call EditUser in DBInteractions
                status = db.EditUser(u);
            }
            else
            {
                //New User, call new user in DBInteractions

                status = db.AddNewUser(u);
                if (status == true)
                {
                    db.PushUserToCache();
                }
            }

            return new JsonResult { Data = new { status = status } };
        }


    }
}