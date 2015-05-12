using System;
using System.Collections.Generic;
using System.Linq;

namespace ENETCare.Business
{
	/// <summary>
	/// MedicationType EntityFramework implementation
	/// </summary>
	public class MedicationTypeEntityFrameworkDAO : MedicationTypeDAO
	{
		/// <summary>
		/// Retrieves all medication types in the database.
		/// </summary>
		/// <returns>a list of all the medication types</returns>
		public List<MedicationType> FindAllMedicationTypes()
		{
			using (DatabaseEntities context = new DatabaseEntities())
			{
				return context.MedicationType.ToList();
			}
		}

		/// <summary>
		/// Retrieves a medication type by looking up its id.
		/// </summary>
		/// <param name="id">medication type id</param>
		/// <returns>a medication type corresponding to the id, or null if no matching medication type was found</returns>
		public MedicationType GetMedicationTypeById(int id)
		{
			using (DatabaseEntities context = new DatabaseEntities())
			{
				return context.MedicationType.SingleOrDefault(x => x.ID == id);
			}
		}
	}
}
