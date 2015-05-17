using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ENETCare.Business
{
	/// <summary>
	/// Employee entity
	/// </summary>
	public class Employee
	{
		public Employee()
		{
			this.EmployeeRole = new HashSet<EmployeeRole>();
		}

		public string ID { get; set; }
		public string Username { get; set; }
		public string Fullname { get; set; }
		public string Email { get; set; }
		[Column("DistributionCentre_Id")]
		public int DistributionCentreId { get; set; }

		public virtual ICollection<EmployeeRole> EmployeeRole { get; set; }
		public virtual DistributionCentre DistributionCentre { get; set; }
	}
	
	public enum Role
	{
		Agent = 1,
		Doctor = 2,
		Manager = 3,
		Undefined = 4
	};
}
