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

        public EfContext() : this("Server=.\\SQLEXPRESS;Database=QualitySlegeHammer_dev;Trusted_Connection=true")
        { }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));
        }
    }
}
