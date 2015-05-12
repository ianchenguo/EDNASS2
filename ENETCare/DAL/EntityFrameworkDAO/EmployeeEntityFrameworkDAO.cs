using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ENETCare.Business
{
	/// <summary>
	/// Employee EntityFramework implementation
	/// </summary>
	public class EmployeeEntityFrameworkDAO : EmployeeDAO
	{
		/// <summary>
		/// Retrieves all employees in the database.
		/// </summary>
		/// <returns>a list of all the employees</returns>
		public List<Employee> FindAllEmployees()
		{
			using (DatabaseEntities context = new DatabaseEntities())
			{
				return context.Employee.ToList();
			}
		}

		/// <summary>
		/// Retrieves employees of given role.
		/// </summary>
		/// <param name="role">employee role</param>
		/// <returns>a list of the employees corresponding to the role</returns>
		public List<Employee> FindEmployeesByRole(Role role)
		{
			using (DatabaseEntities context = new DatabaseEntities())
			{
				return context.Employee
					.Where(e => e.EmployeeRole.Any(r => r.Name == role.ToString()))
					.Include(x => x.DistributionCentre)
					.ToList();
			}
		}

		/// <summary>
		/// Retrieves an employee by looking up its username.
		/// </summary>
		/// <param name="username">employee username</param>
		/// <returns>an employee corresponding to the username, or null if no matching employee was found</returns>
		public Employee GetEmployeeByUserName(string username)
		{
			using (DatabaseEntities context = new DatabaseEntities())
			{
				return context.Employee.SingleOrDefault(x => x.Username == username);
			}
		}
	}
}
