using System;
using System.Collections.Generic;
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
			DeleteExistingData();
			CreateTestDistributionCentres();
			CreateTestMedicationTypes();
			CreateTestMedicationPackages();
		}

		protected override void CreateTestMedicationPackages()
		{
			int dc1 = 1;
			int dc2 = 2;
			int type1 = 1;
			int type2 = 2;
			int type3 = 3;
			DateTime expireDate = new DateTime(2015, 12, 31);
			DateTime updateTime = TimeProvider.Current.Now;

			// DC1 Packages
			InsertMedicationPackage("10001000", type1, expireDate, PackageStatus.InStock, dc1, null, null, "doctor1", updateTime);
			InsertMedicationPackage("10001001", type1, expireDate, PackageStatus.InStock, dc1, null, null, "doctor1", updateTime);
			InsertMedicationPackage("20001000", type2, expireDate, PackageStatus.InStock, dc1, null, null, "doctor1", updateTime);
			InsertMedicationPackage("30001000", type3, expireDate, PackageStatus.InStock, dc1, null, null, "doctor1", updateTime);
			InsertMedicationPackage("10001002", type1, expireDate, PackageStatus.Distributed, dc1, null, null, "doctor1", updateTime);
			InsertMedicationPackage("10001003", type1, expireDate, PackageStatus.Discarded, dc1, null, null, "doctor1", updateTime);
			InsertMedicationPackage("10001004", type1, expireDate, PackageStatus.Lost, dc1, null, null, "doctor1", updateTime);

			// DC2 Packages
			InsertMedicationPackage("20001001", type2, expireDate, PackageStatus.InStock, dc2, null, null, "doctor2", updateTime);
			InsertMedicationPackage("30001001", type3, expireDate, PackageStatus.InStock, dc2, null, null, "doctor2", updateTime);

			// In Transit Packages
			InsertMedicationPackage("10001005", type1, expireDate, PackageStatus.InTransit, null, dc1, dc2, "doctor1", updateTime);
			InsertMedicationPackage("20001002", type2, expireDate, PackageStatus.InTransit, null, dc1, dc2, "doctor1", updateTime);
			InsertMedicationPackage("10001006", type1, expireDate, PackageStatus.InTransit, null, dc2, dc1, "doctor2", updateTime);
		}

		#endregion

		[TestMethod()]
		public void ReportDAO_FindDistributionCentreStockByStatus_ReturnsCorrectList()
		{
			// InStock Packages
			List<MedicationTypeViewData> list1 = dao.FindDistributionCentreStockByStatus(1, PackageStatus.InStock);

			Assert.AreEqual(3, list1.Count);

			Assert.AreEqual("TEST MT1", list1[0].Type);
			Assert.AreEqual(2, list1[0].Quantity);
			Assert.AreEqual(4000, list1[0].Value);

			Assert.AreEqual("TEST MT2", list1[1].Type);
			Assert.AreEqual(1, list1[1].Quantity);
			Assert.AreEqual(1000, list1[1].Value);

			Assert.AreEqual("TEST MT3", list1[2].Type);
			Assert.AreEqual(1, list1[2].Quantity);
			Assert.AreEqual(300, list1[2].Value);

			// Discarded and Lost Packages
			List<MedicationTypeViewData> list2 = dao.FindDistributionCentreStockByStatus(1, PackageStatus.Discarded, PackageStatus.Lost);

			Assert.AreEqual(1, list2.Count);

			Assert.AreEqual("TEST MT1", list2[0].Type);
			Assert.AreEqual(2, list2[0].Quantity);
			Assert.AreEqual(4000, list2[0].Value);
		}

		[TestMethod()]
		public void ReportDAO_FindGlobalStock_ReturnsCorrectList()
		{
			List<MedicationTypeViewData> list = dao.FindGlobalStock();

			Assert.AreEqual(3, list.Count);

			Assert.AreEqual("TEST MT1", list[0].Type);
			Assert.AreEqual(2, list[0].Quantity);
			Assert.AreEqual(4000, list[0].Value);

			Assert.AreEqual("TEST MT2", list[1].Type);
			Assert.AreEqual(2, list[1].Quantity);
			Assert.AreEqual(2000, list[1].Value);

			Assert.AreEqual("TEST MT3", list[2].Type);
			Assert.AreEqual(2, list[2].Quantity);
			Assert.AreEqual(600, list[2].Value);
		}

		[TestMethod()]
		public void ReportDAO_FindDoctorActivityByUserName_ReturnsCorrectList()
		{
			List<MedicationTypeViewData> list1 = dao.FindDoctorActivityByUserName("doctor1");
			Assert.AreEqual(1, list1.Count);
			Assert.AreEqual("TEST MT1", list1[0].Type);
			Assert.AreEqual(1, list1[0].Quantity);
			Assert.AreEqual(2000, list1[0].Value);

			List<MedicationTypeViewData> list2 = dao.FindDoctorActivityByUserName("doctor2");
			Assert.AreEqual(0, list2.Count);
		}

		[TestMethod()]
		public void ReportDAO_FindAllValueInTransit_ReturnsCorrectList()
		{
			List<ValueInTransitViewData> list = dao.FindAllValueInTransit();

			Assert.AreEqual(2, list.Count);

			Assert.AreEqual("TEST DC1", list[0].FromDistributionCentre);
			Assert.AreEqual("TEST DC2", list[0].ToDistributionCentre);
			Assert.AreEqual(2, list[0].Packages);
			Assert.AreEqual(3000, list[0].Value);

			Assert.AreEqual("TEST DC2", list[1].FromDistributionCentre);
			Assert.AreEqual("TEST DC1", list[1].ToDistributionCentre);
			Assert.AreEqual(1, list[1].Packages);
			Assert.AreEqual(2000, list[1].Value);
		}
	}
}
