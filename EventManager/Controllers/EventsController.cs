using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EventManager.Models;
using EventManager.Helpers;

namespace EventManager.Controllers
{

    
    
    //TODO: MOVE ALL DB Interactions into DB Model, all queries into STORED PROCS
    [SessionTimeout]
    public class EventsController : Controller
    {
        private EVENTS_MGR_TESTING_Entities db = new EVENTS_MGR_TESTING_Entities();

        public ActionResult AllEvents()
        {
            TempData["pageUser"] = new UserService().GetUserIDFromLDAP(User.Identity.Name.Substring(User.Identity.Name.LastIndexOf(@"\") + 1));
            return View(new EventService().GetAllEvents());
        }

        public ActionResult MyEvents()
        {
            int userID = new UserService().GetUserIDFromLDAP(User.Identity.Name.Substring(User.Identity.Name.LastIndexOf(@"\") + 1));
            return View(new EventService().GetMyEvents(userID));
        }

        public ActionResult Manage(int eventID)
        {
            return View(new EventService().GetEventToManage(eventID));
        }



        //CALENDARVIEW METHODS - ASYNC
        public ActionResult CalendarView()
        {
            TempData["pageUser"] = new UserService().GetUserIDFromLDAP(User.Identity.Name.Substring(User.Identity.Name.LastIndexOf(@"\") + 1));
            return View();
        }

        public JsonResult GetEvents()
        {
            using (EVENTS_MGR_TESTING_Entities dc = new EVENTS_MGR_TESTING_Entities())
            {
                dc.Configuration.LazyLoadingEnabled = false;
                var events = new EventService().GetCalendarEvents();
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

    
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        //OLD STUFF BELOW HERE - I added the calendarview stuff above

      
        public ActionResult UserEvents()
        {
            int userID = new UserService().GetUserIDFromLDAP(User.Identity.Name.Substring(User.Identity.Name.LastIndexOf(@"\") + 1));
            return View(db.USER_OWNED_EVENTS1(userID).ToList());
        }

        // GET: Events/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }
        public JsonResult GetEventDetails(int id)
        {

            //Event e = db.Events.Find(id);
            var e = db.Events.Where(u => u.EventID == id)
                            .Select(u => new {
                                EventID = u.EventID,
                                EventName = u.EventName,
                                StartTime = u.StartTime,
                                EndTime = u.EndTime,
                                Description = u.Description,
                                MaxStaff = u.MaxStaff,
                                MinStaff = u.MinStaff,
                                FundCenter = u.FundCenter,
                                DisplayColor = u.DisplayColor                                
                            }).Single();
            
                return new JsonResult { Data = e, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        // GET: Events/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EventID,EventName,StartTime,EndTime,Description,MaxStaff,MinStaff,FundCenter,EnteredBy")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Events.Add(@event);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(@event);
        }

        // GET: Events/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EventID,EventName,StartTime,EndTime,Description,MaxStaff,MinStaff,FundCenter,EnteredBy")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@event).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(@event);
        }

        // GET: Events/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Event @event = db.Events.Find(id);
            db.Events.Remove(@event);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        [HttpPost]
        public JsonResult UpdateRegistrations(List<RegistrationUpdateListItem> regList)

        {
            var result = false;
            DBInteractions db = new DBInteractions();
            result = db.UpdateRegistrationsByList(regList);

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult ConfirmRegistration(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DBInteractions db = new DBInteractions();
            var result = db.EditRegistration(Convert.ToInt32(id), RegistrationStatus.Confirmed);

            return RedirectToAction("UserEvents");
            
        }
        public ActionResult DeclineRegistration(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DBInteractions db = new DBInteractions();
            var result = db.EditRegistration(Convert.ToInt32(id), RegistrationStatus.Declined);

            return RedirectToAction("UserEvents");

        }
        public ActionResult StandbyRegistration(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DBInteractions db = new DBInteractions();
            var result = db.EditRegistration(Convert.ToInt32(id), RegistrationStatus.Standby);

            return RedirectToAction("UserEvents");

        }

    }
}
