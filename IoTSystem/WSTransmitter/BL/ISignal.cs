namespace WSTransmitter.BL
{
    internal interface ISignal
    {
        SignalType SignalType { get; set; }
        string TimeStamp { get; set; }
        int Value { get; set; }
        int GenerateSignal();
    }
}