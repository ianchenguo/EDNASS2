using ENETCare.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ENETCare.Presentation.MVC.Controllers
{
    [Authorize(Roles="Manager")]
    public class ManagerController : Controller
    {
        private DistributionCentreBLL distributionCentresBLL;
        private ReportBLL reportBLL;
        private EmployeeBLL employeeBLL;

        public ManagerController()
        {
            distributionCentresBLL = new DistributionCentreBLL();
            reportBLL = new ReportBLL();
            employeeBLL = new EmployeeBLL();
        }

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        // GET: CentreList
        public ActionResult CentreList()
        {
            return View(distributionCentresBLL.GetDistributionCentreList());
        }

        // GET: DoctorList
        public ActionResult DoctorList()
        {
            return View(employeeBLL.GetEmployeeListByRole(Role.Doctor));
        }

        // GET: DistributionCentreStock
        public ActionResult DistributionCentreStock(int id, string name)
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

        // GET: DoctorActivity
        public ActionResult DoctorActivity(string name)
        {
            var doctoryActivity = reportBLL.DoctorActivity(name);

            ViewBag.Name = name;
            ViewBag.Sum = doctoryActivity.Sum(c => c.Value);

            if (doctoryActivity == null)
            {
                return HttpNotFound();
            }

            return View(doctoryActivity);
        }

        // GET: GlobalStock
        public ActionResult GlobalStock()
        {
            var globalStock = reportBLL.GlobalStock();
            ViewBag.Sum = globalStock.Sum(c => c.Value);
            return View(globalStock);
        }

        // GET: DistributionCentreLosses
        public ActionResult DistributionCentreLosses()
        {
            return View(reportBLL.DistributionCentreLosses());
        }

        // GET: ValueInTransit
        public ActionResult ValueInTransit()
        {
            var valueInTransit = reportBLL.ValueInTransit();
            ViewBag.Sum = valueInTransit.Sum(c => c.Value);
            return View(valueInTransit);
        }
    }
}