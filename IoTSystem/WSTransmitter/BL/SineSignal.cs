using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSTransmitter.BL
{
    internal class SineSignal : ISignal
    {
        private static Random _random = new Random();
        private static int n = 0;
        public SignalType SignalType { get; set; }
        public string TimeStamp { get; set; }
        public int Value { get; set; }
        public SineSignal(bool anomaly = false)
        {
            int value = GenerateSignal();

            this.SignalType = SignalType.Sine;
            this.Value = anomaly ? value * _random.Next(20, 200) : value;
            this.TimeStamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
        }
        public int GenerateSignal()
        {
            int sampleRate = 8000;
            double amplitude = 16;
            double frequency = 100;

            int value = 16 + (short)(amplitude * Math.Sin((2 * Math.PI * n * frequency) / sampleRate));
            n++;

            return value;
        }
    }
}
