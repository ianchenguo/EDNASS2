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
        IMedicationPackageDAO PackageDao;
        IMedicationTypeDAO TypeDao;

		[TestInitialize]
		public void Setup()
		{
			dao = new ReportEntityFrameworkDAO();
            PackageDao = new MedicationPackageEntityFrameworkDAO();
            TypeDao = new MedicationTypeEntityFrameworkDAO();

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
            InsertMedicationType("TEST MT3", 1000, 300, false);
        }
        void InsertTestMedicationPackages()
        {
            DateTime minDateTime = DateTime.MaxValue;
            DateTime maxDateTime = DateTime.MinValue;

            minDateTime = new DateTime(1753, 1, 1); //Minimum SQL Date
            maxDateTime = new DateTime(9999, 12, 31, 23, 59, 59, 997); //Maximum SQL Date

            InsertMedicationPackage("1111111", 1, minDateTime, PackageStatus.InStock, 1, 1, 2, maxDateTime, "doctor");
            InsertMedicationPackage("2222222", 2, minDateTime, PackageStatus.InTransit, 1, 1, 2, maxDateTime, "agent");
            InsertMedicationPackage("3333333", 3, minDateTime, PackageStatus.InTransit, 1, 2, 3, maxDateTime, "manager");
            InsertMedicationPackage("4444444", 1, minDateTime, PackageStatus.Distributed, 1, 1, 2, maxDateTime, "doctor");
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

        void InsertMedicationPackage(string barcode, int type, DateTime expiredate, PackageStatus status, int stockdc, int sourcedc, int  destinationdc, DateTime updatetime, string Operator)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = connectionString;
                conn.Open();
                string query = @"insert into MedicationPackage (Barcode, Type, ExpireDate, Status, StockDC, SourceDC, DestinationDC, UpdateTime, Operator)
								 values (@barcode, @type, @expiredate, @status, @stockdc, @sourcedc, @destinationdc, @updatetime, @operator)";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.Add(new SqlParameter("barcode", barcode));
                command.Parameters.Add(new SqlParameter("type", type));
                command.Parameters.Add(new SqlParameter("expiredate", expiredate));
                command.Parameters.Add(new SqlParameter("status", status));
                command.Parameters.Add(new SqlParameter("stockdc", stockdc));
                command.Parameters.Add(new SqlParameter("sourcedc", sourcedc));
                command.Parameters.Add(new SqlParameter("destinationdc", destinationdc));
                command.Parameters.Add(new SqlParameter("updatetime", updatetime));
                command.Parameters.Add(new SqlParameter("operator", Operator));
                command.ExecuteNonQuery();
            }
        }

        #endregion


		[TestMethod()]
		public void ReportDAO_FindDistributionCentreStockByStatus_ReturnsCorrectList()
		{
            List<MedicationTypeViewData> distributioncentre_stock_bystatus = dao.FindDistributionCentreStockByStatus(1, PackageStatus.InTransit);
            Assert.AreEqual(2, distributioncentre_stock_bystatus.Count);

            //test a instock one
            List<MedicationTypeViewData> distributioncentre_stock_bystatus1 = dao.FindDistributionCentreStockByStatus(1, PackageStatus.InStock);
            Assert.AreEqual(1, distributioncentre_stock_bystatus1.Count);
		}

		[TestMethod()]
		public void ReportDAO_FindGlobalStock_ReturnsCorrectList()
		{
            //MedicationType type = TypeDao.GetMedicationTypeById(1);
            //Assert.IsNotNull(type);
            //Assert.AreEqual(1, type.ID);
            //
            List<MedicationPackage> medicationpackages = PackageDao.FindAllPackages();
            Assert.AreEqual(4, medicationpackages.Count);

            //test if there is a row in the database
            List<MedicationTypeViewData> globalreturn = dao.FindGlobalStock();
            Assert.AreEqual(1, globalreturn.Count);

            
		}

		[TestMethod()]
		public void ReportDAO_FindDoctorActivityByUserName_ReturnsCorrectList()
		{   //check a non-exist operator name
            List<MedicationTypeViewData> doctorname_result = dao.FindDoctorActivityByUserName("12345678");
            Assert.IsNotNull(doctorname_result);
            Assert.AreEqual(0, doctorname_result.Count);

            MedicationPackage package = new MedicationPackage();
            package.Operator = "doctor";
            List<MedicationTypeViewData> username_result = dao.FindDoctorActivityByUserName("doctor");
            Assert.IsNotNull(username_result);
            Assert.AreEqual(1, username_result.Count);
		}

		[TestMethod()]
		public void ReportDAO_FindAllValueInTransit_ReturnsCorrectList()
		{
            // should be 2 results (stockdc2 - destinationdc3) and (stockdc1-destinationdc2) as test data
            List<ValueInTransitViewData> alltransitvalue = dao.FindAllValueInTransit();

            Assert.AreEqual(2, alltransitvalue.Count);

		}
	}
}
