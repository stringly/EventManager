using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EventManager.Models;
using System.Diagnostics;
using EventManager.Helpers;
using EventManager.ViewModels;

namespace EventManager.Controllers
{
    
    public class UserInfoController : Controller
    {
        // GET: UserInfo
        [AllowAnonymous]
        public ActionResult UserInfo()
        {
            //TODO: store this in cookie?
            string nameWithoutDomain = User.Identity.Name.Substring(User.Identity.Name.LastIndexOf(@"\") + 1);
            UserInfoViewModel u = new UserService().GetUserInfoViewModelByLDAP(nameWithoutDomain);

            if (u == null) // I think this is causing an error unless I set all of the fields before the view is called
            {
                return HttpNotFound();
            }
            return View(u);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save([Bind(Include = "UserId,Rank,FirstName,LastName,IDNumber,PayrollID,Email,ContactNumber,LDAPName")] UserInfoViewModel @uvm)
        {
            bool result = false;
            if (ModelState.IsValid)
            {
                if (@uvm.UserId != 0) //is known user
                {
                    result = new UserService().UpdateUserFromViewModel(@uvm);
                }
                else //is new user
                {
                    result = new UserService().CreateUserFromViewModel(@uvm);
                }
                if (result)
                {
                    if (Request.Cookies["roleCookie"] != null) //overwrite the RoleProvider Cookie to refresh the User's Roles
                    {
                        HttpCookie newCookie = new HttpCookie("roleCookie");
                        newCookie.Expires = DateTime.Now.AddDays(-1d);
                        Response.Cookies.Add(newCookie);                        
                    }
                    return RedirectToAction("UserInfo");
                }
                else
                {
                    return HttpNotFound();
                }
            }
            else
            {
                return HttpNotFound();
            }
            
        }
    }
}