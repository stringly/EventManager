using System.Web.Mvc;
using EventManager.Helpers;
using System.Net;

namespace EventManager.Controllers
{
    [SessionTimeout]
    public class UserEventRegistrationController : Controller
    {

        /// <summary>
        /// Main method for this controller; Uses the Event Service to show a list of AvailableEventViewModel objects for the current user
        /// </summary>
        /// <returns>View:UserEventRegistration/AvailableEvents</returns>
        [HttpGet]
        [Authorize(Roles="User")]
        public ActionResult AvailableEvents()
        {
            return View(new EventService().GetAvailableEventsByLDAP(User.Identity.Name.Substring(User.Identity.Name.LastIndexOf(@"\") + 1)));
        }
        /// <summary>
        /// Uses the Registration Service to show a list of RegistrationsForUserViewModel objects for the current user
        /// </summary>
        /// <returns>View:UserEventRegistration/Registrations</returns>
        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult Registrations()
        {
            return View(new RegistrationService().GetCurrentRegistrationsByLDAP(User.Identity.Name.Substring(User.Identity.Name.LastIndexOf(@"\") + 1)));
        }
        /// <summary>
        /// POST Method that allows a User to register for an event.
        /// </summary>
        /// <param name="eventID">The DB eventID of the event for which the user is trying to register</param>
        /// <returns>RedirectToView:UserEventRegistration/AvailableEvents</returns>
        [HttpPost]
        [Authorize(Roles = "User")]
        [ValidateAntiForgeryToken]        
        public ActionResult Register(int eventID)
        {
            int userID = new UserService().GetUserIDFromLDAP(User.Identity.Name.Substring(User.Identity.Name.LastIndexOf(@"\") + 1));
            bool success = new RegistrationService().CreateRegistration(userID, eventID);

            if (success)
            {
                CreateTempDataMessage(RegistrationStatus.Pending);
                return RedirectToAction("AvailableEvents");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }            
        }
        /// <summary>
        /// POST Method that allows a User to update the status of a registration
        /// </summary>
        /// <param name="registrationID">The DB registrationId of the registration being updated</param>
        /// <param name="oldStatus">The current status of the registration being updated</param>
        /// <param name="newStatus">The new status that the user is trying to assign to the registration</param>
        /// <returns>RedirectToView:UserEventRegistration/Registrations</returns>
        [HttpPost]
        [Authorize(Roles = "User")]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateRegistration(int registrationID, RegistrationStatus oldStatus, RegistrationStatus newStatus)
        {
            bool success = new RegistrationService().UpdateRegistrationStatus(registrationID, newStatus);

            if (success)
            {
                CreateTempDataMessage(newStatus);
                return RedirectToAction("Registrations");

            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

        }
        /// <summary>
        /// Method that creates TempData info to pass to a redirect so that a message can be displayed
        /// </summary>
        /// <param name="nStatus">The status of the registration that was altered/created</param>
        public void CreateTempDataMessage(RegistrationStatus nStatus)
        {
            switch (nStatus)
            {
                case RegistrationStatus.Confirmed:
                    TempData["Status"] = "Success!";
                    TempData["Message"] = "Registration was confirmed.";
                    break;
                case RegistrationStatus.Declined:
                    TempData["Status"] = "Success!";
                    TempData["Message"] = "Registration was declined.";
                    break;
                case RegistrationStatus.Deleted:
                    TempData["Status"] = "Success!";
                    TempData["Message"] = "Registration was deleted.";
                    break;
                case RegistrationStatus.Standby:
                    TempData["Status"] = "Success!";
                    TempData["Message"] = "You have been placed on standby for this event.";
                    break;
                case RegistrationStatus.TransferPending:
                    TempData["Status"] = "Success!";
                    TempData["Message"] = "You have submitted a request to transfer this registration.";
                    break;
                case RegistrationStatus.WithdrawlPending:
                    TempData["Status"] = "Success!";
                    TempData["Message"] = "You have submitted a request to withdraw from this event.";
                    break;
                case RegistrationStatus.Pending:
                    TempData["Status"] = "Success!";
                    TempData["Message"] = "You have been registered for this event.";
                    break;
            }
        }
    }
}