namespace DBLogger.Domain
{
    public class Log
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public byte Severity { get; set; }
    }
}
