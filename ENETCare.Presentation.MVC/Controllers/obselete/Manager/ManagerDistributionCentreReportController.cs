using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ENETCare.Business;

namespace ENETCare.Presentation.MVC.Controllers.ManagerControllers
{
    public class ManagerDistributionCentreReportController : Controller
    {

        private DistributionCentreBLL distributionCentresBLL;

        public ManagerDistributionCentreReportController()
        {
            distributionCentresBLL = new DistributionCentreBLL();
        }

        // GET: ManagerDistributionCentreReport
        public ActionResult Index()
        {
            return View(distributionCentresBLL.GetDistributionCentreList());
        }


    }
}