using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ppedv.QualitySlegehammer.Model.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> GetRepo<T>() where T : Entity;

        IOrderRepository OrderRepository { get; }

        void SaveAll();
    }

    public interface IOrderRepository : IRepository<Order>
    {
        IReadOnlyCollection<Order> GetOrdersAllNew();
    }

    public interface IRepository<T> where T : Entity
    {
        IQueryable<T> Query();
        T GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
