using System;
using System.Threading.Tasks;
using JustGiving.EventStore.Http.Client;
using JustGiving.EventStore.Http.SubscriberHost;
using System.Collections;

namespace candidate_consumer
{
    public class JustGivingClient
    {
        public async void Test()
        {
            var connection = EventStoreHttpConnection.Create(ConnectionSettings.Default, "http://192.168.99.100:2113");
            var handlerResolver = new EventHandlerResolver();
            var typeResolver = new EventTypeResolver();

            var settingsBuilder = new EventStreamSubscriberSettingsBuilder(connection, handlerResolver, new MemoryBackedStreamPositionRepositoryForDebugging());
            settingsBuilder.WithLogger(log4net.LogManager.GetLogger(typeof(JustGivingClient)));
            settingsBuilder.WithCustomEventTypeResolver(typeResolver);

            var subscriber = EventStreamSubscriber.Create(settingsBuilder);
            var @event = await connection.ReadEventAsync("candidates", 0);
            Console.WriteLine("Atleast an event is fetched" + @event.EventInfo.Content.EventType);

            subscriber.SubscribeTo("candidates");
        }
    }

    public class EventTypeResolver : IEventTypeResolver
    {
        public Type Resolve(string fullName)
        {
            return typeof(CandidateCreatedEvent);
        }
    }

    public class EventHandlerResolver : IEventHandlerResolver
    {
        public IEnumerable GetHandlersOf(Type handlerType)
        {
            Console.WriteLine(handlerType.ToString());
			
            var array = new[] { new CandidateCreatedEventHandler() };
            return array;
        }
    }

    public class CandidateCreatedEventHandler : IHandleEventsOf<CandidateCreatedEvent>
    {
        public Task Handle(CandidateCreatedEvent @event)
        {
            Task t = Task.Factory.StartNew(() =>
            {
                Console.WriteLine(@event.ToString());
            });
			return t;
        }
        public void OnError(Exception ex, CandidateCreatedEvent @event)
        {
            Console.WriteLine(ex);
        }
    }

    public class CandidateCreatedEvent
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Id { get; set; }

        public override string ToString()
        {
            return string.Format("Name: {1}, {0}, Id: {2}", FirstName, LastName, Id);
        }
    }
}
