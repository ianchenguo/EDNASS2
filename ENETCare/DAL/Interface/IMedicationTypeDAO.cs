using System.Collections.Generic;

namespace ENETCare.Business
{
	public interface IMedicationTypeDAO
	{
		List<MedicationType> FindAllMedicationTypes();
		MedicationType GetMedicationTypeById(int id);
	}
}
