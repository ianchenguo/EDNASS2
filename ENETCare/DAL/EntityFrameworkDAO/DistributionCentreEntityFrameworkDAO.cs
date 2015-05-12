using System;
using System.Collections.Generic;
using System.Linq;

namespace ENETCare.Business
{
	/// <summary>
	/// DistributionCentre EntityFramework implementation
	/// </summary>
	public class DistributionCentreEntityFrameworkDAO : DistributionCentreDAO
	{
		/// <summary>
		/// Retrieves all distribution centres in the database.
		/// </summary>
		/// <returns>a list of all the distribution centres</returns>
		public List<DistributionCentre> FindAllDistributionCentres()
		{
			using (DatabaseEntities context = new DatabaseEntities())
			{
				return context.DistributionCentre.ToList();
			}
		}

		/// <summary>
		/// Retrieves a distribution centre by looking up its id.
		/// </summary>
		/// <param name="id">distribution centre id</param>
		/// <returns>a distribution centre corresponding to the id, or null if no matching distribution centre was found</returns>
		public DistributionCentre GetDistributionCentreById(int id)
		{
			using (DatabaseEntities context = new DatabaseEntities())
			{
				return context.DistributionCentre.SingleOrDefault(x => x.ID == id);
			}
		}
	}
}
