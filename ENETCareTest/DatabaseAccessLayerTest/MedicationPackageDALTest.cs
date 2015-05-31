using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ENETCare.Business;

namespace ENETCareTest
{
	[TestClass]
	public class MedicationPackageDALTest : DatabaseAccessLayerTest
	{
		IMedicationPackageDAO dao;

		[TestInitialize]
		public void Setup()
		{
			dao = new MedicationPackageEntityFrameworkDAO();
		}

		#region PrepareTestData

		protected override void PrepareTestData()
		{
			DeleteExistingData();
			CreateTestDistributionCentres();
			CreateTestMedicationTypes();
			CreateTestMedicationPackages();
		}

		#endregion

		[TestMethod()]
		public void MedicationPackageDAO_FindAllPackages_ReturnsAllPackages()
		{
			List<MedicationPackage> medicationPackages = dao.FindAllPackages();
			Assert.AreEqual(3, medicationPackages.Count);
		}

		[TestMethod()]
		public void MedicationPackageDAO_FindInStockPackagesInDistributionCentre_ReturnsCorrectList()
		{
			List<MedicationPackage> medicationPackages = dao.FindInStockPackagesInDistributionCentre(1);
			Assert.AreEqual(1, medicationPackages.Count);

			medicationPackages = dao.FindInStockPackagesInDistributionCentre(2);
			Assert.AreEqual(2, medicationPackages.Count);

			medicationPackages = dao.FindInStockPackagesInDistributionCentre(3);
			Assert.AreEqual(0, medicationPackages.Count);
		}

		[TestMethod()]
		public void MedicationPackageDAO_FindPackageByBarcode_ReturnsCorrespondingPackage()
		{
			MedicationPackage medicationPackage = dao.FindPackageByBarcode("10001000");
			Assert.IsNotNull(medicationPackage);
			Assert.AreEqual(1, medicationPackage.TypeId);
			Assert.AreEqual(PackageStatus.InStock, medicationPackage.Status);
			Assert.AreEqual(1, medicationPackage.StockDCId);
			Assert.AreEqual("blizzard", medicationPackage.Operator);

			medicationPackage = dao.FindPackageByBarcode("10001001");
			Assert.IsNull(medicationPackage);
		}

		[TestMethod()]
		public void MedicationPackageDAO_InsertPackage_Succeeds()
		{
			string barcode = "10001001";

			// Preconditions
			MedicationPackage packageBeforeInsert = dao.FindPackageByBarcode(barcode);
			Assert.IsNull(packageBeforeInsert);

			// Insert package
			MedicationPackage package = new MedicationPackage();
			package.Barcode = barcode;
			package.TypeId = 1;
			package.Status = PackageStatus.InStock;
			package.ExpireDate = new DateTime(2015, 12, 31);
			package.StockDCId = 1;
			package.Operator = "blizzard";
			package.UpdateTime = TimeProvider.Current.Now;
			dao.InsertPackage(package);

			// Assert
			MedicationPackage packageAfterInsert = dao.FindPackageByBarcode(barcode);
			Assert.IsNotNull(packageAfterInsert);
		}

		[TestMethod()]
		public void MedicationPackageDAO_UpdatePackage_Succeeds()
		{
			string barcode = "10001000";

			// Preconditions
			MedicationPackage package = dao.FindPackageByBarcode(barcode);
			Assert.IsNotNull(package);
			Assert.AreEqual(PackageStatus.InStock, package.Status);
			Assert.AreEqual("blizzard", package.Operator);

			// Update package
			package.Status = PackageStatus.Distributed;
			package.Operator = "popcap";
			dao.UpdatePackage(package);

			// Assert
			MedicationPackage packageAfterUpdate = dao.FindPackageByBarcode(barcode);
			Assert.IsNotNull(packageAfterUpdate);
			Assert.AreEqual(PackageStatus.Distributed, packageAfterUpdate.Status);
			Assert.AreEqual("popcap", packageAfterUpdate.Operator);
		}

		[TestMethod()]
		public void MedicationPackageDAO_DeletePackage_Succeeds()
		{
			string barcode = "10001000";

			// Preconditions
			MedicationPackage package = dao.FindPackageByBarcode(barcode);
			Assert.IsNotNull(package);

			// Delete package
			dao.DeletePackage(package);

			// Assert
			MedicationPackage after_insert_package = dao.FindPackageByBarcode(barcode);
			Assert.IsNull(after_insert_package);
		}
	}
}
