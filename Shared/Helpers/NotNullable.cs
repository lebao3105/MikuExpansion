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

        public static implicit operator NotNullable<T>(T val)
            => new NotNullable<T>(val);

        public static implicit operator T(NotNullable<T> from) => from.Value;

        public override string ToString()
            => Value.ToString();
    }

    /// <summary>
    /// Struct that holds a set-once data object.
    /// Not really set-once, since you can just create a new
    /// instance of <see cref="SetOnce{T}"/>, and that may happend
    /// in some situations?? I don't know.
    /// Can be casted from+to <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct SetOnce<T>
    {
        public T Value { get; private set; }

        public SetOnce(T val)
        {
            Value = val;
        }

        public static implicit operator SetOnce<T>(T val)
            => new SetOnce<T>(val);

        public static implicit operator T(SetOnce<T> from)
            => from.Value;

        public override string ToString()
            => Value.ToString();

        public override int GetHashCode()
            => Value.GetHashCode();

        public override bool Equals(object obj)
            => obj.GetType() == typeof(SetOnce<T>) &&
               ((SetOnce<T>)obj).Value.Equals(Value);
    }
}
