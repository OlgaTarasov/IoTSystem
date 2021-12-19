using System;
using WebSocketSharp.Server;
using WSReceiver.DAL;
using System.Configuration;

namespace WSReceiver
{
    internal class Program
    {
        static void Main(string[] args)
        {
            WebSocketServer ws = new WebSocketServer("ws://localhost:9876");
            DAService.Connect(ConfigurationManager.ConnectionStrings["SignalDB"].ConnectionString);
            DAService.OpenConnection();

            ws.AddWebSocketService<Iot>("/Iot");
            ws.Start();

            Console.WriteLine("Websocket started on ws://localhost:9876/Iot");

            Console.ReadLine();
            DAService.CloseConnection();
            ws.Stop();
        }
    }
    
}
