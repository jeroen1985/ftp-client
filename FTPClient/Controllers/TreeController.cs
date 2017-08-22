using FTPClient.Src;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FTPClient.Controllers
{
    public class TreeController : Controller
    {
        // GET: Tree/
        public ActionResult Local(string path)
        {
            try
            {
                LocalTree tree = new LocalTree(path);
                List<BaseNode> localTree = tree.GetTree();
                return Json(localTree, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {              
                return HttpNotFound(e.Message);
            }
           
        }
     
    }
}
