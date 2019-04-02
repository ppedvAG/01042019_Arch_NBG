using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ppedv.QualitySlegehammer.Model;

namespace ppedv.QualitySlegehammer.Data.EF.Tests
{
    [TestClass]
    public class EfContextTests
    {
        [TestMethod]
        public void EfContext_can_create_database()
        {
            using (var con = new EfContext())
            {
                if (con.Database.Exists())
                    con.Database.Delete();

                Assert.IsFalse(con.Database.Exists());

                con.Database.CreateIfNotExists();
                Assert.IsTrue(con.Database.Exists());
            }
        }

        [TestMethod]
        public void EfContext_can_add_device()
        {
            var dev = new Device { Name = $"Test_{Guid.NewGuid()}", Adress = 8 };
            using (var con = new EfContext())
            {
                con.Devices.Add(dev);
                con.SaveChanges();
            }

            using (var con = new EfContext())
            {
                var loaded = con.Devices.Find(dev.Id);
                Assert.AreEqual(dev.Name, loaded.Name);
            }
        }
    }
}
