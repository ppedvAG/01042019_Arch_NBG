using System.Collections.Generic;

namespace ppedv.QualitySlegehammer.Model
{
    public class Order : Entity
    {
        public string ProdNr { get; set; }
        public OrderStatus Status { get; set; }
        public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();
    }

    public enum OrderStatus
    {
        New,
        Running,
        Finished
    }
}