using FTPClient.Src.EventLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FTPClient.Controllers
{ 
    public class EventLogController : Controller
    {
        
        // GET: EventLog
        public ActionResult Index()
        {
            EventLogResponse eventLog = new EventLogResponse();
 
            if (Session["Connected"] != null)
            {
                eventLog.Connected = true;
                eventLog.Host = (string)Session["Host"];
                eventLog.Port = (int)Session["Port"];
                eventLog.UserName = (String)Session["UserName"];
            }

            if (Session["EventsLog"] != null)
            {
                eventLog.EventsLog = (List<LogEvents>)Session["EventsLog"];
                Session["EventsLog"] = null;
            }

            return Json(eventLog, JsonRequestBehavior.AllowGet);
        }
    }
}