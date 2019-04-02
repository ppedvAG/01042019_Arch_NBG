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
            var jobs = Repository.Query<Job>().Where(x => x.Order.Id == order.Id).ToList();

            if (jobs.All(x => x.Status == JobStatus.New))
                return OrderStatus.New;
            else if (jobs.Any(x => x.Status == JobStatus.Running))
                return OrderStatus.Running;
            else
                return OrderStatus.Finished;
        }

        public Core(IRepository repo) //todo: dependency injection in here
        {
            Repository = repo;
        }

        public Core() : this(new Data.EF.EfRepository())
        { }
    }
}
