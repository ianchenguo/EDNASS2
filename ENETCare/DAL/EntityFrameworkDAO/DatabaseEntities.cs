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
			modelBuilder.Entity<Employee>().ToTable("AspNetUsers");
			modelBuilder.Entity<EmployeeRole>().ToTable("AspNetRoles");
			modelBuilder.Entity<Employee>()
				.HasMany(i => i.EmployeeRole)
				.WithMany(s => s.Employee)
				.Map(m => m.MapLeftKey("UserId").MapRightKey("RoleId").ToTable("AspNetUserRoles"));
		}

		public virtual DbSet<DistributionCentre> DistributionCentre { get; set; }
		public virtual DbSet<Employee> Employee { get; set; }
		public virtual DbSet<MedicationType> MedicationType { get; set; }
	}
}
