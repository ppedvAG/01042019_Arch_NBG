namespace ppedv.QualitySlegehammer.Model
{
    public class Job : Entity
    {
        public string Name { get; set; }
        public virtual Order Order { get; set; }
        public JobStatus Status { get; set; }
        public virtual Device Device { get; set; }
    }

    public enum JobStatus
    {
        New,
        Running,
        OK,
        Error
    }
}