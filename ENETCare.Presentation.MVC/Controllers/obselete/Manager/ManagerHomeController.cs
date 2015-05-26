using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ENETCare.Presentation.MVC.Controllers.ManagerControllers
{
    public class ManagerHomeController : Controller
    {
        // GET: ManagerHome
        public ActionResult Index()
        {
            return View();
        }
    }
}