using System;

namespace ENETCare.Business
{
	/// <summary>
	/// Choose DAO implementations
	/// </summary>
	public class DAOFactory
	{
		public static IDistributionCentreDAO GetDistributionCentreDAO()
		{
			return new DistributionCentreEntityFrameworkDAO();
		}

		public static IEmployeeDAO GetEmployeeDAO()
		{
			return new EmployeeEntityFrameworkDAO();
		}

		public static IMedicationTypeDAO GetMedicationTypeDAO()
		{
			return new MedicationTypeEntityFrameworkDAO();
		}

		public static IMedicationPackageDAO GetMedicationPackageDAO()
		{
			return new MedicationPackageEntityFrameworkDAO();
		}

		public static IReportDAO GetReportDAO()
		{
			return new ReportEntityFrameworkDAO();
		}
	}
}
