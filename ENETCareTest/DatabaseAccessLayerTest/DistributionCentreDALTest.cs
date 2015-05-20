using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ENETCare.Business;

namespace ENETCareTest
{
	[TestClass]
	public class DistributionCentreDALTest : DatabaseAccessLayerTest
	{
		IDistributionCentreDAO dao;

		[TestInitialize]
		public void Setup()
		{
			dao = new DistributionCentreEntityFrameworkDAO();
		}

		#region PrepareTestData

		protected override void PrepareTestData()
		{
			DeleteExistingData();
			ReseedTable("DistributionCentre");
			InsertTestDistributionCentres();
		}

		void InsertTestDistributionCentres()
		{
			InsertDistributionCentre("TEST DC1", "TEST DC1 Address", "");
			InsertDistributionCentre("TEST DC2", "TEST DC2 Address", "");
			InsertDistributionCentre("TEST DC3", "TEST DC3 Address", "");
		}

		void InsertDistributionCentre(string name, string address, string phone)
		{
			using (SqlConnection conn = new SqlConnection())
			{
				conn.ConnectionString = connectionString;
				conn.Open();
				string query = "insert into DistributionCentre (Name, Address, Phone) values (@name, @address, @phone)";
				SqlCommand command = new SqlCommand(query, conn);
				command.Parameters.Add(new SqlParameter("name", name));
				command.Parameters.Add(new SqlParameter("address", address));
				command.Parameters.Add(new SqlParameter("phone", phone));
				command.ExecuteNonQuery();
			}
		}

		#endregion

		[TestMethod()]
		public void DistributionCentreDAO_FindAllDistributionCentres_ReturnsAllDistributionCentres()
		{
			List<DistributionCentre> distributionCentres = dao.FindAllDistributionCentres();
			Assert.AreEqual(3, distributionCentres.Count);
		}

		[TestMethod()]
		public void DistributionCentreDAO_GetDistributionCentreById_ReturnsCorrespondingDistributionCentre()
		{
			DistributionCentre dc;
			
			dc = dao.GetDistributionCentreById(1);
			Assert.IsNotNull(dc);
			Assert.AreEqual(1, dc.ID);
			Assert.AreEqual("TEST DC1", dc.Name);

			dc = dao.GetDistributionCentreById(3);
			Assert.IsNotNull(dc);
			Assert.AreEqual(3, dc.ID);
			Assert.AreEqual("TEST DC3", dc.Name);

			dc = dao.GetDistributionCentreById(5);
			Assert.IsNull(dc);
		}
	}
}
