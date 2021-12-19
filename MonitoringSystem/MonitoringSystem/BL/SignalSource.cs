namespace MonitoringSystem.BL
{
    public class SignalSource
    {
        public int ID { get; set; }
        public int Code { get; set; }
        public string Source { get; set; }

        public SignalSource(int ID, int code, string source)
        {
            this.ID = ID;
            this.Code = code;
            this.Source = source;
        }
    }
}
