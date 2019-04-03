using ppedv.QualitySlegehammer.Model;
using ppedv.QualitySlegehammer.Model.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ppedv.QualitySlegehammer.Data.EF
{

    public class EfUnitOfWork : IUnitOfWork
    {
        private EfContext con = new EfContext();

        private EfOrderRepository orderRepo = null;
        public IOrderRepository OrderRepository
        {
            get
            {
                if (orderRepo == null)
                    orderRepo = new EfOrderRepository(con);
                return orderRepo;
            }
        }

        public void Dispose()
        {
            con.Dispose();
        }

        public IRepository<T> GetRepo<T>() where T : Entity
        {
            return new EfRepository<T>(con);
        }

        public void SaveAll()
        {
            con.SaveChanges();
        }
    }

    public class EfOrderRepository : EfRepository<Order>, IOrderRepository
    {
        public EfOrderRepository(EfContext con) : base(con)
        { }

        public IReadOnlyCollection<Order> GetOrdersAllNew()
        {
            return con.Orders.Where(x => x.Status == OrderStatus.New).ToList();
        }

    }

    public class EfRepository<T> : IRepository<T> where T : Entity
    {
        protected EfContext con;

        public EfRepository(EfContext con)
        {
            this.con = con;
        }

        public void Add(T entity)
        {
            con.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            con.Set<T>().Remove(entity);
        }

        public T GetById(int id)
        {
            return con.Set<T>().Find(id);
        }

        public IQueryable<T> Query()
        {
            return con.Set<T>();
        }

        public void Update(T entity)
        {
            var loaded = GetById(entity.Id);
            if (loaded != null)
                con.Entry(loaded).CurrentValues.SetValues(entity);
        }
    }
}
