using EventsCommon;
using System;

namespace CrossDomainEvents
{
    /// <summary>
    /// Creates Subscriber AppDomain
    /// </summary>
    public class SubscriberAppDomain : MarshalByRefObject
    {
        public static AppDomain CustomDomain;
        public static object Instance;

        public static void SetupDomain(IEventCommonGenerator eventCommonGenerator)
        {
            // Domain Name EventsCatcher
            CustomDomain = AppDomain.CreateDomain("EventsCatcher");

            // Loads EventsSubscriber Assembly and create EventsSubscriber.EventsCatcher
            Instance = Activator.CreateInstance(CustomDomain,
                "EventsSubscriber, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
                "EventsSubscriber.EventsCatcher").Unwrap();
        }
    }
}