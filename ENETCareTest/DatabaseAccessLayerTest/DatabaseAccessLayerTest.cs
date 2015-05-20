using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ENETCareTest
{
	[TestClass]
	public class DatabaseAccessLayerTest
	{
		TransactionScope _trans;
		protected string connectionString;

		[AssemblyInitialize]
		public static void SetupDataDirectory(TestContext context)
		{
			AppDomain.CurrentDomain.SetData("DataDirectory", Path.GetFullPath(@"..\..\..\LocalDB\"));
		}

		[TestInitialize]
		public void DALTestInit()
		{
			_trans = new TransactionScope();
			connectionString = ConfigurationManager.ConnectionStrings["LocalDb"].ConnectionString;
			PrepareTestData();
		}

		[TestCleanup()]
		public void DALTestCleanup()
		{
			_trans.Dispose();
		}

		protected virtual void PrepareTestData()
		{
		}

		protected void DeleteExistingData()
		{
			DeleteTable("MedicationPackage");
			DeleteTable("MedicationType");
			DeleteTable("AspNetUsers");
			DeleteTable("DistributionCentre");
		}

		protected void DeleteTable(string tableName)
		{
			using (SqlConnection conn = new SqlConnection())
			{
				conn.ConnectionString = connectionString;
				conn.Open();
				string cmdText = string.Format("delete from {0}", tableName);
				SqlCommand command = new SqlCommand(cmdText, conn);
				command.ExecuteNonQuery();
			}
		}

		protected void ReseedTable(string tableName)
		{
			using (SqlConnection conn = new SqlConnection())
			{
				conn.ConnectionString = connectionString;
				conn.Open();
				string cmdText = string.Format("DBCC CHECKIDENT ('{0}', RESEED, 0)", tableName);
				SqlCommand command = new SqlCommand(cmdText, conn);
				command.ExecuteNonQuery();
			}
		}
	}
}
