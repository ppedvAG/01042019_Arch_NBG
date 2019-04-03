using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ppedv.QualitySlegehammer.Data.BinFord.Tests
{
    [TestClass]
    public class BinFordDeviceTests
    {
        [TestMethod]
        [TestCategory("BinFord")]
        public void BinFordDevice_Test()
        {
            var bfd = new BinFordDevice();

            bfd.Start(200);
        }

        [TestMethod]
        [TestCategory("BinFord")]
        public void BinFordDevice_Explosion_Test()
        {
            var bfd = new BinFordDevice();

            bool ok = false;
            bfd.ErrorEvent += (s, i) => ok = true;

            bfd.Start(200);
            bfd.Start(200);
            bfd.Start(200);

            Assert.IsTrue(ok);
        }
    }
}
