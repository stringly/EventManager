using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EventManager.Helpers;
using EventManager.Models;

namespace EventManager.Controllers
{
    //TODO: [Authorize(Roles = "Administrator")]
    [SessionTimeout]
    public class HomeController : Controller
    {
        // GET: Home
        
        public ActionResult Index()
        {
            return View();
        }

        //TODO: Move code in DB Context into DBInteractions class?
        //TODO: Make this a Stored Procedure to limit data returned?
        public JsonResult GetEvents()
        {
            using (EVENTS_MGR_TESTING_Entities dc = new EVENTS_MGR_TESTING_Entities())
            {
                dc.Configuration.LazyLoadingEnabled = false;
                //TODO: CREATE STORED PROC Limit the returned list of events to exclude full/past events
                var events = dc.Events.ToList();
                MessageFactory ms = new MessageFactory();
                return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
        }

        [HttpPost]
        public JsonResult SaveEvent(EventFormResult fr)
        {
            var status = false;
            var repeatStatus = false;            

            Event e = new EventManager.Event();
                e.EventID = Convert.ToInt32(fr.EventID);
                e.EventName = fr.EventName;
                e.StartTime = Convert.ToDateTime(fr.StartTime);
                e.EndTime = Convert.ToDateTime(fr.EndTime);
                e.Description = fr.Description;
                e.MaxStaff = Convert.ToInt32(fr.MaxStaff);
                e.MinStaff = Convert.ToInt32(fr.MinStaff);
                e.FundCenter = fr.FundCenter;
                //I changed this to not use the cache...
                //e.EnteredBy = Convert.ToInt32(System.Web.HttpContext.Current.Cache["userID"].ToString());
                e.EnteredBy = new UserService().GetUserIDFromLDAP(User.Identity.Name.Substring(User.Identity.Name.LastIndexOf(@"\") + 1));
                e.DisplayColor = fr.DisplayColor;

            EventRepeater r = new EventRepeater();
                r.repeatType = Convert.ToInt32(fr.repeatType);
                r.startDate = Convert.ToDateTime(fr.startDate);
                r.endDate = Convert.ToDateTime(fr.endDate);
                r.dow = fr.dow;
                r.repeatCount = Convert.ToInt32(fr.repeatCount);

            DBInteractions db = new DBInteractions();

            if (e.EventID == 0)
            {
                
                status = db.AddNewEvent(e);
            }
            else
            {
                    status = db.EditExistingEvent(e);                
            }

            if (r != null)
            {
                repeatStatus = db.RepeatEvent(e, r);
            }
            MessageFactory ms = new MessageFactory(status);

            return new JsonResult { Data = new { status = status, message = ms.GenerateMessage() } };
        }

        [HttpPost]
        public JsonResult DeleteEvent(int eventID)
        {
            var status = false;
            DBInteractions db = new DBInteractions();
            status = db.DeleteEvent(eventID);
            MessageFactory ms = new MessageFactory(status);
            return new JsonResult { Data = new { status = status, message = ms.GenerateMessage() } };
        }
    }
}