using System;
using System.Collections.Generic;
using System.Linq;

namespace candidate_consumer
{
    public class Program
    {
        public void Main(string[] args)
        {
            if (args[0] == "tcp")
            {
				// Read using tcp
				Console.WriteLine("Reading stream using tcp");
	            new TcpEventStreamReader().Start();
			}
            else if (args[0] == "http")
            {
				// Read using http
				Console.WriteLine("Reading stream using http");
	            new HttpEventStreamReader().Start();
			}

            Console.WriteLine("waiting for events. press enter to exit");
			Console.ReadLine();
        }
    }


}
