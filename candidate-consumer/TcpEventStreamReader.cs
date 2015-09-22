using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using System.Net;
using System.Text;

namespace candidate_consumer
{
	public class TcpEventStreamReader
    {
        public void Start()
        {
			const string STREAM = "candidates";
            const int DEFAULTPORT = 1113;
            //uncommet to enable verbose logging in client.
            var settings = ConnectionSettings.Create();//.EnableVerboseLogging().UseConsoleLogger();
            using (var conn = EventStoreConnection.Create(settings, new IPEndPoint(IPAddress.Parse("192.168.99.100"), DEFAULTPORT)))
            {
                conn.ConnectAsync().Wait();
                //Note the subscription is subscribing from the beginning every time. You could also save
                //your checkpoint of the last seen event and subscribe to that checkpoint at the beginning.
                //If stored atomically with the processing of the event this will also provide simulated
                //transactional messaging.
                var sub = conn.SubscribeToStreamFrom(STREAM, StreamPosition.Start, true,
                    (_, x) =>
                    {
                        var data = Encoding.ASCII.GetString(x.Event.Data);
                        Console.WriteLine("Received: " + x.Event.EventStreamId + ":" + x.Event.EventNumber);
                        Console.WriteLine(data);
                    });
				Console.ReadLine();
            }
		}
    }
}
