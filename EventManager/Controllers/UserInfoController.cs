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
        [AllowAnonymous]
        public ActionResult Index()
        {
            //TODO: store this in cookie?
            string nameWithoutDomain = User.Identity.Name.Substring(User.Identity.Name.LastIndexOf(@"\") + 1);
            User u = new DBInteractions().GetUserByLDAP(nameWithoutDomain);
            
            if (u == null) // I think this is causing an error unless I set all of the fields before the view is called
            {
                u = new EventManager.User();
                u.UserId = 0;
                u.LDAPName = nameWithoutDomain;
                u.Rank = 1;
                u.Email = nameWithoutDomain + "@co.pg.md.us";                 
            }
            return View(u);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save([Bind(Include = "UserId,Rank,FirstName,LastName,IDNumber,PayrollID,Email,ContactNumber,LDAPName")] User @user)
        {
            if (ModelState.IsValid)
            {
                if(@user.UserId != 0) //is known user
                {
                    @user.EditUser();
                }
                else //is new user
                {
                    @user.CreateUser();
                    if (Request.Cookies["roleCookie"] != null) //overwrite the RoleProvider Cookie to refresh the User's Roles
                    {
                        HttpCookie newCookie = new HttpCookie("roleCookie");
                        newCookie.Expires = DateTime.Now.AddDays(-1d);
                        Response.Cookies.Add(newCookie);
                    }                    
                }
            }
            return RedirectToAction("Index");
        }
    }
}