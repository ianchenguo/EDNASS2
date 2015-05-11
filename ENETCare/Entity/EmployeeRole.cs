using System.Collections.Generic;

namespace ENETCare.Business
{
	/// <summary>
	/// EmployeeRole entity
	/// </summary>
	public class EmployeeRole
	{
		public EmployeeRole()
		{
			this.Employee = new HashSet<Employee>();
		}
		
		public string Id { get; set; }
		public string Name { get; set; }
		
		public virtual ICollection<Employee> Employee { get; set; }
	}
}
