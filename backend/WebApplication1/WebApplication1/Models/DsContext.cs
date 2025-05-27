using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using System.Data;


namespace WebApplication1.Models
{

	public class DsContext : IdentityDbContext<ApplicationUser>
	//public class DsContext : DbContext
	{

			public DsContext(DbContextOptions optons) : base(optons) { }
		public DbSet<Demand> Demandes { get; set; }


		public DbSet<Pharmacien> Pharmaciens { get; set; }

        public DbSet<Medecin> Medecins { get; set; }

        public DbSet<Medicament> Medicaments { get; set; }

		public DbSet<Patient> Patients { get; set; }
		
		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
	        base.OnModelCreating(modelBuilder);
	        modelBuilder.Entity<Patient>()
		        .HasOne(p => p.Medecin)
		        .WithMany(m => m.Patients)
		        .HasForeignKey(p => p.MedecinID).OnDelete(DeleteBehavior.Restrict); ;

			//Relation Demande-Patient
			modelBuilder.Entity<Demand>().HasOne(d => d.Patient)
				.WithMany(m => m.Demandes).HasForeignKey(d => d.PatID)
					.OnDelete(DeleteBehavior.Restrict);

			//modelBuilder.Entity<Medicament>()
			//	.HasOne(m => m.Demande)
			//	.WithMany() // Explicitly reference the collection
			//	.HasForeignKey(m => m.DemandeID)
			//	.OnDelete(DeleteBehavior.Restrict);

		}




	}
		
	
}
