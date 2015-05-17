using System;
using System.Collections.Generic;
using System.Globalization;

namespace ENETCare.Business
{
	/// <summary>
	/// MedicationPackage Business Logic Layer
	/// </summary>
	public class MedicationPackageBLL
	{
		#region Properties

		string username;
		Employee user;
		public Employee User
		{
			get
			{
				if (user == null)
				{
					user = GetEmployeeByUserName(username);
				}
				return user;
			}
		}
		public MedicationPackageDAO MedicationPackageDAO { get; set; }
		public MedicationTypeDAO MedicationTypeDAO { get; set; }
		public DistributionCentreDAO DistributionCentreDAO { get; set; }
		public EmployeeDAO EmployeeDAO { get; set; }

		#endregion

		#region Constructor

		public MedicationPackageBLL(string username)
		{
			this.username = username;
			MedicationPackageDAO = DAOFactory.GetMedicationPackageDAO();
			MedicationTypeDAO = DAOFactory.GetMedicationTypeDAO();
			DistributionCentreDAO = DAOFactory.GetDistributionCentreDAO();
			EmployeeDAO = DAOFactory.GetEmployeeDAO();
		}

		#endregion

		#region Scan

		/// <summary>
		/// Retrieves a medication package by looking up its barcode.
		/// </summary>
		/// <param name="barcode">medication package barcode</param>
		/// <returns>a medication package corresponding to the barcode, or null if no matching medication package was found</returns>
		public MedicationPackage ScanPackage(string barcode)
		{
			return MedicationPackageDAO.FindPackageByBarcode(barcode);
		}

		#endregion

		#region Register

		/// <summary>
		/// Registers a medication package.
		/// </summary>
		/// <param name="medicationTypeId">medication type id</param>
		/// <param name="expireDate">package expire date</param>
		/// <param name="barcode">package barcode</param>
		/// <returns>generated barcode number</returns>
		public string RegisterPackage(int medicationTypeId, string expireDate, string barcode = "")
		{
			DateTime parsedExpireDate;
			if (!DateTime.TryParse(expireDate, out parsedExpireDate))
			{
				throw new ENETCareException(Properties.Resources.InvalidDateFormat);
			}
			MedicationType medicationType = MedicationTypeDAO.GetMedicationTypeById(medicationTypeId);
			CheckMedicationTypeIsValid(medicationType);
			return RegisterPackage(medicationType, parsedExpireDate, barcode);
		}

		string RegisterPackage(MedicationType medicationType, DateTime expireDate, string barcode)
		{
			MedicationPackage package = new MedicationPackage();
			if (string.IsNullOrEmpty(barcode))
			{
				barcode = BarcodeHelper.GenerateBarcode();
			}
			package.Barcode = barcode;
			package.TypeId = medicationType.ID;
			package.ExpireDate = expireDate;
			ManipulatePackage(package, PackageStatus.InStock, User.DistributionCentreId, null, null);
			MedicationPackageDAO.InsertPackage(package);
			return barcode;
		}

		#endregion

		#region Send

		/// <summary>
		/// Sends a medication package.
		/// </summary>
		/// <param name="barcode">package barcode</param>
		/// <param name="distributionCentreId">distribution centre id</param>
		/// <param name="isTrusted">whether to trust the source of the package</param>
		public void SendPackage(string barcode, int distributionCentreId, bool isTrusted = true)
		{
			DistributionCentre distributionCentre = DistributionCentreDAO.GetDistributionCentreById(distributionCentreId);
			CheckDistributionCentreIsValid(distributionCentre);
			MedicationPackage package = ScanPackage(barcode);
			CheckMedicationPackageIsValid(package);
			if (User.DistributionCentreId == distributionCentreId)
			{
				throw new ENETCareException(Properties.Resources.AnotherDistributionCentre);
			}
			if (!isTrusted)
			{
				CheckMedicationPackageStatus(package, PackageStatus.InStock);
				CheckStockDistributionCentre(package);
			}
			ManipulatePackage(package, PackageStatus.InTransit, null, User.DistributionCentreId, distributionCentreId);
			MedicationPackageDAO.UpdatePackage(package);
		}

		#endregion

		#region Receive

		/// <summary>
		/// Receives a medication package.
		/// </summary>
		/// <param name="barcode">package barcode</param>
		/// <param name="isTrusted">whether to trust the source of the package</param>
		public void ReceivePackage(string barcode, bool isTrusted = true)
		{
			MedicationPackage package = ScanPackage(barcode);
			CheckMedicationPackageIsValid(package);
			if (!isTrusted)
			{
				CheckMedicationPackageStatus(package, PackageStatus.InTransit);
				CheckDestinationDistributionCentre(package);
			}
			ManipulatePackage(package, PackageStatus.InStock, User.DistributionCentreId, null, null);
			MedicationPackageDAO.UpdatePackage(package);
		}

		#endregion

		#region Distribute

		/// <summary>
		/// Distributes a medication package.
		/// </summary>
		/// <param name="barcode">package barcode</param>
		/// <param name="isTrusted">whether to trust the source of the package</param>
		public void DistributePackage(string barcode, bool isTrusted = true)
		{
			MedicationPackage package = ScanPackage(barcode);
			CheckMedicationPackageIsValid(package);
			if (!isTrusted)
			{
				CheckMedicationPackageStatus(package, PackageStatus.InStock);
				CheckStockDistributionCentre(package);
			}
			ManipulatePackage(package, PackageStatus.Distributed, User.DistributionCentreId, null, null);
			MedicationPackageDAO.UpdatePackage(package);
		}

		#endregion

		#region Discard

		/// <summary>
		/// Discards a medication package.
		/// </summary>
		/// <param name="barcode">package barcode</param>
		/// <param name="isTrusted">whether to trust the source of the package</param>
		public void DiscardPackage(string barcode, bool isTrusted = true)
		{
			MedicationPackage package = ScanPackage(barcode);
			CheckMedicationPackageIsValid(package);
			if (!isTrusted)
			{
				CheckMedicationPackageStatus(package, PackageStatus.InStock);
				CheckStockDistributionCentre(package);
			}
			ManipulatePackage(package, PackageStatus.Discarded, User.DistributionCentreId, null, null);
			MedicationPackageDAO.UpdatePackage(package);
		}

		#endregion

		#region Stocktake

		/// <summary>
		/// Retrieves the stocktaking report.
		/// </summary>
		/// <returns>a list of package barcode, medication type, expire date and expire status</returns>
		public List<StocktakingViewData> Stocktake()
		{
			List<MedicationPackage> packages = MedicationPackageDAO.FindInStockPackagesInDistributionCentre(User.DistributionCentreId);
			List<StocktakingViewData> list = new List<StocktakingViewData>();
			const int warningDays = 7;
			foreach (MedicationPackage package in packages)
			{
				ExpireStatus expireStatus = ExpireStatus.NotExpired;
				if (TimeProvider.Current.Now > package.ExpireDate)
				{
					expireStatus = ExpireStatus.Expired;
				}
				else if (TimeProvider.Current.Now.AddDays(warningDays) > package.ExpireDate)
				{
					expireStatus = ExpireStatus.AboutToExpired;
				}
				var row = new StocktakingViewData
				{
					Barcode = package.Barcode,
					Type = package.Type.Name,
					ExpireDate = package.ExpireDate.ToString("d", new CultureInfo("en-au")),
					ExpireStatus = expireStatus
				};
				list.Add(row);
			}
			return list;
		}

		#endregion

		#region Audit

		/// <summary>
		/// Check whether a package is unexpected and has its status/location updated
		/// </summary>
		/// <param name="medicationTypeId">medication type id</param>
		/// <param name="barcode">package barcode</param>
		/// <returns>true if status/location of the package has been updated, or false if not</returns>
		public bool CheckAndUpdatePackage(int medicationTypeId, string barcode)
		{
			bool updated = false;
			MedicationPackage package = MedicationPackageDAO.FindPackageByBarcode(barcode);
			if (package == null)
			{
				MedicationType medicationType = MedicationTypeDAO.GetMedicationTypeById(medicationTypeId);
				DateTime expireDate = medicationType.DefaultExpireDate;
				RegisterPackage(medicationType, expireDate, barcode);
				updated = true;
			}
			else if (package.TypeId != medicationTypeId)
			{
				throw new ENETCareException(Properties.Resources.MedicationTypeNotMatched);
			}
			else if (package.Status != PackageStatus.InStock || package.StockDCId != User.DistributionCentreId)
			{
				ManipulatePackage(package, PackageStatus.InStock, User.DistributionCentreId, null, null);
				MedicationPackageDAO.UpdatePackage(package);
				updated = true;
			}
			return updated;
		}

		/// <summary>
		/// Identifies lost packages for a given package type.
		/// </summary>
		/// <param name="medicationTypeId">medication type id</param>
		/// <param name="scannedList">scanned barcode list</param>
		/// <returns>a list of the medication packages</returns>
		public List<MedicationPackage> AuditPackages(int medicationTypeId, List<string> scannedList)
		{
			List<MedicationPackage> lostPackages = new List<MedicationPackage>();
			List<MedicationPackage> inStockPackages = GetInStockList(medicationTypeId);
			foreach (MedicationPackage package in inStockPackages)
			{
				if (!scannedList.Contains(package.Barcode))
				{
					ManipulatePackage(package, PackageStatus.Lost, User.DistributionCentreId, null, null);
					lostPackages.Add(package);
					MedicationPackageDAO.UpdatePackage(package);
				}
			}
			return lostPackages;
		}

		public List<MedicationPackage> GetInStockList(int medicationTypeId)
		{
			return MedicationPackageDAO.FindInStockPackagesInDistributionCentre(User.DistributionCentreId, medicationTypeId);
		}

		#endregion

		#region Implementation

		/// <summary>
		/// Retrieves an employee by the user name.
		/// </summary>
		/// <param name="username">employee user name</param>
		/// <returns>an employee corresponding to the username</returns>
		Employee GetEmployeeByUserName(string username)
		{
			Employee employee = EmployeeDAO.GetEmployeeByUserName(username);
			if (employee == null)
			{
				throw new ENETCareException(string.Format("{0}: {1}", Properties.Resources.InvalidUser, username));
			}
			return employee;
		}

		/// <summary>
		/// Manipulate package updating its status and related distribution centres.
		/// </summary>
		/// <param name="package">medication package</param>
		/// <param name="status">package status </param>
		/// <param name="stockDC">stock distribution centre</param>
		/// <param name="sourceDC">source distribution centre</param>
		/// <param name="destinationDC">destination distribution centre</param>
		void ManipulatePackage(MedicationPackage package, PackageStatus status, int? stockDC, int? sourceDC, int? destinationDC)
		{
			package.Status = status;
			package.StockDCId = stockDC;
			package.SourceDCId = sourceDC;
			package.DestinationDCId = destinationDC;
			package.Operator = User.Username;
			package.UpdateTime = TimeProvider.Current.Now;
		}

		#region Validity Check

		void CheckMedicationTypeIsValid(MedicationType type)
		{
			if (type == null)
			{
				throw new ENETCareException(Properties.Resources.MedicationTypeNotFound);
			}
		}

		void CheckMedicationPackageIsValid(MedicationPackage package)
		{
			if (package == null)
			{
				throw new ENETCareException(Properties.Resources.MedicationPackageNotFound);
			}
		}

		void CheckMedicationPackageStatus(MedicationPackage package, PackageStatus status)
		{
			if (package.Status != status)
			{
				throw new ENETCareException(Properties.Resources.IncorrectPackageStatus);
			}
		}

		void CheckDistributionCentreIsValid(DistributionCentre distributionCentre)
		{
			if (distributionCentre == null)
			{
				throw new ENETCareException(Properties.Resources.DistributionCentreNotFound);
			}
		}

		void CheckStockDistributionCentre(MedicationPackage package)
		{
			if (package.StockDCId != User.DistributionCentreId)
			{
				throw new ENETCareException(Properties.Resources.IncorrectDistributionCentreStock);
			}
		}

		void CheckDestinationDistributionCentre(MedicationPackage package)
		{
			if (package.DestinationDCId != User.DistributionCentreId)
			{
				throw new ENETCareException(Properties.Resources.IncorrectDistributionCentreDestination);
			}
		}

		#endregion

		#endregion
	}
}
