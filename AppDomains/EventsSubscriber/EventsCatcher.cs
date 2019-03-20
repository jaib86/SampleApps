using EventsCommon;
using System;
using System.Collections.Generic;

namespace EventsSubscriber
{
    /// <summary>
    /// Implements <see cref="IEventCommonCatcher"/> from <see cref="EventCommons"/>
    /// </summary>
    [Serializable]
    public class EventsCatcher : IEventCommonCatcher
    {
        /// <summary>
        /// Initializes object of <see cref="ReceivedValueList"/> and <see cref="EventsCatcher"/>
        /// </summary>
        public EventsCatcher()
        {
            this.ReceivedValueList = new List<string>();
        }

        /// <summary>
        /// Subscribes to the Publisher
        /// </summary>
        /// <param name="commonGenerator"></param>
        public void Subscribe(IEventCommonGenerator commonGenerator)
        {
            if (commonGenerator != null)
            {
                commonGenerator.NameGenerator += this.CommonNameGenerator;
            }
        }

        /// <summary>
        /// Called when event fired from <see cref="IEventCommonGenerator"/> using <see cref="IEventCommonGenerator.FireEvent"/>
        /// </summary>
        /// <param name="input"></param>
        private void CommonNameGenerator(string input)
        {
            this.ReceivedValueList.Add(input);
        }

        /// <summary>
        /// Holds Events Values
        /// </summary>
        public List<string> ReceivedValueList { get; set; }

        /// <summary>
        /// Returns Comma Separated Events Value
        /// </summary>
        /// <returns></returns>
        public string PrintEvents()
        {
            return string.Join(",", this.ReceivedValueList);
        }
    }
}