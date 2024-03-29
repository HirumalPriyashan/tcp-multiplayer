﻿namespace TBGServer.Helpers
{
    public class ThreadSafe<T>
    {
        private T _value;
        private object _lock = new object();

        public T Value
        {
            get
            {
                lock (_lock)
                    return _value;
            }

            set
            {
                lock (_lock)
                    _value = value;
            }
        }

        public ThreadSafe(T value = default(T))
        {
            Value = value;
        }
    }
}
