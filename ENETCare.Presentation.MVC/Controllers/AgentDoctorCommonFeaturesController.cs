using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using ENETCare.Business;
using ENETCare.Presentation.MVC.Models;
using System.Data.Entity.Validation;
using System.Globalization;

namespace ENETCare.Presentation.MVC.Controllers
{
    [Authorize(Roles = "Agent, Doctor")]
    public class AgentDoctorCommonFeaturesController : Controller
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

        #region Home Page

        public ActionResult MasterPage()
        {
            return View();
        }

        public ActionResult AgentDoctorHomePackages()
        {
            return View();
        }

        #endregion

        #region Register Package

        [HttpGet]
        public ActionResult AgentDoctorRegisterPackages()
        {
            var model = GetRegisteringViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult AgentDoctorRegisterPackages(int medicationTypeId, string expireDate, string submit)
        {
            var model = GetRegisteringViewModel();
            if (submit == "Refresh")
            {
                model.SelectedMedicationType = MedicationTypeBLL.GetMedicationTypeById(medicationTypeId);
            }
            else
            {
                try
                {
                    string barcode = MedicationPackageBLL.RegisterPackage(medicationTypeId, expireDate);
                    model.Barcode = barcode;
                    model.Result = new Notification { Level = NotificationLevel.Info, Message = "Register package succeeded" };
                }
                catch (ENETCareException ex)
                {
                    model.Result = new Notification { Level = NotificationLevel.Error, Message = ex.Message };
                }
            }
            ModelState.Clear();
            return View(model);
        }

        [HttpGet]
        public ActionResult BarcodeImage(string barcode)
        {
            FileContentResult result;
            using (var memStream = new MemoryStream())
            {
                Bitmap image = BarcodeHelper.GenerateBarcodeImage(barcode);
                image.Save(memStream, ImageFormat.Jpeg);
                result = File(memStream.GetBuffer(), "image/jpeg");
            }
            return result;
        }

        RegisteringViewModel GetRegisteringViewModel()
        {
            RegisteringViewModel model = new RegisteringViewModel();
            List<MedicationType> list = MedicationTypeBLL.GetMedicationTypeList();
            model.MedicationTypes = list;
            model.SelectedMedicationType = list.FirstOrDefault();
            return model;
        }

        #endregion

        #region Send Package

        [HttpGet]
        public ActionResult AgentDoctorSendPackage()
        {
            var model = GetSendingViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult AgentDoctorSendPackage(int distributionCenterId, string barcode)
        {
            var model = GetSendingViewModel();
            try
            {
                MedicationPackageBLL.SendPackage(barcode, distributionCenterId);
                model.Result = new Notification { Level = NotificationLevel.Info, Message = "Send package succeeded" };
            }
            catch (ENETCareException ex)
            {
                model.Result = new Notification { Level = NotificationLevel.Error, Message = ex.Message };
            }
            ModelState.Clear();
            return View(model);
        }

        SendingViewModel GetSendingViewModel()
        {
            SendingViewModel model = new SendingViewModel();
            List<DistributionCentre> list = DistributionCentreBLL.GetDistributionCentreList();
            model.DistributionCentres = list;
            return model;
        }

        #endregion

        #region Receive Package

        [HttpGet]
        public ActionResult AgentDoctorReceivePackage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AgentDoctorReceivePackage(string barcode)
        {
            var model = new ReceivingViewModel();
            try
            {
                MedicationPackageBLL.ReceivePackage(barcode);
                model.Result = new Notification { Level = NotificationLevel.Info, Message = "Receive package succeeded" };
            }
            catch (ENETCareException ex)
            {
                model.Result = new Notification { Level = NotificationLevel.Error, Message = ex.Message };
            }
            ModelState.Clear();
            return View(model);
        }

        #endregion

        #region Discard Package

        [HttpGet]
        public ActionResult AgentDoctorDiscardPackage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AgentDoctorDiscardPackage(string barcode)
        {
            var model = new DiscardingViewModel();
            try
            {
                MedicationPackageBLL.DiscardPackage(barcode);
                model.Result = new Notification { Level = NotificationLevel.Info, Message = "Discard package succeeded" };
            }
            catch (ENETCareException ex)
            {
                model.Result = new Notification { Level = NotificationLevel.Error, Message = ex.Message };
            }
            ModelState.Clear();
            return View(model);
        }

        #endregion

        #region To-Do: Distribute Package should be moved to doctor only feature
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

        #region Stocktaking

        [HttpGet]
        public ActionResult AgentDoctorViewReport()
        {
            List<StocktakingViewData> list = MedicationPackageBLL.Stocktake();
            return View(list);
        }

        #endregion

        public ActionResult AgentDoctorAuditPackageTask()
        {
            return View(MedicationTypeBLL.GetMedicationTypeList());
        }

        [HttpGet]
        public ActionResult AgentDoctorAuditPackageScan(int medicationTypeId, string medicationTypeName)
        {
            var model = new AuditViewModels();
            Session["auditMedicationTypeId"] = model.MedicationTypeId = medicationTypeId;
            Session["auditMedicationTypeName"] = model.MedicationTypeName = medicationTypeName;

            return View(model);
        }

        #region Auditing
        [HttpPost]
        public ActionResult AgentDoctorAuditPackageScan(string submitAction, AuditViewModels model)
        {
            model.MedicationTypeId = (int)Session["auditMedicationTypeId"];
            model.MedicationTypeName = Session["auditMedicationTypeName"] as string;

            if (submitAction == "Cancel")
            {
                _emptySessionStore();
                return View("AgentDoctorHomePackages");
            }
            else
            {
                if (submitAction == "Scan")
                {
                    if (!ModelState.IsValid)
                    {
                        return View(model);
                    }
                    _scanBarcode(model);
                    _clearInputField(model);
                }
                else if (submitAction == "Submit")
                {
                    _submitAudit(model);
                    _emptySessionStore();
                }
                return View(model);
            }
        }

        private void _clearInputField(AuditViewModels model)
        {
            ModelState.Remove("Barcode");
            model.Barcode = null;
        }

        private void _emptySessionStore()
        {
            Session["medicationPackageList"] = Session["auditMedicationTypeId"] = Session["auditMedicationTypeName"] = null;
        }

        private void _submitAudit(AuditViewModels model)
        {
            try
            {
                var scannedList = Session["medicationPackageList"] as List<string>;
                var lostPackages = MedicationPackageBLL.AuditPackages(model.MedicationTypeId, scannedList);
                if (lostPackages != null)
                {
                    model.LostPackages = lostPackages;
                }
                else
                {
                    model.LostPackages = new List<MedicationPackage>();
                }
            }
            catch (ENETCareException ex)
            {
                ModelState.AddModelError("", ex.Message.ToString());
            }
        }

        private void _scanBarcode(AuditViewModels model)
        {
            try
            {
                if (model.Barcode != null)
                {
                    MedicationPackageBLL.CheckAndUpdatePackage(model.MedicationTypeId, model.Barcode);
                    _storeScannedPackageInSession(model);
                }

            }
            catch (ENETCareException ex)
            {
                ModelState.AddModelError("", ex.Message.ToString());
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var error in ex.EntityValidationErrors)
                {
                    foreach (var validationError in error.ValidationErrors)
                    {
                        ModelState.AddModelError("", validationError.ErrorMessage.ToString());
                    }
                }
            }
        }

        private void _storeScannedPackageInSession(AuditViewModels model)
        {
            List<string> medicationPackageList;
            if (Session["medicationPackageList"] == null)
            {
                medicationPackageList = new List<string>();
            }
            else
            {
                medicationPackageList = Session["medicationPackageList"] as List<string>;

            }

            medicationPackageList.Add(model.Barcode);
            Session["medicationPackageList"] = medicationPackageList;
        }
        #endregion
    }
}