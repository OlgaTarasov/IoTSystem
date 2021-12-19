using System;
using WebSocketSharp;
using WSTransmitter.BL;
using Newtonsoft.Json;

namespace WSTransmitter
{
    public enum SignalType
    {
        Sine = 1,
        State = 2,
    }
    internal class Program
    {
        private static Random _random = new Random();
        private const int SIGNALPERIOD = 2;

        static void Main(string[] args)
        {
            using (WebSocket ws = new WebSocket("ws://localhost:9876/Iot"))
            {
                ws.Connect();

                int iteration = 0;
                int randomPeriod = _random.Next(2, 5) * 100 / SIGNALPERIOD;
                try
                {
                    var timer = new System.Threading.Timer(
                        (e) => {
                            iteration++;

                            bool isAnomalySignal = iteration % randomPeriod == 0;
                            randomPeriod = isAnomalySignal ? _random.Next(2, 5) * 100 / SIGNALPERIOD : randomPeriod;

                            ISignal sine = new SineSignal(isAnomalySignal);
                            ISignal state = new StateSignal(isAnomalySignal);

                            if (sine != null && state != null)
                            {
                                ws.Send(JsonConvert.SerializeObject(sine));
                                ws.Send(JsonConvert.SerializeObject(state));
                            }
                            else
                            {
                                Console.WriteLine("ERROR - Underfined signal!!!");
                            }

                        }, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(SIGNALPERIOD)
                    );
                }
                catch (Exception ex)
                {
                    Console.WriteLine("==================  ERROR  ================");
                    Console.WriteLine(ex.ToString());
                    Console.WriteLine("===========================================");
                }

                Console.ReadLine();
                ws.Close();
            }
        }
    }
}
