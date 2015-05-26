using ENETCare.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ENETCare.Presentation.MVC.Controllers.ManagerControllers
{
    public class ManagerGlobalStockReportController : Controller
    {
        private ReportBLL reportBLL;

        public ManagerGlobalStockReportController()
        {
            reportBLL = new ReportBLL();
        }

        // GET: ManagerGlobalStockReport
        public ActionResult Index()
        {
            var globalStock = reportBLL.GlobalStock();
            ViewBag.Sum = globalStock.Sum(c => c.Value);
            return View(globalStock);
        }
    }
}