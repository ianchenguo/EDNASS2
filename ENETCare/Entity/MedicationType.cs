using System;
using System.Collections.Generic;

namespace ENETCare.Business
{
	/// <summary>
	/// MedicationType entity
	/// </summary>
	[Serializable]
	public class MedicationType
	{
		public MedicationType()
		{
			this.MedicationPackage = new HashSet<MedicationPackage>();
		}

		public int ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public short ShelfLife { get; set; }
		public decimal Value { get; set; }
		public bool IsSensitive { get; set; }

		public virtual ICollection<MedicationPackage> MedicationPackage { get; set; }

		public DateTime DefaultExpireDate
		{
			get
			{
				return DateTime.Today.AddDays(ShelfLife);
			}
		}
	}
}
