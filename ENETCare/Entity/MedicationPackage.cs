using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ENETCare.Business
{
	/// <summary>
	/// MedicationPackage entity
	/// </summary>
	[Serializable]
	public class MedicationPackage
	{
		public int ID { get; set; }
		[RegularExpression(@"^\d{8}$", ErrorMessage = "Invalid barcode format")]
		public string Barcode { get; set; }
		[Column("Type")]
		public int TypeId { get; set; }
		public DateTime ExpireDate { get; set; }
		public PackageStatus Status { get; set; }
		[Column("StockDC")]
		public int? StockDCId { get; set; }
		[Column("SourceDC")]
		public int? SourceDCId { get; set; }
		[Column("DestinationDC")]
		public int? DestinationDCId { get; set; }
		public string Operator { get; set; }
		public DateTime UpdateTime { get; set; }

		public virtual MedicationType Type { get; set; }
		public virtual DistributionCentre StockDC { get; set; }
		public virtual DistributionCentre SourceDC { get; set; }
		public virtual DistributionCentre DestinationDC { get; set; }
	}

	public enum PackageStatus : short
	{
		InStock = 0,
		InTransit = 1,
		Distributed = 2,
		Discarded = 3,
		Lost = 4
	};

	public enum ExpireStatus
	{
		Expired = 0,
		NotExpired = 1,
		AboutToExpired = 2
	};
}
