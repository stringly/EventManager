using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EventManager.Models;

namespace EventManager.Controllers
{
    public class EventsController : Controller
    {
        private EVENTS_MGR_TESTING_Entities db = new EVENTS_MGR_TESTING_Entities();
        private int userID = Convert.ToInt32(System.Web.HttpContext.Current.Cache["userID"]);
        // GET: Events
        public ActionResult Index()
        {
            ViewBag.CurrentUser = userID;
            //return View(db.Events.ToList());
            if (User.IsInRole("Development")){
                 return View(db.EVENTS_LAST_6_MONTHS1().ToList());
            }
            else
            {
                return View(db.Events.ToList());
            }
            
        }
        public ActionResult UserEvents()
        {
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
                db.Entry(@event).State = EntityState.Modified;
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
            var result = db.EditRegistration(Convert.ToInt32(id), RegistrationStats.Confirmed);

            return RedirectToAction("UserEvents");
            
        }
        public ActionResult DeclineRegistration(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DBInteractions db = new DBInteractions();
            var result = db.EditRegistration(Convert.ToInt32(id), RegistrationStats.Declined);

            return RedirectToAction("UserEvents");

        }
        public ActionResult StandbyRegistration(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DBInteractions db = new DBInteractions();
            var result = db.EditRegistration(Convert.ToInt32(id), RegistrationStats.Standby);

            return RedirectToAction("UserEvents");

        }
    }
}
