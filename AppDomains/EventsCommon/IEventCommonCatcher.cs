namespace EventsCommon
{
    /// <summary>
    /// Common Interface for Subscriber
    /// </summary>
    public interface IEventCommonCatcher
    {
        /// <summary>
        /// Print Events executed
        /// </summary>
        /// <returns></returns>
        string PrintEvents();

        /// <summary>
        /// Subscribe to Publisher's <see cref="IEventCommonGenerator.NameGenerator"/> event
        /// </summary>
        /// <param name="commonGenerator"></param>
        void Subscribe(IEventCommonGenerator commonGenerator);
    }
}