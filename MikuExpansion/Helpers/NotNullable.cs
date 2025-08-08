using System;

namespace MikuExpansion.Helpers
{
    public struct NotNullable<T>
        where T : class
    {
        public T Value { get; private set; }

        public NotNullable(T val)
        {
            if (val == null)
                throw new ArgumentNullException();
            Value = val;
        }

        public void Set(T val)
        {
            if (val == null)
                throw new ArgumentNullException();
            Value = val;
        }
    }
}
