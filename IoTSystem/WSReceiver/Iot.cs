using Newtonsoft.Json;
using System;
using WebSocketSharp;
using WebSocketSharp.Server;
using WSReceiver.BL;

namespace WSReceiver
{
    internal class Iot : WebSocketBehavior
    {
        SignalService _signalServ = new SignalService();
        protected override void OnMessage(MessageEventArgs e)
        {
            Console.WriteLine(e.Data);
            Signal signal = JsonConvert.DeserializeObject<Signal>(e.Data);

            if (signal != null)
            {
                _signalServ.Post(signal);
            }
        }
    }
}
