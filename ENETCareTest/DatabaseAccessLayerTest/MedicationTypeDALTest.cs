using System.Collections.Generic;
using System.Data.SqlClient;
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
			ReseedTable("MedicationType");
			InsertTestMedicationTypes();
		}

		void InsertTestMedicationTypes()
		{
			InsertMedicationType("TEST MT1", 365, 2000, true);
			InsertMedicationType("TEST MT2", 730, 1000, false);
			InsertMedicationType("TEST MT3", 1000, 300, false);
		}

		void InsertMedicationType(string name, short shelfLife, decimal value, bool isSensitive)
		{
			using (SqlConnection conn = new SqlConnection())
			{
				conn.ConnectionString = connectionString;
				conn.Open();
				string query = "insert into MedicationType (Name, ShelfLife, Value, IsSensitive) values (@name, @shelfLife, @value, @isSensitive)";
				SqlCommand command = new SqlCommand(query, conn);
				command.Parameters.Add(new SqlParameter("name", name));
				command.Parameters.Add(new SqlParameter("shelfLife", shelfLife));
				command.Parameters.Add(new SqlParameter("value", value));
				command.Parameters.Add(new SqlParameter("isSensitive", isSensitive));
				command.ExecuteNonQuery();
			}
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
