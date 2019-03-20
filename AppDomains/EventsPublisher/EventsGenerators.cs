using EventsCommon;
using System;

namespace EventsPublisher
{
    /// <summary>
    /// Implements <see cref="IEventCommonGenerator"/> from <see cref="EventCommons"/>
    /// </summary>
    [Serializable]
    public class EventsGenerators : IEventCommonGenerator
    {
        /// <summary>
        /// Fires Event
        /// </summary>
        /// <param name="input"></param>
        public void FireEvent(string input)
        {
            this.NameGenerator?.Invoke(input);
        }

        /// <summary>
        /// Event for Publisher
        /// </summary>
        public event Action<string> NameGenerator;
    }
}