using System.Collections.Generic;
using ENETCare.Business;

namespace ENETCare.Presentation.MVC.Models
{
	public class SendingViewModel
	{
		public ICollection<DistributionCentre> DistributionCentres
		{
			get;
			set;
		}

		public Notification Result
		{
			get;
			set;
		}
	}
}