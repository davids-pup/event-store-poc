using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using log4net;

namespace candidate_consumer
{
    public class Program
    {
        public void Main(string[] args)
        {
			 //log4net.Config.BasicConfigurator.Configure();

            if (args[0] == "tcp")
            {
				// Read using tcp
				Console.WriteLine("Reading stream using tcp");

                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                new TcpEventStreamReader().Start();

				stopWatch.Stop();

				TimeSpan time = stopWatch.Elapsed;
				string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
		            time.Hours, time.Minutes, time.Seconds,
		            time.Milliseconds / 10);
		        Console.WriteLine("RunTime with tcp is " + elapsedTime);
            }
            else if (args[0] == "http")
            {
				// Read using http
				Console.WriteLine("Reading stream using http");

				Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

	            new HttpEventStreamReader().Start();

				stopWatch.Stop();

				TimeSpan time = stopWatch.Elapsed;
				string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
		            time.Hours, time.Minutes, time.Seconds,
		            time.Milliseconds / 10);
		        Console.WriteLine("RunTime with http is " + elapsedTime);
			}
			else if(args[0] == "jgClient")
			{
                Console.WriteLine("running jgClient");
                new JustGivingClient().Test();
            }

            Console.WriteLine("waiting for events. press enter to exit");
			Console.ReadLine();
        }
    }
}
