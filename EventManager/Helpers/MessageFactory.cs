using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;

namespace EventManager.Helpers
{
    public class MessageFactory
    {
        public string CallerFunctionName { get; set; }
        public bool Status { get; set; }

        public MessageFactory()
        {
            StackTrace st = new StackTrace();
            this.CallerFunctionName = st.GetFrame(1).GetMethod().Name;
            this.Status = true;
        }
        
        public MessageFactory(bool status)
        {
            StackTrace st = new StackTrace();
            this.CallerFunctionName = st.GetFrame(1).GetMethod().Name;
            this.Status = status;             
        }
        //TODO: ActionResult Message passing? Maybe viewbag?
        public string GenerateMessage()
        {
            Debug.WriteLine("MessageFactory was called by: " + CallerFunctionName);
            switch (CallerFunctionName)
            {
                case "SaveEvent":
                    if (Status)
                    {
                        return "Event added successfully.";
                                               
                    }
                    else
                    {
                        return "Event could not be added. Please try again. Contact the administrator if the issue persists.";
                                              
                    }
                case "DeleteEvent":
                    if (Status)
                    {
                        return "Event deleted successfully";
                    }
                    else
                    {
                        return "Event could not be deleted. Please try again. Contact the administrator if the issue persists.";
                    }
                case "CancelRegistration":
                    if (Status)
                    {
                        return "Registration Cancelled successfully";
                    }
                    else
                    {
                        return "Registration could not be cancelled. Please try again. Contact the administrator if the issue persists.";
                    }
            }
            return "";            
        }
    }
}