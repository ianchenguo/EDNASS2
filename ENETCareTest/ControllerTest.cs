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
            Assert.AreEqual(url, vr.ViewBag.ReturnUrl, "Does not match");
        }

        [TestMethod]
        public void ManagerController_CentreList_returnDistributions()
        {
            //Arrange
            var mc = new ManagerController();
            var distributionCentresBLL = new DistributionCentreBLL();

            //Act
            var result = mc.CentreList() as ViewResult;
            var products = (List<DistributionCentre>)result.ViewData.Model;

            //Asert
            Assert.IsNotNull(products);
            
        }

        [TestMethod]
        public void ManagerController_DistributionCentreStock_returnDistributionsStock()
        {
            //Arrange
            var mc = new ManagerController();
            

            //Act
            var result = mc.DistributionCentreStock(1, "Head Office") as ViewResult;
            

            //Asert
            Assert.AreEqual("Head Office",result.ViewBag.Name);

        }

        [TestMethod]
        public void ManagerController_DoctorActivity()
        {
            //Arrange
            var mc = new ManagerController();


            //Act
            var result = mc.DoctorActivity("doctor4@enetcare.com") as ViewResult;
            var distributions = (List<MedicationTypeViewData>)result.ViewData.Model;

            //Asert
            Assert.AreEqual("doctor4@enetcare.com", result.ViewBag.Name);
            Assert.IsNotNull(distributions);
        }

        [TestMethod]
        public void ManagerController_GlobalStock()
        {
            //Arrange
            var mc = new ManagerController();


            //Act
            var result = mc.GlobalStock() as ViewResult;
            var packages = (List<MedicationTypeViewData>)result.ViewData.Model;

            //Asert
            Assert.AreEqual(0, result.ViewBag.Sum);
            Assert.IsNotNull(packages);
        }

        [TestMethod]
        public void ManagerController_ValueInTransit()
        {
            //Arrange
            var mc = new ManagerController();


            //Act
            var result = mc.ValueInTransit() as ViewResult;
            var packages = (List<ValueInTransitViewData>)result.ViewData.Model;

            //Asert
            Assert.AreEqual(0, result.ViewBag.Sum);
            Assert.IsNotNull(packages);
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
