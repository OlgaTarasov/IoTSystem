namespace MonitoringSystem.BL
{
    public enum SignalType
    {
        Sine = 1,
        State = 2,
    }
    public class Signal
    {
        public int Value { get; set; }
        public SignalType SignalType { get; set; }
        public DateTime TimeStamp { get; set; }

        public Signal(int value, SignalType type, DateTime timeStamp)
        {
            this.Value = value;
            this.SignalType = type;
            this.TimeStamp = timeStamp;
        }
    }
}
