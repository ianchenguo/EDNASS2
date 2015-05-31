using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ENETCare.Business;
using ENETCare.Presentation.MVC.Models;

namespace ENETCare.Presentation.MVC.Controllers
{
    [Authorize(Roles = "Doctor")]

    public class DoctorDistributeController : Controller
    {
        #region Properties

        MedicationPackageBLL medicationPackageBLL;
        MedicationPackageBLL MedicationPackageBLL
        {
            get
            {
                if (medicationPackageBLL == null)
                {
                    medicationPackageBLL = new MedicationPackageBLL(User.Identity.Name);
                }
                return medicationPackageBLL;
            }
        }

        MedicationTypeBLL medicationTypeBLL;
        MedicationTypeBLL MedicationTypeBLL
        {
            get
            {
                if (medicationTypeBLL == null)
                {
                    medicationTypeBLL = new MedicationTypeBLL();
                }
                return medicationTypeBLL;
            }
        }

        DistributionCentreBLL distributionCentreBLL;
        DistributionCentreBLL DistributionCentreBLL
        {
            get
            {
                if (distributionCentreBLL == null)
                {
                    distributionCentreBLL = new DistributionCentreBLL();
                }
                return distributionCentreBLL;
            }
        }

        #endregion

        #region
        public ActionResult DoctorMasterPage()
        {
            return View();
        }
        #endregion

        #region Doctor Distribute Package

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

        #endregion
    }
}