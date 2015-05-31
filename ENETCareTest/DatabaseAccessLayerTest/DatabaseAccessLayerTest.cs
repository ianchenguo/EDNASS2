using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ENETCare.Business;

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
			// should be overridden in a derived class
		}

		protected virtual void CreateTestDistributionCentres()
		{
			// default test distribution centres
			InsertDistributionCentre("TEST DC1", "TEST DC1 Address", "");
			InsertDistributionCentre("TEST DC2", "TEST DC2 Address", "");
			InsertDistributionCentre("TEST DC3", "TEST DC3 Address", "");
		}

		protected virtual void CreateTestMedicationTypes()
		{
			// default test medication types
			InsertMedicationType("TEST MT1", 365, 2000, true);
			InsertMedicationType("TEST MT2", 730, 1000, false);
			InsertMedicationType("TEST MT3", 1000, 300, false);
		}

		protected virtual void CreateTestMedicationPackages()
		{
			// default test medication packages
			InsertMedicationPackage("10001000", 1, new DateTime(2015, 12, 31), PackageStatus.InStock, 1, null, null, "blizzard", TimeProvider.Current.Now);
			InsertMedicationPackage("20001000", 2, new DateTime(2015, 12, 31), PackageStatus.InStock, 2, null, null, "popcap", TimeProvider.Current.Now);
			InsertMedicationPackage("30001000", 3, new DateTime(2015, 12, 31), PackageStatus.InStock, 2, null, null, "popcap", TimeProvider.Current.Now);
		}

		#region Implementation

		protected void DeleteExistingData()
		{
			DeleteTable("MedicationPackage");
			DeleteTable("MedicationType");
			DeleteTable("AspNetUsers");
			DeleteTable("DistributionCentre");

			ReseedTable("MedicationPackage");
			ReseedTable("MedicationType");
			ReseedTable("DistributionCentre");
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
				string cmdText = string.Format("dbcc checkident ('{0}', reseed, 0)", tableName);
				SqlCommand command = new SqlCommand(cmdText, conn);
				command.ExecuteNonQuery();
			}
		}

		protected void InsertDistributionCentre(string name, string address, string phone)
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

		protected void InsertMedicationType(string name, short shelfLife, decimal value, bool isSensitive)
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

		protected void InsertMedicationPackage(string barcode, int type, DateTime expiredate, PackageStatus status, int? stockdc, int? sourcedc, int? destinationdc, string username, DateTime updatetime)
		{
			using (SqlConnection conn = new SqlConnection())
			{
				conn.ConnectionString = connectionString;
				conn.Open();
				string query = @"insert into MedicationPackage (Barcode, Type, ExpireDate, Status, StockDC, SourceDC, DestinationDC, Operator, UpdateTime)
								 values (@barcode, @type, @expiredate, @status, @stockdc, @sourcedc, @destinationdc, @operator, @updatetime)";
				SqlCommand command = new SqlCommand(query, conn);
				command.Parameters.Add(new SqlParameter("barcode", barcode));
				command.Parameters.Add(new SqlParameter("type", type));
				command.Parameters.Add(new SqlParameter("expiredate", expiredate));
				command.Parameters.Add(new SqlParameter("status", status));
				command.Parameters.Add(new SqlParameter("stockdc", (object)stockdc ?? DBNull.Value));
				command.Parameters.Add(new SqlParameter("sourcedc", (object)sourcedc ?? DBNull.Value));
				command.Parameters.Add(new SqlParameter("destinationdc", (object)destinationdc ?? DBNull.Value));
				command.Parameters.Add(new SqlParameter("operator", username));
				command.Parameters.Add(new SqlParameter("updatetime", updatetime));
				command.ExecuteNonQuery();
			}
		}

		#endregion
	}
}
