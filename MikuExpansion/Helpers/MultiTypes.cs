using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MikuExpansion.Helpers
{
    public sealed class TypeAndCtorDictionary : Dictionary<Type, Func<object, object>> { }

    /// <summary>
    /// Informations needed for <see cref="MultiType{I}"/>.
    /// Derive this class and implement <see cref="GetTypes"/> and/or
    /// <see cref="GetTypesNoCtor"/>. By default they will return null.
    /// Only one instance of your derived <see cref="MultiTypeInfo"/> is
    /// made and used during the application lifetime.
    /// </summary>
    public abstract class MultiTypeInfo
    {
        public TypeAndCtorDictionary Types { get { return GetTypes(); } }
        public IEnumerable<Type> TypesWithoutConstructors { get { return GetTypesNoCtor(); } }

        public bool AllowSubTypes { get { return GetAllowSubTypes(); } }

        protected virtual TypeAndCtorDictionary GetTypes() => null;
        protected virtual IEnumerable<Type> GetTypesNoCtor() => null;
        protected virtual bool GetAllowSubTypes() => false;

        public static MultiTypeInfo Instance { get; private set; }

        public MultiTypeInfo()
        {
            Instance = this;
        }
    }

    public sealed class MultiType<I>
        where I : MultiTypeInfo, new()
    {
        private object Value;
        private NotNullable<I> Info;

        public MultiType()
        {
            var test = new NotNullable<I>(
                (I)typeof(I).GetProperty("Instance", System.Reflection.BindingFlags.Static)
                            .GetValue(null));
            Info = test.Value != null ? test : new NotNullable<I>(new I());
        }
        
        public MultiType(object value) : this()
        {
            Set(value);
        }

        public MultiType(NotNullable<I> info) { this.Info = info; }

        public MultiType(object value, NotNullable<I> info)
        {
            this.Info = info;
            Set(value);
        }

        public Type GetValueType() => Value.GetType();

        /// <summary>
        /// Returns the current value this class is holding as an
        /// instance of T.
        /// By default, if <see cref="MultiTypeInfo.Types"/> is not empty,
        /// this function will use it first, use <see cref="MultiType{I}.Value"/> to
        /// create new instance of T - by passing it to T's constructor.
        /// </summary>
        /// <typeparam name="T">Any type that the instance of MultiType accepts.</typeparam>
        /// <returns></returns>
        public T Get<T>()
        {
            Type target = typeof(T);

            // If the target type matches the current value's type,
            // return the casted value
            if (GetValueType().Equals(target))
                return (T)Value;

            // Care the types dictionary first.
            try
            {
                return (T)Info.Value.Types.FirstOrDefault(
                    t => IsTypePresent(target, t.Key)).Value.Invoke(Value);
            }
            catch
            {
                if (Info.Value.TypesWithoutConstructors.Any(t => IsTypePresent(target, t)))
                    return (T)Value;

                throw new Exception($"The provided object's type does not match with any of the provided types");
            }
        }

        public void Set(object value)
        {
            if (Info.Value.Types.Any(t => IsTypePresent(value.GetType(), t.Key)))
            {
                Value = value;
                return;
            }

            throw new Exception($"The provided object's type does not match with any of the provided types");
        }

        private bool IsTypePresent(Type target, Type lambdaType)
        {
            return lambdaType.Equals(target) ||
                (Info.Value.AllowSubTypes && target.IsSubclassOf(lambdaType));
        }
    }
}
