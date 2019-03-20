using System;

namespace CrossDomainEvents
{
    /// <summary>
    /// Creates Publisher AppDomain
    /// </summary>
    public class PublisherAppDomain : MarshalByRefObject
    {
        public static AppDomain CustomDomain;
        public static object Instance;

        public static void SetupDomain()
        {
            // Domain Name EventsGenerator
            CustomDomain = AppDomain.CreateDomain("EventsGenerator");

            // Loads EventsPublisher Assembly and create EventsPublisher.EventsGenerators
            Instance = Activator.CreateInstance(CustomDomain, "EventsPublisher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "EventsPublisher.EventsGenerators").Unwrap();
        }
    }
}