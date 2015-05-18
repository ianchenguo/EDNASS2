using System;

namespace ENETCare.Business
{
	public class EntityFrameworkDAO : IDisposable
	{
		protected DatabaseEntities context;

		public EntityFrameworkDAO()
		{
			context = new DatabaseEntities();
		}

		public EntityFrameworkDAO(DatabaseEntities context)
		{
			this.context = context;
		}

		public void Dispose()
		{
			context.Dispose();
		}
	}
}
