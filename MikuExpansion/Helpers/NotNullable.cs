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

        public static explicit operator NotNullable<T>(T val)
            => new NotNullable<T>(val);

        public static explicit operator T(NotNullable<T> from) => from.Value;

        public override string ToString()
            => Value.ToString();
    }
}
