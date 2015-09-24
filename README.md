# event-store-poc

Proof of concept (POC) for reading from event store. Different programs to read using TCP, HTTP, and using the JustGiving Event Store client.

###Running service

Need an event store with events in it first, can use this project in order to do so - [event-store-ground](https://github.com/humblelistener/event-store-ground)
This will populate your event store with randomly generated candidates.

Once you have events to read, can then run this service.
Commands to run are:

#### Using TCP
'dnx run tcp'
Which will run 'TcpEventStreamReader.cs'

#### Using HTTP
'dnx run http'
Which will run 'HttpEventStreamReader.cs'

#### Using the JustGivingClient
'dnx run jgClient'
Which will run 'JustGivingClient.cs', using the JustGiving.EventStore.Http client, and can be found [here.](https://github.com/JustGiving/JustGiving.EventStore.Http)
Refer to the [Client](https://www.nuget.org/packages/JustGiving.EventStore.Http.Client/) and [SubscriberHost](https://www.nuget.org/packages/JustGiving.EventStore.Http.SubscriberHost/) packages provided.

There is also a (not very well built) timer that starts when you run the program in HTTP or TCP, in order to compare the time it takes for each one to run. If you wish to use this, just hit enter as soon as the program has read all events, and the time should appear on the screen.
