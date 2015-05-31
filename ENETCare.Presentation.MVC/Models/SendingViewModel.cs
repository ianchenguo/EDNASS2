using System.Collections.Generic;
using ENETCare.Business;

namespace ENETCare.Presentation.MVC.Models
{
	public class SendingViewModel
	{
		public ICollection<DistributionCentre> DistributionCentreList
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