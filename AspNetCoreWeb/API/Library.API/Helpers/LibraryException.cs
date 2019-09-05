using System;

namespace Library.API.Helpers
{
    public class LibraryException : Exception
    {
        public LibraryException(string message) : base(message)
        {
        }

        public LibraryException() : base()
        {
        }

        public LibraryException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}