using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ENETCare.Presentation.MVC.Controllers
{
    public class AgentDoctorCommonFeaturesController : Controller
    {
        // GET: AgentDoctorMasterPage
        public ActionResult MasterPage()
        {
            return View();
        }
        // GET: AgentDoctorHmePackage
        public ActionResult AgentDoctorHomePackages()
        {
            return View();
        }
        // GET: AgentDoctorRegisterPackage
        public ActionResult AgentDoctorRegisterPackages()
        {
            return View();
        }
        // GET: AgentDoctorSendPackage
        public ActionResult AgentDoctorSendPackage()
        {
            return View();
        }
        // GET: AgentDoctorReceivePackage
        public ActionResult AgentDoctorReceivePackage()
        {
            return View();
        }
    }
}