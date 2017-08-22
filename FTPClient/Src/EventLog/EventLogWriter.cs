using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FTPClient.Src.EventLog
{
    public static class EventLogWriter
    {
        public static void SetEvent(LogEvents logEvent)
        {
            if (HttpContext.Current.Session["EventsLog"] != null)
            {
                List<LogEvents> events = (List<LogEvents>)HttpContext.Current.Session["EventsLog"];

                events.Add(logEvent);

                HttpContext.Current.Session["EventsLog"] = events;
            }
            else
            {
                HttpContext.Current.Session["EventsLog"] = new List<LogEvents>() { logEvent };
            }
        }
    }
}