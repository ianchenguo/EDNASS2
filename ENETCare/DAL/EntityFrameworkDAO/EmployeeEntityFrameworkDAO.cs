using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ENETCare.Business
{
	/// <summary>
	/// Employee EntityFramework implementation
	/// </summary>
	public class EmployeeEntityFrameworkDAO : EntityFrameworkDAO, EmployeeDAO
	{
		/// <summary>
		/// Retrieves all employees in the database.
		/// </summary>
		/// <returns>a list of all the employees</returns>
		public List<Employee> FindAllEmployees()
		{
			return context.Employee.OrderBy(x => x.Username).ToList();
		}

		/// <summary>
		/// Retrieves employees of given role.
		/// </summary>
		/// <param name="role">employee role</param>
		/// <returns>a list of the employees corresponding to the role</returns>
		public List<Employee> FindEmployeesByRole(Role role)
		{
			return context.Employee
				.Where(e => e.EmployeeRole.Any(r => r.Name == role.ToString()))
				.Include(x => x.DistributionCentre)
				.OrderBy(x => x.Fullname)
				.ToList();
		}

		/// <summary>
		/// Retrieves an employee by looking up its username.
		/// </summary>
		/// <param name="username">employee username</param>
		/// <returns>an employee corresponding to the username, or null if no matching employee was found</returns>
		public Employee GetEmployeeByUserName(string username)
		{
			return context.Employee.SingleOrDefault(x => x.Username == username);
		}
	}
}
