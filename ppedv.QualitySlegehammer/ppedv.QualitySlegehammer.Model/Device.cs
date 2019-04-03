using System.Collections.Generic;

namespace ppedv.QualitySlegehammer.Model
{
    public class Device : Entity
    {
        /// <summary>
        /// Das property speichert den Namen des Gerätes
        /// </summary>
        public string Name { get; set; }
        public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();
        public int Adress { get; set; }
    }
}