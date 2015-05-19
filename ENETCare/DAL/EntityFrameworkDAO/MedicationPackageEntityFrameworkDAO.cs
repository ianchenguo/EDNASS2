using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ENETCare.Business
{
	/// <summary>
	/// MedicationPackage EntityFramework implementation
	/// </summary>
	public class MedicationPackageEntityFrameworkDAO : EntityFrameworkDAO, IMedicationPackageDAO
	{
		/// <summary>
		/// Retrieves all medication packages in the database.
		/// </summary>
		/// <returns>a list of all the medication packages</returns>
		public List<MedicationPackage> FindAllPackages()
		{
			return context.MedicationPackage.ToList();
		}
		
		/// <summary>
		/// Retrieves medication packages of given medication type at a given distribution centre.
		/// </summary>
		/// <param name="distributionCentreId">distribution centre id</param>
		/// <param name="medicationTypeId">medication type id</param>
		/// <returns>a list of the medication packages</returns>
		public List<MedicationPackage> FindInStockPackagesInDistributionCentre(int distributionCentreId, int? medicationTypeId = null)
		{
			var packages = context.MedicationPackage.Where(x => x.Status == PackageStatus.InStock && x.StockDCId == distributionCentreId);
			if (medicationTypeId != null)
			{
				packages = packages.Where(x => x.TypeId == medicationTypeId);
			}
			return packages.Include(x => x.Type).ToList();
		}
		
		/// <summary>
		/// Retrieves a medication package by looking up its barcode.
		/// </summary>
		/// <param name="barcode">medication package barcode</param>
		/// <returns>a medication package corresponding to the barcode, or null if no matching medication package was found</returns>
		public MedicationPackage FindPackageByBarcode(string barcode)
		{
			return context.MedicationPackage.SingleOrDefault(x => x.Barcode == barcode);
		}

		/// <summary>
		/// Inserts a medication package record into the database.
		/// </summary>
		/// <param name="package">medication package</param>
		public void InsertPackage(MedicationPackage package)
		{
			context.MedicationPackage.Add(package);
			context.SaveChanges();
		}

		/// <summary>
		/// Updates a medication package record in the database.
		/// </summary>
		/// <param name="package">medication package</param>
		public void UpdatePackage(MedicationPackage package)
		{
			var currentPackage = context.MedicationPackage.SingleOrDefault(x => x.ID == package.ID);
			if (currentPackage != null)
			{
				context.Entry(currentPackage).CurrentValues.SetValues(package);
				context.SaveChanges();
			}
		}

		/// <summary>
		/// Deletes a medication package record from the database.
		/// </summary>
		/// <param name="package">medication package</param>
		public void DeletePackage(MedicationPackage package)
		{
			context.MedicationPackage.Attach(package);
			context.MedicationPackage.Remove(package);
			context.SaveChanges();
		}
	}
}
