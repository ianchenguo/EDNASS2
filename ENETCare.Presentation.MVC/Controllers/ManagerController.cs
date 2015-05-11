using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ENETCare.Presentation.MVC.Controllers
{
    public class ManagerController : Controller
    {
        // GET: Manager
        public ActionResult ManagerHome()
        {
            return View();
        }

        public ActionResult ManagerDistributionCentreStock()
        {
            return View();
        }

        public ActionResult ManagerGlobalStock()
        {
            return View();
        }

        public ActionResult ManagerDistributionCentreLosses()
        {
            return View();
        }

        public ActionResult ManagerValueInTransit()
        {
            return View();
        }

        public ActionResult ManagerDoctorActivity()
        {
            return View();
        }

        public ActionResult ManagerDoctorActivityDoctors()
        {
            return View();
        }
    }
}