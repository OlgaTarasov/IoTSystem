using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSReceiver.DAL;

namespace WSReceiver.BL
{
    public enum SignalType
    {
        Sine = 1,
        State = 2,
    }
    internal class Signal
    {
        const int MAX_SINE_VALUE = 32;
        const int MIN_SINE_VALUE = 0;
        const int MAX_STATE_VALUE = 4095;
        const int MIN_STATE_VALUE = 256;
        public int Value { get; set; }
        public SignalType SignalType { get; set; }
        public string TimeStamp { get; set; }
        public bool IsAnomalySignal
        {
            get
            {
                return 
                    !(this.Value <= (this.SignalType == SignalType.Sine ? MAX_SINE_VALUE : MAX_STATE_VALUE) 
                    && this.Value >= (this.SignalType == SignalType.Sine ? MIN_SINE_VALUE : MIN_STATE_VALUE));
            }
        }
    }
}
