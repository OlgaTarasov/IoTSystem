namespace MonitoringSystem.BL
{
    public class SignalDTO
    {
        public int ID { get; set; }
        public int Value { get; set; }
        public DateTime TimeStamp { get; set; }

        public int IsAnomaly { get; set; }
        public string Source { get; set; }
    }
}
