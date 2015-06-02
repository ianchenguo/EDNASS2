using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ENETCare.Presentation.MVC.Controllers;
using System.Web.Mvc;
using System.Collections.Generic;
using ENETCare.Business;

namespace ENETCareTest
{
    [TestClass]
    public class ControllerTest
    {
        [TestMethod]
        public void AccountController_login_returnUrl()
        {
            //Arrange
            AccountController ac = new AccountController();
            string url = "www.google.com";

            //Act
            ViewResult vr = ac.Login(url) as ViewResult;


            //Asert
            Assert.AreEqual(url, vr.ViewBag.ReturnUrl, "String does not match");
        }

        [TestMethod]
        public void ManagerController_DoctorList_returnAllDoctors()
        {
            //Arrange
            var mc = new ManagerController();
            var employeeBLL = new EmployeeBLL();

            //Act
            var result = mc.DoctorList() as ViewResult;
            var doctors = (List<Employee>)result.ViewData.Model;

            //Assert 4 doctors in the AspUser table
            Assert.AreEqual(4, doctors.Count);

        }

        [TestMethod]
        public void ManagerController_CentreList_returnDistributions()
        {
            //Arrange
            var mc = new ManagerController();
            var distributionCentresBLL = new DistributionCentreBLL();

            //Act
            var result = mc.CentreList() as ViewResult;
            var distributions = (List<DistributionCentre>)result.ViewData.Model;

            //Asert 5 distributions in the DistributionCentre
            Assert.AreEqual(5, distributions.Count);
            
        }

        [TestMethod]
        public void ManagerController_DistributionCentreStock_returnDistributionsStock()
        {
            //Arrange
            var mc = new ManagerController();
            var reportBLL = new ReportBLL();
            
            

            //Act
            var result = mc.DistributionCentreStock(1, "Head Office") as ViewResult;
            var distributionstocks = (List<MedicationTypeViewData>)result.ViewData.Model;
            var distributionCentreStocks = reportBLL.DistributionCentreStock(1);

            //Asert
            Assert.AreEqual("Head Office",result.ViewBag.Name);
            CollectionAssert.AllItemsAreNotNull(distributionstocks);
            Assert.AreEqual(distributionstocks.Count, distributionCentreStocks.Count);
            //// if medicationpackage is blank
            //Assert.AreEqual(0, result.ViewBag.Sum);

        }

        [TestMethod]
        public void ManagerController_DoctorActivity()
        {
            //Arrange
            var mc = new ManagerController();
            var reportBLL = new ReportBLL();

            //Act
            var result = mc.DoctorActivity("doctor4@enetcare.com") as ViewResult;
            var doctorconducted_pc = (List<MedicationTypeViewData>)result.ViewData.Model;
            var doctorconducted_pcs = reportBLL.DoctorActivity("doctor4@enetcare.com");
            //Asert
            Assert.AreEqual("doctor4@enetcare.com", result.ViewBag.Name);
            Assert.AreEqual(doctorconducted_pcs.Count, doctorconducted_pc.Count);
        }

        [TestMethod]
        public void ManagerController_GlobalStock()
        {
            //Arrange
            var mc = new ManagerController();
            var reportBLL = new ReportBLL();

            //Act
            var result = mc.GlobalStock() as ViewResult;
            var packages = (List<MedicationTypeViewData>)result.ViewData.Model;
            var allpackages = reportBLL.GlobalStock();
            //Asert
            Assert.IsNotNull(packages);
            Assert.AreEqual(packages.Count, allpackages.Count);
        }

        [TestMethod]
        public void ManagerController_ValueInTransit()
        {
            //Arrange
            var mc = new ManagerController();
            var reportBLL = new ReportBLL();

            //Act
            var result = mc.ValueInTransit() as ViewResult;
            var packages = (List<ValueInTransitViewData>)result.ViewData.Model;
            var transit_packages = reportBLL.ValueInTransit();

            //Asert
            Assert.IsNotNull(packages);
            Assert.AreEqual(packages.Count, transit_packages.Count);
        }

        [TestMethod]
        public void ManagerController_DistributionCentreLosses()
        {
            //Arrange
            var mc = new ManagerController();
            var reportBLL = new ReportBLL();

            //Act
            var result = mc.CentreList() as ViewResult;
            var packages = (List<DistributionCentre>)result.ViewData.Model;

            //Asert
            Assert.IsNotNull(packages);

        }
    }


}
