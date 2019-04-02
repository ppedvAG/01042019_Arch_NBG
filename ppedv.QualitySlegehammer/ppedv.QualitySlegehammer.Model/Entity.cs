using System;

namespace ppedv.QualitySlegehammer.Model
{
    public abstract class Entity
    {
        public int Id { get; set; }
        public DateTime Added { get; set; }
        public DateTime Modified { get; set; }
    }
}