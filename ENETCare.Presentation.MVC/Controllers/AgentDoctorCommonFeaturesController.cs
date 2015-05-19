using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ENETCare.Business;

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
        [HttpGet]
        public ActionResult AgentDoctorRegisterPackages()
        {
            List<MedicationType> list = new MedicationTypeBLL().GetMedicationTypeList();
            return View(list);
        }
        [HttpPost]
        public ActionResult AgentDoctorRegisterPackages(int MedicationType)
        {
            List<MedicationType> list = new MedicationTypeBLL().GetMedicationTypeList();
            string expireDate = "2015-12-31";
            new MedicationPackageBLL("agent1@enetcare.com").RegisterPackage(MedicationType, expireDate);
            return View(list);
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