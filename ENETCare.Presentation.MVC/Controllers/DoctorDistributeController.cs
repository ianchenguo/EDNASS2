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
            var model = new DistributingViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult DoctorDistributePackage(string DoctorDistributePackageTypebarcode)
        {
            var model = new DistributingViewModel();
            try
            {
                MedicationPackageBLL.DistributePackage(DoctorDistributePackageTypebarcode);
                model.Result = new Notification { Level = NotificationLevel.Info, Message = "Distribute package succeeded" };
            }
            catch (ENETCareException ex)
            {
                model.Result = new Notification { Level = NotificationLevel.Error, Message = ex.Message };
            }
            ModelState.Clear();
            return View(model);
        }

        #endregion
    }
}