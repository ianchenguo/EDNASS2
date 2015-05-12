using System;

namespace ENETCare.Business
{
	/// <summary>
	/// Choose DAO implementations
	/// </summary>
	public class DAOFactory
	{
		public static DistributionCentreDAO GetDistributionCentreDAO()
		{
			return new DistributionCentreEntityFrameworkDAO();
		}

		public static EmployeeDAO GetEmployeeDAO()
		{
			return new EmployeeEntityFrameworkDAO();
		}

		public static MedicationTypeDAO GetMedicationTypeDAO()
		{
			return new MedicationTypeEntityFrameworkDAO();
		}

		public static MedicationPackageDAO GetMedicationPackageDAO()
		{
			return new MedicationPackageEntityFrameworkDAO();
		}

		public static ReportDAO GetReportDAO()
		{
			return new ReportDataReaderDAO();
		}
	}
}
