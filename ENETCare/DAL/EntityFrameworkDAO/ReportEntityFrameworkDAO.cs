using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace ENETCare.Business
{
	/// <summary>
	/// Report EntityFramework implementation
	/// </summary>
	public class ReportEntityFrameworkDAO : EntityFrameworkDAO, IReportDAO
	{
		/// <summary>
		/// Retrieves the quantity and total value for each product type of given statuses at a given distribution centre.
		/// </summary>
		/// <param name="distributionCentreId">distribution centre id</param>
		/// <param name="statuses">package statuses</param>
		/// <returns>a list of MedicationTypeViewData</returns>
		public List<MedicationTypeViewData> FindDistributionCentreStockByStatus(int distributionCentreId, params PackageStatus[] statuses)
		{
			var query = from p in context.MedicationPackage
						join t in context.MedicationType on p.TypeId equals t.ID
						where p.StockDCId == distributionCentreId && statuses.Contains(p.Status)
						group t by t.Name into g
						select new MedicationTypeViewData
						{
							Type = g.Key,
							Quantity = g.Count(),
							Value = g.Sum(i => i.Value)
						};

			return query.OrderBy(x => x.Type).ToList();
		}

		/// <summary>
		/// Retrieves the quantity and total value for each product type in stock across all distribution centres.
		/// </summary>
		/// <returns>a list of MedicationTypeViewData</returns>
		public List<MedicationTypeViewData> FindGlobalStock()
		{
			var query = from p in context.MedicationPackage
						join t in context.MedicationType on p.TypeId equals t.ID
						where p.Status == PackageStatus.InStock
						group t by t.Name into g
						select new MedicationTypeViewData
						{
							Type = g.Key,
							Quantity = g.Count(),
							Value = g.Sum(i => i.Value)
						};

			return query.OrderBy(x => x.Type).ToList();
		}

		/// <summary>
		/// Retrieves the quantity and total value for each product type distributed by a given doctor.
		/// </summary>
		/// <param name="username">doctor username</param>
		/// <returns>a list of MedicationTypeViewData</returns>
		public List<MedicationTypeViewData> FindDoctorActivityByUserName(string username)
		{
			var query = from p in context.MedicationPackage
						join t in context.MedicationType on p.TypeId equals t.ID
						where p.Status == PackageStatus.Distributed && p.Operator == username
						group t by t.Name into g
						select new MedicationTypeViewData
						{
							Type = g.Key,
							Quantity = g.Count(),
							Value = g.Sum(i => i.Value)
						};

			return query.OrderBy(x => x.Type).ToList();
		}

		/// <summary>
		/// Retrieves total value and number of packages in transit between distribution centres.
		/// </summary>
		/// <returns>a list of ValueInTransitViewData</returns>
		public List<ValueInTransitViewData> FindAllValueInTransit()
		{
			var subQuery = from p in context.MedicationPackage
							join t in context.MedicationType on p.TypeId equals t.ID
							where p.Status == PackageStatus.InTransit
							group t by new { p.SourceDCId, p.DestinationDCId } into g
							select new
							{
								SourceDCId = g.Key.SourceDCId,
								DestinationDCId = g.Key.DestinationDCId,
								Packages = g.Count(),
								Value = g.Sum(i => i.Value)
							};

			var query = from sq in subQuery
						join d1 in context.DistributionCentre on sq.SourceDCId equals d1.ID
						join d2 in context.DistributionCentre on sq.DestinationDCId equals d2.ID
						select new ValueInTransitViewData
						{
							FromDistributionCentre = d1.Name,
							ToDistributionCentre = d2.Name,
							Packages = sq.Packages,
							Value = sq.Value
						};

			return query.OrderBy(x => x.FromDistributionCentre).ThenBy(x => x.ToDistributionCentre).ToList();
		}
	}
}
