using System.Collections.Generic;

namespace ENETCare.Business
{
	public interface IDistributionCentreDAO
	{
		List<DistributionCentre> FindAllDistributionCentres();
		DistributionCentre GetDistributionCentreById(int id);
	}
}
