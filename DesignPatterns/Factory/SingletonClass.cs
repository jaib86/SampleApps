using System;

namespace Factory
{
    public sealed class SingletonClass : IDisposable
    {
        private readonly bool disposed;

        // The volatile keyword ensure tat the instantiation is complete
        // before it can be accessed further helping with thread safety.
        private static volatile SingletonClass instance;

        private static readonly object syncLock = new object();

        private SingletonClass()
        {
        }

        // Double check locking pattern
        public static SingletonClass Instance
        {
            get
            {
                if (instance != null) return instance;

                lock (syncLock)
                {
                    if (instance == null)
                    {
                        instance = new SingletonClass();
                    }
                }

                return instance;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);

            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SuppressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (this.disposed) return;
            // If disposing equals true, dispose all managed
            // and unmanaged resources
            if (disposing)
            {
                instance = null;
                // Dispose managed resources
            }

            // Call appropriate methods to clean up
            // unmanaged resources here.
        }

        public int SomeValue { get; set; }
    }
}