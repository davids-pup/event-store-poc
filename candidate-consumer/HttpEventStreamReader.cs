using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.ServiceModel.Syndication;
using System.Threading;
using System.Xml;

namespace candidate_consumer
{
    public class HttpEventStreamReader
    {
		private SyndicationLink GetNamedLink(IEnumerable<SyndicationLink> links, string name)
        {
            return links.FirstOrDefault(link => link.RelationshipType == name);
        }

        private Uri GetLast(Uri head)
        {
            var request = (HttpWebRequest)WebRequest.Create(head);
            //request.Credentials = new NetworkCredential("admin", "changeit");
            request.Accept = "application/atom+xml";
            try
            {
                using (var response = (HttpWebResponse) request.GetResponse())
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                        return null;
                    using (var xmlreader = XmlReader.Create(response.GetResponseStream()))
                    {
                        var feed = SyndicationFeed.Load(xmlreader);
                        var last = GetNamedLink(feed.Links, "last");
                        return (last != null) ? last.Uri : GetNamedLink(feed.Links, "self").Uri;
                    }
                }
            }
            catch(WebException ex)
            {
                if (((HttpWebResponse) ex.Response).StatusCode == HttpStatusCode.NotFound) return null;
                throw;
            }
        }

        private void ProcessItem(SyndicationItem item)
        {
            Console.WriteLine(item.Title.Text);
            //get events
            var request = (HttpWebRequest)WebRequest.Create(GetNamedLink(item.Links, "alternate").Uri);
            request.Credentials = new NetworkCredential("admin", "changeit");
            request.Accept = "application/json";
            using (var response = request.GetResponse())
            {
                var streamReader = new StreamReader(response.GetResponseStream());
                Console.WriteLine(streamReader.ReadToEnd());
            }
        }

        private Uri ReadPrevious(Uri uri)
        {
            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Credentials = new NetworkCredential("admin", "changeit");
            request.Accept = "application/atom+xml";
            using(var response = request.GetResponse())
            {
                using(var xmlreader = XmlReader.Create(response.GetResponseStream()))
                {
                    var feed = SyndicationFeed.Load(xmlreader);
                    foreach (var item in feed.Items.Reverse())
                    {
                        ProcessItem(item);
                    }
                    var prev = GetNamedLink(feed.Links, "previous");
                    return prev == null ? uri : prev.Uri;
                }
            }
        }

        public void Start()
        {
			Console.WriteLine("Press any key to exit.");
            var stop = false;
			Uri last = null;
            while (last == null && !stop)
            {
                last = GetLast(new Uri("http://192.168.99.100:2113/streams/candidates"));
                if(last == null) Thread.Sleep(1000);
                if (Console.KeyAvailable)
                {
                    stop = true;
                }
            }

            while(!stop)
            {
                var current = ReadPrevious(last);
                if(last == current)
                {
                    Thread.Sleep(1000);
                }
                last = current;
                if(Console.KeyAvailable)
                {
                    stop = true;
                }
            }
		}
    }
}
