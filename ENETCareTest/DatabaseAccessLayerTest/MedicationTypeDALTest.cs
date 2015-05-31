using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ENETCare.Business;

namespace ENETCareTest
{
	[TestClass]
	public class MedicationTypeDALTest : DatabaseAccessLayerTest
	{
		IMedicationTypeDAO dao;

		[TestInitialize]
		public void Setup()
		{
			dao = new MedicationTypeEntityFrameworkDAO();
		}

		#region PrepareTestData

		protected override void PrepareTestData()
		{
			DeleteExistingData();
			CreateTestMedicationTypes();
		}

		#endregion

		[TestMethod()]
		public void MedicationTypeDAO_FindAllMedicationTypes_ReturnsAllMedicationTypes()
		{
			List<MedicationType> medicationTypes = dao.FindAllMedicationTypes();
			Assert.AreEqual(3, medicationTypes.Count);
		}

		[TestMethod()]
		public void MedicationTypeDAO_GetMedicationTypeById_ReturnsCorrespondingMedicationType()
		{
			MedicationType type;
			
			type = dao.GetMedicationTypeById(1);
			Assert.IsNotNull(type);
			Assert.AreEqual(1, type.ID);
			Assert.AreEqual("TEST MT1", type.Name);
			Assert.AreEqual(2000, type.Value);

			type = dao.GetMedicationTypeById(3);
			Assert.IsNotNull(type);
			Assert.AreEqual(3, type.ID);
			Assert.AreEqual("TEST MT3", type.Name);
			Assert.AreEqual(300, type.Value);

			type = dao.GetMedicationTypeById(5);
			Assert.IsNull(type);
		}
	}
}
