using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ENETCare.Business;
using System.Data.Entity;
using System.Diagnostics;
using System.Reflection;

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

        [HttpGet]
        public ActionResult AgentDoctorRegisterPackages()
        {
            List<MedicationType> list = new MedicationTypeBLL().GetMedicationTypeList();

            return View(list);
        }



        [HttpPost]
        public ActionResult AgentDoctorRegisterPackages(int MediTypeId, string AgentDoctorRegisterFormExpireDateInput)
        {
            List<MedicationType> list = new MedicationTypeBLL().GetMedicationTypeList();
            
            MedicationType selectedMedi = new MedicationTypeBLL().GetMedicationTypeById(MediTypeId);
            
            Debug.WriteLine("Name: -- " + selectedMedi.Name + "\n Default Expired Date: " + selectedMedi.DefaultExpireDate + "\n Value---: " + 
                selectedMedi.Value + "\n ID is: " + selectedMedi.ID + "\n ShelfLife ---: " + selectedMedi.ShelfLife);

            string expireDate = AgentDoctorRegisterFormExpireDateInput;
            new MedicationPackageBLL(User.Identity.Name).RegisterPackage(MediTypeId, expireDate);

            if (AgentDoctorRegisterFormExpireDateInput != null && AgentDoctorRegisterFormExpireDateInput != "")
            {
                Debug.WriteLine("AgentDoctorRegisterFormExpireDateInput -------- " + AgentDoctorRegisterFormExpireDateInput);
            }
            else
            {
                Debug.WriteLine("Even no string.");
            }

            ModelState.Clear();

            return View(list);
        }
        // GET: AgentDoctorSendPackage
        [HttpGet]
        public ActionResult AgentDoctorSendPackage()
        {
            List<DistributionCentre> list = new DistributionCentreBLL().GetDistributionCentreList();
            return View(list);
        }

        [HttpPost]
        public ActionResult AgentDoctorSendPackage(int sendToCenterId, string AgentDoctorSendPackageTypebarcodeInput)
        {
            List<DistributionCentre> list = new DistributionCentreBLL().GetDistributionCentreList();
            //DistributionCentre destinationCenter = new DistributionCentreBLL().GetDistributionCentreById(sendToCenterId);
            new MedicationPackageBLL(User.Identity.Name).SendPackage(AgentDoctorSendPackageTypebarcodeInput, sendToCenterId);

            return View(list);
        }
        // GET: AgentDoctorReceivePackage
        [HttpGet]
        public ActionResult AgentDoctorReceivePackage()
        {
            List<MedicationType> list = new MedicationTypeBLL().GetMedicationTypeList();
            return View(list);
        }

        [HttpPost]
        public ActionResult AgentDoctorReceivePackage(string AgentDoctorReceivePackagesBarcodeInput)
        {
            List<MedicationType> list = new MedicationTypeBLL().GetMedicationTypeList();
            new MedicationPackageBLL(User.Identity.Name).ReceivePackage(AgentDoctorReceivePackagesBarcodeInput, true);

            return View(list);
        }

        [HttpGet]
        public ActionResult DoctorDistributePackage()
        {
        
            return View();
        }

        [HttpPost]
        public ActionResult DoctorDistributePackage(string DoctorDistributePackageTypebarcode)
        {
            new MedicationPackageBLL(User.Identity.Name).DistributePackage(DoctorDistributePackageTypebarcode, true);
            return View();
        }

        [HttpGet]
        public ActionResult AgentDoctorAuditePackage()
        {
            List<MedicationType> list = new MedicationTypeBLL().GetMedicationTypeList();
            return View(list);
        }

        public ActionResult AgentDoctorViewReport()
        {
            List<MedicationTypeViewData> list = new ReportBLL().GlobalStock();
            return View(list);
        }

        [HttpGet]
        public ActionResult AgentDoctorDiscardPackage()
        {
            List<MedicationType> list = new MedicationTypeBLL().GetMedicationTypeList();
            return View(list);
        }



        public ActionResult SampleReport()
        {
            return View();
        }
    }
}