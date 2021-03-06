﻿using System;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ppedv.QualitySlegehammer.Model;
using ppedv.QualitySlegehammer.Model.Contracts;

namespace ppedv.QualitySlegehammer.Logic.Tests
{
    [TestClass]
    public class CoreTests
    {
        [TestMethod]
        public void Core_GetStatus_order_is_null_throws_ArgumentNullException()
        {
            var core = new Core(null);

            Assert.ThrowsException<ArgumentNullException>(() => core.GetStatus(null));
        }

        [TestMethod]
        public void Core_GetStatus_order_without_jobs_thows_()
        {
            var repoMock = new Mock<IRepository<Job>>();
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(x => x.GetRepo<Job>()).Returns(() => repoMock.Object);
            var core = new Core(uowMock.Object);
            var order = new Order();

            Assert.ThrowsException<InvalidOperationException>(() => core.GetStatus(order));
        }

        [TestMethod]
        public void Core_GetStatus_order_with_1_job_new_returns_new()
        {
            var order = new Order();
            var j1 = new Job() { Status = JobStatus.New };
            j1.Order = order;
            order.Jobs.Add(j1);


            var repoMock = new Mock<IRepository<Job>>();
            repoMock.Setup(x => x.Query()).Returns(() =>
              {
                  return new[] { j1 }.AsQueryable();
              });

            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(x => x.GetRepo<Job>()).Returns(() => repoMock.Object);
            var core = new Core(uowMock.Object);

            core.GetStatus(order).Should().Be(OrderStatus.New);
        }

        [TestMethod]
        public void Core_GetStatus_order_with_1_job_running_returns_running()
        {
            var order = new Order();
            var j1 = new Job() { Status = JobStatus.Running };
            j1.Order = order;
            order.Jobs.Add(j1);

            var repoMock = new Mock<IRepository<Job>>();
            repoMock.Setup(x => x.Query()).Returns(() =>
            {
                return new[] { j1 }.AsQueryable();
            });
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(x => x.GetRepo<Job>()).Returns(() => repoMock.Object);
            var core = new Core(uowMock.Object);

            core.GetStatus(order).Should().Be(OrderStatus.Running);
        }

        [TestMethod]
        public void Core_GetStatus_order_with_1_job_OK_returns_finished()
        {
            var order = new Order();
            var j1 = new Job() { Status = JobStatus.OK };
            j1.Order = order;
            order.Jobs.Add(j1);

            var repoMock = new Mock<IRepository<Job>>();
            repoMock.Setup(x => x.Query()).Returns(() =>
            {
                return new[] { j1 }.AsQueryable();
            });

            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(x => x.GetRepo<Job>()).Returns(() => repoMock.Object);
            var core = new Core(uowMock.Object);

            core.GetStatus(order).Should().Be(OrderStatus.Finished);
        }

        [TestMethod]
        public void Core_GetStatus_order_with_1_job_FAILED_returns_finished()
        {
            var order = new Order();
            var j1 = new Job() { Status = JobStatus.Error };
            j1.Order = order;
            order.Jobs.Add(j1);

            var repoMock = new Mock<IRepository<Job>>();
            repoMock.Setup(x => x.Query()).Returns(() =>
            {
                return new[] { j1 }.AsQueryable();
            });

            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(x => x.GetRepo<Job>()).Returns(() => repoMock.Object);
            var core = new Core(uowMock.Object);

            core.GetStatus(order).Should().Be(OrderStatus.Finished);
        }


        [TestMethod]
        [TestCategory("Core")]
        [DataRow(JobStatus.New, JobStatus.New, OrderStatus.New)]
        [DataRow(JobStatus.New, JobStatus.Running, OrderStatus.Running)]
        [DataRow(JobStatus.New, JobStatus.OK, OrderStatus.Running)]
        [DataRow(JobStatus.New, JobStatus.Error, OrderStatus.Running)]
        [DataRow(JobStatus.Running, JobStatus.New, OrderStatus.Running)]
        [DataRow(JobStatus.Running, JobStatus.OK, OrderStatus.Running)]
        [DataRow(JobStatus.Running, JobStatus.Error, OrderStatus.Running)]
        [DataRow(JobStatus.Running, JobStatus.Running, OrderStatus.Running)]
        [DataRow(JobStatus.OK, JobStatus.New, OrderStatus.Running)]
        [DataRow(JobStatus.OK, JobStatus.OK, OrderStatus.Finished)]
        [DataRow(JobStatus.OK, JobStatus.Error, OrderStatus.Finished)]
        [DataRow(JobStatus.OK, JobStatus.Running, OrderStatus.Running)]
        [DataRow(JobStatus.Error, JobStatus.New, OrderStatus.Running)]
        [DataRow(JobStatus.Error, JobStatus.OK, OrderStatus.Finished)]
        [DataRow(JobStatus.Error, JobStatus.Error, OrderStatus.Finished)]
        [DataRow(JobStatus.Error, JobStatus.Running, OrderStatus.Running)]
        public void Core_GetStatus_order_jobs(JobStatus job1, JobStatus job2, OrderStatus result)
        {
            var o = new Order();
            var j1 = new Job() { Status = job1, Order = o };
            var j2 = new Job() { Status = job2, Order = o };
            o.Jobs.Add(j1);
            o.Jobs.Add(j2);

            var repoMock = new Mock<IRepository<Job>>();
            repoMock.Setup(x => x.Query()).Returns(() => new[] { j1, j2 }.AsQueryable());

            var orderRepoMock = new Mock<IOrderRepository>();
            orderRepoMock.Setup(x => x.GetOrdersAllNew()).Returns(() => new[] { new Order(), new Order() });

            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(x => x.GetRepo<Job>()).Returns(() => repoMock.Object);
            uowMock.Setup(x => x.OrderRepository).Returns(() => orderRepoMock.Object);

            var core = new Core(uowMock.Object);

            core.GetStatus(o).Should().Be(result);
        }


        [TestMethod]
        public void Core_BeepAlleDevices_Test()
        {
            var d1 = new Mock<IDevice>();
            var d2 = new Mock<IDevice>();

            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(x => x.GetRepo<Device>().Query()).Returns(() =>
            {
                return new[] { new Device() { Adress = 8 }, new Device() { Adress = 220 } }.AsQueryable();
            });

          //// d1.Setup(x => x.Start(It.IsAny<int>())).Verifiable();
           // d2.Setup(x => x.Start(It.IsAny<int>()));


            var core = new Core(uowMock.Object, new[] { d1.Object, d2.Object });
            core.BeepAlleDevices(200);

            d1.Verify(x => x.Start(It.IsAny<int>()), Times.Exactly(2));
            d1.Verify(x => x.Start(200), Times.AtLeast(1));

            d2.Verify(x => x.Start(200), Times.Never);
            d2.Verify(x => x.Start(201), Times.Never);


        }

    }
}
