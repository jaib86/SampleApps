using EventsCommon;
using System;

namespace CrossDomainEvents
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Load Publisher DLL
            PublisherAppDomain.SetupDomain();
            PublisherAppDomain.CustomDomain.Load("EventsPublisher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
            var newPublisherGenerator = PublisherAppDomain.Instance as IEventCommonGenerator;

            // Load Subscriber DLL
            SubscriberAppDomain.SetupDomain(newPublisherGenerator);
            SubscriberAppDomain.CustomDomain.Load("EventsSubscriber, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
            var newSubscriberCatcher = SubscriberAppDomain.Instance as IEventCommonCatcher;

            // Fire Event from Publisher and validate event on Subscriber
            if (newSubscriberCatcher != null && newPublisherGenerator != null)
            {
                // Subscribe Across Domains
                newSubscriberCatcher.Subscribe(newPublisherGenerator);

                // Fire Event
                newPublisherGenerator.FireEvent("First");

                // Validate Events
                Console.WriteLine(newSubscriberCatcher.PrintEvents());
            }

            Console.ReadLine();
        }
    }
}