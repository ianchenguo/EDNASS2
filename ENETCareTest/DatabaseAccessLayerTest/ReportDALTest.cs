using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ENETCare.Business;

namespace ENETCareTest
{
	[TestClass]
	public class ReportDALTest : DatabaseAccessLayerTest
	{
		IReportDAO dao;

		[TestInitialize]
		public void Setup()
		{
			dao = new ReportEntityFrameworkDAO();
		}

		#region PrepareTestData

		protected override void PrepareTestData()
		{
		}

		#endregion

		[TestMethod()]
		public void ReportDAO_FindDistributionCentreStockByStatus_ReturnsCorrectList()
		{
			Assert.Fail("Should be removed after implementing this unit test");
		}

		[TestMethod()]
		public void ReportDAO_FindGlobalStock_ReturnsCorrectList()
		{
			Assert.Fail("Should be removed after implementing this unit test");
		}

		[TestMethod()]
		public void ReportDAO_FindDoctorActivityByUserName_ReturnsCorrectList()
		{
			Assert.Fail("Should be removed after implementing this unit test");
		}

		[TestMethod()]
		public void ReportDAO_FindAllValueInTransit_ReturnsCorrectList()
		{
			Assert.Fail("Should be removed after implementing this unit test");
		}
	}
}
