using System;
using System.IO;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Calculator.Tests
{
    [TestClass]
    public class CalcTests
    {
        [TestMethod]
        public void Calc_Sum_3_and_4_Results_7()
        {
            //Arrange
            var calc = new Calc();

            //Act
            var result = calc.Sum(3, 4);

            //Assert
            Assert.AreEqual(7, result);
        }

        [TestMethod]
        public void Calc_Sum_0_and_0_Results_0()
        {
            //Arrange
            var calc = new Calc();

            //Act
            var result = calc.Sum(0, 0);

            //Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Calc_Sum_MAX_and_1_throws()
        {
            var calc = new Calc();

            Assert.ThrowsException<OverflowException>(() => calc.Sum(int.MaxValue, 1));

        }

        [TestMethod]
        public void Calc_IsWeekend()
        {
            var calc = new Calc();

            using (ShimsContext.Create())
            {
                //System.IO.Fakes.ShimFile.ExistsString = s => throw new IOException();
                //Assert.IsTrue(File.Exists("weöoungwöounewglkjbwegogbew"));
                System.Fakes.ShimDateTime.NowGet = () => new DateTime(2019, 4, 1);
                Assert.IsFalse(calc.IsWeekend());//mo
                System.Fakes.ShimDateTime.NowGet = () => new DateTime(2019, 4, 2);
                Assert.IsFalse(calc.IsWeekend());//di
                System.Fakes.ShimDateTime.NowGet = () => new DateTime(2019, 4, 3);
                Assert.IsFalse(calc.IsWeekend());//mi
                System.Fakes.ShimDateTime.NowGet = () => new DateTime(2019, 4, 4);
                Assert.IsFalse(calc.IsWeekend());//do
                System.Fakes.ShimDateTime.NowGet = () => new DateTime(2019, 4, 5);
                Assert.IsFalse(calc.IsWeekend());//fr
                System.Fakes.ShimDateTime.NowGet = () => new DateTime(2019, 4, 6);
                Assert.IsTrue(calc.IsWeekend());//sa
                System.Fakes.ShimDateTime.NowGet = () => new DateTime(2019, 4, 7);
                Assert.IsTrue(calc.IsWeekend());//so
                System.Fakes.ShimDateTime.NowGet = () => new DateTime(2019, 4, 8);

            }
        }
    }
}
