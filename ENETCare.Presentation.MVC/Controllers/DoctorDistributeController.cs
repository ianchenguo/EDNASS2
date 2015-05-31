using System.Web.Mvc;
using ENETCare.Business;
using ENETCare.Presentation.MVC.Models;

namespace ENETCare.Presentation.MVC.Controllers
{
    [Authorize(Roles = "Doctor")]
    public class DoctorDistributeController : Controller
    {
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

        [HttpGet]
        public ActionResult DoctorDistributePackage()
        {
            var model = new DistributingViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult DoctorDistributePackage(string barcode)
        {
            var model = new DistributingViewModel();
            try
            {
                MedicationPackageBLL.DistributePackage(barcode);
                model.Result = new Notification { Level = NotificationLevel.Info, Message = "Distribute package succeeded" };
            }
            catch (ENETCareException ex)
            {
                model.Result = new Notification { Level = NotificationLevel.Error, Message = ex.Message };
            }
            ModelState.Clear();
            return View(model);
        }
    }
}