using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ENETCare.Business;

namespace ENETCare.Presentation.MVC.Controllers
{
    public class ManagerControllerOld : Controller
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
            List<MedicationTypeViewData> list = new ReportBLL().GlobalStock();
            return View(list);
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