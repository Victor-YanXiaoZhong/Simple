namespace Simple.AdminApplication.Entitys
{
    public class SysJobLog : DefaultEntityInt
    {
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public string? Duration { get; set; }
        public DateTime? NextRun { get; set; }
    }
}