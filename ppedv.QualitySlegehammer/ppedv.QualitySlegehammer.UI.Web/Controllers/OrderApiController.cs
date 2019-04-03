using ppedv.QualitySlegehammer.Logic;
using ppedv.QualitySlegehammer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ppedv.QualitySlegehammer.UI.Web.Controllers
{
    public class OrderApiController : ApiController
    {
        Core core = new Core(null, null);
        // GET: api/OrderApi
        public IEnumerable<Order> Get()
        {
            return core.UnitOfWork.OrderRepository.Query().ToList();
        }

        // GET: api/OrderApi/5
        public Order Get(int id)
        {
            return core.UnitOfWork.OrderRepository.GetById(id);
        }

        // POST: api/OrderApi
        public void Post([FromBody]Order value)
        {
            core.UnitOfWork.OrderRepository.Add(value);
            core.UnitOfWork.SaveAll();

        }

        // PUT: api/OrderApi/5
        public void Put(int id, [FromBody]Order value)
        {
            core.UnitOfWork.OrderRepository.Update(value);
            core.UnitOfWork.SaveAll();

        }

        // DELETE: api/OrderApi/5
        public void Delete(int id)
        {
            var loaded = core.UnitOfWork.OrderRepository.GetById(id);
            if (loaded != null)
            {
                core.UnitOfWork.OrderRepository.Delete(loaded);
                core.UnitOfWork.SaveAll();
            }
        }
    }
}
