using ENETCare.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ENETCare.Presentation.MVC.Controllers.ManagerControllers
{
    public class ManagerDistributionCentreLossesController : Controller
    {
        private DistributionCentreBLL distributionCentresBLL;
        private ReportBLL reportBLL;

        public ManagerDistributionCentreLossesController()
        {
            distributionCentresBLL = new DistributionCentreBLL();
            reportBLL = new ReportBLL();
        }

        // GET: ManagerDistributionCentreReport
        public ActionResult Index()
        {
            return View(distributionCentresBLL.GetDistributionCentreList());
        }

        public ActionResult Details(int id, string name)
        {
            var distributionCentreStock = reportBLL.DistributionCentreStock(id);

            ViewBag.Name = name;
            ViewBag.Sum = distributionCentreStock.Sum(c => c.Value);

            if (distributionCentreStock == null)
            {
                return HttpNotFound();
            }

            return View(distributionCentreStock);
        }
    }
}