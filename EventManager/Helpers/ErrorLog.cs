using System;
using System.Web;
using Elmah;

namespace EventManager.Helpers
{
    public static class ErrorLog
    {
        ///<summary>
        ///Log Errors to ELMAH Instance
        ///</summary>
        public static void LogError(Exception ex, string contextualMessage = null)
        {
            try
            {
                //log error to Elmah
                if (contextualMessage != null)
                {
                    // log exception with contextual information that's visible when
                    // clicking the error in the Elmah log.
                    var annotatedException = new Exception(contextualMessage, ex);
                    ErrorSignal.FromCurrentContext().Raise(annotatedException, HttpContext.Current);
                }
                else
                {
                    ErrorSignal.FromCurrentContext().Raise(ex, HttpContext.Current);
                }
            }
            catch (Exception)
            {
                //just keep going
            }
        }
    }
}