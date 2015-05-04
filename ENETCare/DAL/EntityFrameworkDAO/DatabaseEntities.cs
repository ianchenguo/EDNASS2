using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ENETCare.Business
{
	/// <summary>
	/// A context that derives from System.Data.Entity.DbContext, and exposes a typed DbSet<TEntity> for each class in the model.
	/// </summary>
	public class DatabaseEntities : DbContext
	{
		public DatabaseEntities() : base("name=LocalDB")
		{
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
			//modelBuilder.Entity<DistributionCentre>().ToTable("DistributionCentre");
		}

		public virtual DbSet<DistributionCentre> DistributionCentre { get; set; }
		public virtual DbSet<MedicationType> MedicationType { get; set; }
	}
}
