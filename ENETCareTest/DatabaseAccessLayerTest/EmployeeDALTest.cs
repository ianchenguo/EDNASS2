using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ENETCare.Business;

namespace ENETCareTest
{
	[TestClass]
	public class EmployeeDALTest : DatabaseAccessLayerTest
	{
		IEmployeeDAO dao;

		[TestInitialize]
		public void Setup()
		{
			dao = new EmployeeEntityFrameworkDAO();
		}

		#region PrepareTestData

		protected override void PrepareTestData()
		{
			// Data in AspNetUsers, AspNetRoles and AspNetUserRoles tables are populated by Identity Framework.
			// Therefore we use existing data in EmployeeDAL unit test.
		}

		#endregion

		[TestMethod()]
		public void EmployeeDAO_FindAllEmployees_ReturnsAllEmployees()
		{
			List<Employee> employees = dao.FindAllEmployees();
			Assert.AreEqual(7, employees.Count);
		}

		[TestMethod()]
		public void EmployeeDAO_FindAllDoctors_ReturnsAllDoctors()
		{
			List<Employee> doctors = dao.FindEmployeesByRole(Role.Doctor);
			Assert.AreEqual(4, doctors.Count);
			Assert.AreEqual("doctor1@enetcare.com", doctors[0].Username);
			Assert.AreEqual("doctor2@enetcare.com", doctors[1].Username);
			Assert.AreEqual("doctor3@enetcare.com", doctors[2].Username);
			Assert.AreEqual("doctor4@enetcare.com", doctors[3].Username);
		}

		[TestMethod()]
		public void EmployeeTypeDAO_GetEmployeeByUserName_ReturnsCorrespondingEmployee()
		{
			Employee employee;

			employee = dao.GetEmployeeByUserName("agent1@enetcare.com");
			Assert.IsNotNull(employee);
			Assert.AreEqual("agent1@enetcare.com", employee.Username);
			Assert.AreEqual("Agent1", employee.Fullname);
			Assert.AreEqual(1, employee.DistributionCentreId);

			employee = dao.GetEmployeeByUserName("doctor1@enetcare.com");
			Assert.IsNotNull(employee);
			Assert.AreEqual("doctor1@enetcare.com", employee.Username);
			Assert.AreEqual("Doctor1", employee.Fullname);
			Assert.AreEqual(2, employee.DistributionCentreId);

			employee = dao.GetEmployeeByUserName("doctor5@enetcare.com");
			Assert.IsNull(employee);
		}
	}
}
