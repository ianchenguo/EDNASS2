using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ENETCare.Business;

namespace ENETCareTest
{
	[TestClass]
	public class DistributionCentreDALTest : DatabaseAccessLayerTest
	{
		IDistributionCentreDAO dao;

		[TestInitialize]
		public void Setup()
		{
			dao = new DistributionCentreEntityFrameworkDAO();
		}

		#region PrepareTestData

		protected override void PrepareTestData()
		{
			DeleteExistingData();
			CreateTestDistributionCentres();
		}

		#endregion

		[TestMethod()]
		public void DistributionCentreDAO_FindAllDistributionCentres_ReturnsAllDistributionCentres()
		{
			List<DistributionCentre> distributionCentres = dao.FindAllDistributionCentres();
			Assert.AreEqual(3, distributionCentres.Count);
		}

		[TestMethod()]
		public void DistributionCentreDAO_GetDistributionCentreById_ReturnsCorrespondingDistributionCentre()
		{
			DistributionCentre dc;
			
			dc = dao.GetDistributionCentreById(1);
			Assert.IsNotNull(dc);
			Assert.AreEqual(1, dc.ID);
			Assert.AreEqual("TEST DC1", dc.Name);

			dc = dao.GetDistributionCentreById(3);
			Assert.IsNotNull(dc);
			Assert.AreEqual(3, dc.ID);
			Assert.AreEqual("TEST DC3", dc.Name);

			dc = dao.GetDistributionCentreById(5);
			Assert.IsNull(dc);
		}
	}
}
