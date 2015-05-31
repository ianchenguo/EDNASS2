using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ENETCare.Business;

namespace ENETCare.Presentation.MVC.Models
{
	public class RegisteringViewModel
	{
		public string Barcode
		{
			get;
			set;
		}

		public ICollection<MedicationType> MedicationTypes
		{
			get;
			set;
		}

		public MedicationType SelectedMedicationType
		{
			get;
			set;
		}

		public IEnumerable<SelectListItem> MedicationTypeSelectList
		{
			get
			{
				return MedicationTypes.Select(x =>
					new SelectListItem
					{
						Value = x.ID.ToString(),
						Text = x.Name,
						Selected = x.ID == SelectedMedicationType.ID
					}
				);
			}
		}

		public Notification Result
		{
			get;
			set;
		}
	}
}