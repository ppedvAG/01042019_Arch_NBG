using System;
using AutoFixture;
using FluentAssertions;
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
        public void EfContext_can_CRUD_device()
        {
            var dev = new Device { Name = $"Test_{Guid.NewGuid()}", Adress = 8 };
            var newName = $"NEW_TEST_NAME_{Guid.NewGuid()}";

            using (var con = new EfContext())
            {
                //CREATE
                con.Devices.Add(dev);
                con.SaveChanges();
            }

            using (var con = new EfContext())
            {
                //READ(CREATE)
                var loaded = con.Devices.Find(dev.Id);
                Assert.AreEqual(dev.Name, loaded.Name);

                //UPDATE
                loaded.Name = newName;
                con.SaveChanges();
            }

            using (var con = new EfContext())
            {
                //READ(UPDATE)
                var loaded = con.Devices.Find(dev.Id);
                Assert.AreEqual(newName, loaded.Name);

                //DELETE
                con.Devices.Remove(loaded);
                con.SaveChanges();
            }

            using (var con = new EfContext())
            {
                //READ(DELETE)
                var loaded = con.Devices.Find(dev.Id);
                Assert.IsNull(loaded);
            }
        }

        [TestMethod]
        public void EfContext_can_CRUD_device_AutoFix()
        {
            var fix = new Fixture();
            fix.Behaviors.Add(new OmitOnRecursionBehavior());

            var dev = fix.Build<Device>().Without(x => x.Jobs).Create();

            var newDev = fix.Build<Device>().Without(x => x.Jobs).Create();

            using (var con = new EfContext())
            {
                //CREATE
                con.Devices.Add(dev);
                con.SaveChanges();
            }

            using (var con = new EfContext())
            {
                //READ(CREATE)
                var loaded = con.Devices.Find(dev.Id);

                //Assert.AreEqual(dev.Name + "-", loaded.Name);
                loaded.Name.Should().Be(dev.Name);
                loaded.Should().BeEquivalentTo(dev, c => c.Excluding(x => x.Id));

                //UPDATE
                newDev.Id = loaded.Id;
                con.Entry(loaded).CurrentValues.SetValues(newDev);
                con.SaveChanges();
            }

            using (var con = new EfContext())
            {
                //READ(UPDATE)
                var loaded = con.Devices.Find(dev.Id);
                loaded.Should().BeEquivalentTo(newDev, c => c.Excluding(x => x.Id));


                //DELETE
                con.Devices.Remove(loaded);
                con.SaveChanges();
            }


            using (var con = new EfContext())
            {
                //READ(DELETE)
                var loaded = con.Devices.Find(dev.Id);
                Assert.IsNull(loaded);
            }


        }
    }
}
