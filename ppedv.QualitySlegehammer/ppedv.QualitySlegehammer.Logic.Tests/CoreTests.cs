using System;
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
            var repoMock = new Mock<IRepository>();
            var core = new Core(repoMock.Object);
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

            var repoMock = new Mock<IRepository>();
            repoMock.Setup(x => x.Query<Job>()).Returns(() =>
              {
                  return new[] { j1 }.AsQueryable();
              });

            var core = new Core(repoMock.Object);

            core.GetStatus(order).Should().Be(OrderStatus.New);
        }

        [TestMethod]
        public void Core_GetStatus_order_with_1_job_running_returns_running()
        {
            var order = new Order();
            var j1 = new Job() { Status = JobStatus.Running };
            j1.Order = order;
            order.Jobs.Add(j1);

            var repoMock = new Mock<IRepository>();
            repoMock.Setup(x => x.Query<Job>()).Returns(() =>
            {
                return new[] { j1 }.AsQueryable();
            });

            var core = new Core(repoMock.Object);

            core.GetStatus(order).Should().Be(OrderStatus.Running);
        }

        [TestMethod]
        public void Core_GetStatus_order_with_1_job_OK_returns_finished()
        {
            var order = new Order();
            var j1 = new Job() { Status = JobStatus.OK };
            j1.Order = order;
            order.Jobs.Add(j1);

            var repoMock = new Mock<IRepository>();
            repoMock.Setup(x => x.Query<Job>()).Returns(() =>
            {
                return new[] { j1 }.AsQueryable();
            });

            var core = new Core(repoMock.Object);

            core.GetStatus(order).Should().Be(OrderStatus.Finished);
        }

        [TestMethod]
        public void Core_GetStatus_order_with_1_job_FAILED_returns_finished()
        {
            var order = new Order();
            var j1 = new Job() { Status = JobStatus.Error };
            j1.Order = order;
            order.Jobs.Add(j1);

            var repoMock = new Mock<IRepository>();
            repoMock.Setup(x => x.Query<Job>()).Returns(() =>
            {
                return new[] { j1 }.AsQueryable();
            });

            var core = new Core(repoMock.Object);

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

            var repoMock = new Mock<IRepository>();
            repoMock.Setup(x => x.Query<Job>()).Returns(() => new[] { j1, j2 }.AsQueryable());
            var core = new Core(repoMock.Object);

            core.GetStatus(o).Should().Be(result);
        }
    }
}
