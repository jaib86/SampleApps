using System;

namespace EventsCommon
{
    /// <summary>
    /// Common Interface for Publisher
    /// </summary>
    public interface IEventCommonGenerator
    {
        /// <summary>
        /// Event using <see cref="Action{T}"/>
        /// </summary>
        event Action<string> NameGenerator;

        /// <summary>
        /// Fire Events
        /// </summary>
        /// <param name="input"></param>
        void FireEvent(string input);
    }
}