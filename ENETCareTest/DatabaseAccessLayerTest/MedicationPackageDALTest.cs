using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ENETCare.Business;
using System.Data.Common;

namespace ENETCareTest
{
	[TestClass]
    public class MedicationPackageDALTest : DatabaseAccessLayerTest
	{
        IMedicationPackageDAO dao;
        //private DatabaseEntities currentcontext; 

		[TestInitialize]
		public void Setup()
		{
            dao = new MedicationPackageEntityFrameworkDAO();

            //Effort process
            //DbConnection connection = Effort.DbConnectionFactory.CreateTransient();
            //using (var context = new DatabaseEntities(connection))
            //{
            //    context.MedicationPackage.Add(new MedicationPackage() { ID = 1, Barcode = "11111", TypeId = 1, ExpireDate = new DateTime(2015, 12, 31), Status = PackageStatus.InStock, StockDCId = 1, SourceDCId = null, DestinationDC = null, Operator = "Doctor", UpdateTime = TimeProvider.Current.Now });
            //}

            //currentcontext = new DatabaseEntities(connection);

            //SQL CE

		}

        #region PrepareTestData

        protected override void PrepareTestData()
        {
            DeleteExistingData();
            ReseedTable("MedicationPackage");
            ReseedTable("MedicationType");
            ReseedTable("DistributionCentre");
            InsertTestDistributionCentres();
            InsertTestMedicationTypes();
            InsertTestMedicationPackages();
        }

        void InsertTestMedicationTypes()
        {
            InsertMedicationType("TEST MT1", 365, 2000, true);
            InsertMedicationType("TEST MT2", 730, 1000, false);
        }
        void InsertTestMedicationPackages()
        {
            InsertMedicationPackage("1111111", 1, new DateTime(2015, 12, 31), (int)PackageStatus.InStock, 1, TimeProvider.Current.Now, "doctor");
            //InsertMedicationPackage("2222222");
            //InsertMedicationPackage("3333333");
        }

        void InsertTestDistributionCentres()
        {
            InsertDistributionCentre("TEST DC1", "TEST DC1 Address", "");
            InsertDistributionCentre("TEST DC2", "TEST DC2 Address", "");
            InsertDistributionCentre("TEST DC3", "TEST DC3 Address", "");
            
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

        void InsertMedicationPackage(string barcode, int type, DateTime expiredate, int status, int stockdc, DateTime updatetime, string Operator)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = connectionString;
                conn.Open();
                string query = @"insert into MedicationPackage (Barcode, Type, ExpireDate, Status, StockDC, UpdateTime, Operator)
								 values (@barcode, @type, @expiredate, @status, @stockdc, @updatetime, @operator)";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.Add(new SqlParameter("barcode", barcode));
                command.Parameters.Add(new SqlParameter("type", type));
                command.Parameters.Add(new SqlParameter("expiredate", expiredate));
                command.Parameters.Add(new SqlParameter("status", status));
                command.Parameters.Add(new SqlParameter("stockdc", stockdc));
                command.Parameters.Add(new SqlParameter("updatetime", updatetime));
                command.Parameters.Add(new SqlParameter("operator", Operator));
                command.ExecuteNonQuery();
            }
        }

        #endregion

		[TestMethod()]
		public void MedicationPackageDAO_FindAllPackages_ReturnsAllPackages()
		{
            List<MedicationPackage> medicationpackages = dao.FindAllPackages();
            Assert.AreEqual(1, medicationpackages.Count);
            //var Package = new MedicationPackageEntityFrameworkDAO(currentcontext);
            //List<MedicationPackage> medicationpackages = Package.FindAllPackages();
            //Assert.AreEqual(0, medicationpackages.Count);
		}

		[TestMethod()]
		public void MedicationPackageDAO_FindInStockPackagesInDistributionCentre_ReturnsCorrectList()
		{
            List<MedicationPackage> medicationpackages = dao.FindInStockPackagesInDistributionCentre(1);
            Assert.AreEqual(1, medicationpackages.Count);
            Assert.AreEqual("doctor", medicationpackages[0].Operator);
		}

		[TestMethod()]
		public void MedicationPackageDAO_FindPackageByBarcode_ReturnsCorrespondingPackage()
		{
             MedicationPackage medicationpackage = dao.FindPackageByBarcode("1111111");
             Assert.AreEqual("doctor", medicationpackage.Operator);
		}

		[TestMethod()]
		public void MedicationPackageDAO_InsertPackage_Succeeds()
		{
            string barcode = "22222222";
            // Insert package
            MedicationPackage package = new MedicationPackage();
            package.Barcode = barcode;
            package.TypeId = (int)2;
            package.Status = PackageStatus.InStock;
            package.ExpireDate = new DateTime(2015, 12, 31);
            package.StockDCId = 1;
            package.Operator = "blizzard";
            package.UpdateTime = TimeProvider.Current.Now;
            dao.InsertPackage(package);

            // Find package after insert
            MedicationPackage actualpc = dao.FindPackageByBarcode("22222222");
            Assert.IsNotNull(actualpc);
            Assert.AreEqual("blizzard", package.Operator);
		}

		[TestMethod()]
		public void MedicationPackageDAO_UpdatePackage_Succeeds()
		{
			// Update package
            MedicationPackage package = new MedicationPackage();
			package.Status = PackageStatus.Distributed;
			package.Operator = "popcap";
			dao.UpdatePackage(package);

			// Find package after update
            MedicationPackage after_update = dao.FindPackageByBarcode("1111111");
            Assert.IsNotNull(after_update);
			Assert.AreEqual(PackageStatus.Distributed, package.Status);
			Assert.AreEqual("popcap", package.Operator);
		}

		[TestMethod()]
		public void MedicationPackageDAO_DeletePackage_Succeeds()
		{
            // Delete package
            MedicationPackage package = dao.FindPackageByBarcode("1111111");
            Assert.IsNotNull(package);
            dao.DeletePackage(package);

            // Find package after delete
            MedicationPackage after_insert_package = dao.FindPackageByBarcode("1111111");
            Assert.IsNull(after_insert_package);
		}

		/*
		protected override void PrepareTestData()
		{
			barcode = "123456";
			dc = FetchTestDistributionCentre();
			type = FetchTestMedicationType();
			DeletePackageIfExists();
		}

		DistributionCentre FetchTestDistributionCentre()
		{
			DistributionCentre dc = new DistributionCentre();
			using (SqlConnection conn = new SqlConnection())
			{
				conn.ConnectionString = connectionString;
				conn.Open();
				string query = "select top 1 ID, Name, Address, Phone from DistributionCentre";
				SqlCommand command = new SqlCommand(query, conn);
				using (SqlDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						dc.ID = reader.GetInt32(0);
						dc.Name = reader.GetString(1);
						dc.Address = reader.GetString(2);
						dc.Phone = reader.GetString(3);
					}
				}
			}
			return dc;
		}

		MedicationType FetchTestMedicationType()
		{
			MedicationType type = new MedicationType();
			using (SqlConnection conn = new SqlConnection())
			{
				conn.ConnectionString = connectionString;
				conn.Open();
				string query = "select top 1 ID, Name, ISNULL(Description, ''), ShelfLife, Value, IsSensitive from MedicationType";
				SqlCommand command = new SqlCommand(query, conn);
				using (SqlDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						type.ID = reader.GetInt32(0);
						type.Name = reader.GetString(1);
						type.Description = reader.GetString(2);
						type.ShelfLife = reader.GetInt16(3);
						type.Value = reader.GetDecimal(4);
						type.IsSensitive = reader.GetBoolean(5);
					}
				}
			}
			return type;
		}

		void DeletePackageIfExists()
		{
			using (SqlConnection conn = new SqlConnection())
			{
				conn.ConnectionString = connectionString;
				conn.Open();
				string query = "delete from MedicationPackage where Barcode = @barcode";
				SqlCommand command = new SqlCommand(query, conn);
				command.Parameters.Add(new SqlParameter("barcode", barcode));
				command.ExecuteNonQuery();
			}
		}

		[TestMethod()]
		public void Connection_OpenClose_Succeeds()
		{
			string connectionString = ConfigurationManager.ConnectionStrings["LocalDb"].ConnectionString;
			SqlConnection conn = new SqlConnection(connectionString);
			conn.Open();
			conn.Close();
		}

		[TestMethod()]
		public void MedicationPackageDAO_CRUD_ShouldSucceed()
		{
			// Find package by non-existing barcode
			MedicationPackage package = dao.FindPackageByBarcode(barcode);
			Assert.IsNull(package);

			// Insert package
			package = new MedicationPackage();
			package.Barcode = barcode;
			package.TypeId = type.ID;
			package.Status = PackageStatus.InStock;
			package.ExpireDate = new DateTime(2015, 12, 31);
			package.StockDCId = dc.ID;
			package.SourceDCId = null;
			package.DestinationDCId = null;
			package.Operator = "blizzard";
			package.UpdateTime = TimeProvider.Current.Now;
			dao.InsertPackage(package);

			// Find package after insert
			package = dao.FindPackageByBarcode(barcode);
			Assert.IsNotNull(package);
			Assert.AreEqual(PackageStatus.InStock, package.Status);
			Assert.AreEqual("blizzard", package.Operator);

			// Update package
			package.Status = PackageStatus.Distributed;
			package.Operator = "popcap";
			dao.UpdatePackage(package);

			// Find package after update
			package = dao.FindPackageByBarcode(barcode);
			Assert.IsNotNull(package);
			Assert.AreEqual(PackageStatus.Distributed, package.Status);
			Assert.AreEqual("popcap", package.Operator);

			// Delete package
			dao.DeletePackage(package);

			// Find package after delete
			package = dao.FindPackageByBarcode(barcode);
			Assert.IsNull(package);
		}
		*/
	}
}
