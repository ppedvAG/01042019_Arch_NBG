using ppedv.QualitySlegehammer.Model;
using ppedv.QualitySlegehammer.Model.Contracts;
using System;
using System.Linq;

namespace ppedv.QualitySlegehammer.Logic
{
    public class Core
    {
        public IRepository Repository { get; }

        public OrderStatus GetStatus(Order order)
        {
            if (order == null)
                throw new ArgumentNullException();

            if (order.Jobs.Count == 0)
                throw new InvalidOperationException();

            var jobs = Repository.Query<Job>().Where(x => x.Order.Id == order.Id).ToList();

            if (jobs.All(x => x.Status == JobStatus.New))
                return OrderStatus.New;
            else if (jobs.All(x => x.Status == JobStatus.Error || x.Status == JobStatus.OK))
                return OrderStatus.Finished;
            else
                return OrderStatus.Running;
        }

        public Core(IRepository repo) //todo: dependency injection in here
        {
            Repository = repo;
        }

        public Core() : this(new Data.EF.EfRepository())
        { }
    }
}
