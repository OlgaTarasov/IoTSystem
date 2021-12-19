using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSTransmitter.BL
{
    internal class StateSignal : ISignal
    {
        private static Random _random = new Random();

        private SignalType _signalType;
        private string _timeStamp;
        private int _value;
        public SignalType SignalType { get; set; }
        public string TimeStamp { get; set; }
        public int Value { get; set; }

        public StateSignal(bool anomaly = false)
        {
            int value = GenerateSignal();

            this.SignalType = SignalType.State;
            this.Value = anomaly ? value * _random.Next(20, 200) : value;
            this.TimeStamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
        }
        public int GenerateSignal()
        {
            int value = _random.Next(256, 4095);
            return value;
        }

    }
}
