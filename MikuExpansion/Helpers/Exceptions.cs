using System;

namespace MikuExpansion.Helpers
{
    /// <summary>
    /// Exception class that uses a hard-coded message.
    /// Override <see cref="HardCodedException()"/> and
    /// <see cref="HardCodedException(Exception)"/> constructors.
    /// All remaining constructors inherited from <see cref="Exception"/> shall not
    /// be overriden.
    /// </summary>
    public class HardCodedException : Exception
    {
        public override string Message
        {
            get { throw new NotSupportedException("The message needs to be changed. Contact the developer."); }
        }

        [Obsolete("Use constructors that do not ask for exception message", true)]
        public HardCodedException(string message) { }

        [Obsolete("Use constructors that do not ask for exception message", true)]
        public HardCodedException(string message, Exception inner) { }

        public HardCodedException(Exception inner) : base(null, inner) { }

        public HardCodedException() : base() { }
    }

    public class EmptyEnumerable : HardCodedException
    {
        public override string Message
        {
            get { return "Empty enumerable used. Its Count() result returns 0"; }
        }
    }

    public class AttributeNotAssignedException : HardCodedException
    {
        public Type AttributeType { get; private set; }

        public override string Message
        {
            get { return $"The object is not assigned with the {AttributeType.FullName} attribute."; }
        }

        public AttributeNotAssignedException(Type type)
        {
            if (!type.IsSubclassOf(typeof(Attribute)))
                throw new InvalidOperationException($"{nameof(type)} does not derive from System.Attribute");

            AttributeType = type;
        }

        public AttributeNotAssignedException(Type type, Exception inner)
            : base(inner)
        {
            if (!type.IsSubclassOf(typeof(Attribute)))
                throw new InvalidOperationException($"{nameof(type)} does not derive from System.Attribute");

            AttributeType = type;
        }
    }
}
