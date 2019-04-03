using ppedv.QualitySlegehammer.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ppedv.QualitySlegehammer.Data.EF
{
    public class EfContext : DbContext
    {
        public DbSet<Device> Devices { get; set; }
        public DbSet<Order> Orders { get; set; }

        /// <summary>
        /// Steve
        /// </summary>
        public DbSet<Job> Jobs { get; set; }

        public EfContext(string conString) : base(conString)
        { }

        public override int SaveChanges()
        {
            DateTime dt = DateTime.Now;
            foreach (var item in ChangeTracker.Entries<Entity>().Where(X => X.State == EntityState.Added))
            {
                item.Entity.Added = dt;
                item.Entity.Modified = dt;
            }

            foreach (var item in ChangeTracker.Entries<Entity>().Where(X => X.State == EntityState.Modified))
            {
                item.Entity.Modified = dt;
            }

            return base.SaveChanges();
        }

        public EfContext() : this("Server=.\\SQLEXPRESS;Database=QualitySlegeHammer_dev;Trusted_Connection=true")
        { }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));
        }
    }
}
