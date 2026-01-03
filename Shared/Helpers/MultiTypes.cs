using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MikuExpansion.Helpers
{
    public sealed class TypeAndCtorDictionary : Dictionary<Type, Func<object, object>> { }

    /// <summary>
    /// Informations needed for <see cref="MultiType{I}"/>.
    /// Derive this class and implement <see cref="GetTypes"/> and/or
    /// <see cref="GetTypesNoCtor"/>. By default they will return null.
    /// Only one instance of your derived <see cref="MultiTypeInfo"/> is
    /// made and used in the application lifetime.
    /// </summary>
    public abstract class MultiTypeInfo
    {
        /// <summary>
        /// <see cref="Type"/>s, including or excluding (recommend you to exclude) their derived types,
        /// that <see cref="MultiType{I}"/>'s value can hold/be an instance of.
        /// Here you will pass Types with their corresponding way of constructing a new instance.
        /// If you do not want to specify Types only, use <see cref="TypesWithoutConstructors"/>.
        /// Do not override this. Override <see cref="GetTypes"/> instead.
        /// </summary>
        public TypeAndCtorDictionary Types => GetTypes();

        /// <summary>
        /// <see cref="Type"/>s, including or excluding (recommend you to exclude) their derived types,
        /// that <see cref="MultiType{I}"/>'s value can hold/be an instance of.
        /// If you want to specify how instances can be constructed too, use <see cref="Types"/>.
        /// Do not override this. Override <see cref="GetTypesNoCtor"/> instead.
        /// </summary>
        public IEnumerable<Type> TypesWithoutConstructors => GetTypesNoCtor();

        /// <summary>
        /// Specifies whether types derived from any of the specified <see cref="Types"/>
        /// are allowed to use in <see cref="MultiType{I}.Set(object)"/> and such.
        /// Override <see cref="GetAllowSubTypes"/> instead of this.
        /// </summary>
        public bool AllowSubTypes => GetAllowSubTypes();

        protected virtual TypeAndCtorDictionary GetTypes() => null;
        protected virtual IEnumerable<Type> GetTypesNoCtor() => null;
        protected virtual bool GetAllowSubTypes() => false;

        public static MultiTypeInfo Instance { get; private set; } = null;

        /// <summary>
        /// Constructor.
        /// You don't even need to call this yourself, <see cref="MultiType{I}"/> will do
        /// that if <see cref="Instance"/> is not initialized.
        /// Should not be overriden.
        /// </summary>
        public MultiTypeInfo()
        {
            Instance = this;
        }
    }

    /// <summary>
    /// Special Type that accepts multiple kinds of object. Like <see cref="object"/>, but
    /// more declarative. Ideal for JSON responses/messages and such.
    /// How sad that I can't make explicit operator that we can just assign the object without
    /// explicitly create <see cref="MultiType{I}"/> and call its <see cref="Set(object)"/>.
    /// </summary>
    /// <typeparam name="I">
    /// Derives <see cref="MultiTypeInfo"/> and has a constructor with no parameters.
    /// The instance will be assigned once, won't be updated by any means.
    /// </typeparam>
    public sealed class MultiType<I> where I : MultiTypeInfo, new()
    {
        private object Value;
        private NotNullable<I> Info;

        public MultiType()
        {
            try
            {
#if WINDOWS_RT
                var test = new NotNullable<I>(
                    (I)typeof(I).GetRuntimeProperty("Instance").GetValue(null));
#else
                var test = new NotNullable<I>(
                    (I)typeof(I).GetProperty("Instance", BindingFlags.Static)
                                .GetValue(null, null));
#endif
                Info = test.Value != null ? test : new NotNullable<I>(new I());
            }
            catch
            {
                Info = new NotNullable<I>(new I());
            }
        }

        public MultiType(object value) : this() { Set(value); }

        public MultiType(NotNullable<I> i) { Info = i; }

        public MultiType(object value, NotNullable<I> info)
        {
            Info = info;
            Set(value);
        }

        /// <summary>
        /// Returns the type of the currently held value.
        /// If the current value is null, this function returns null.
        /// </summary>
        public Type GetValueType() => Value != null ? Value.GetType() : null;

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

            if (Info.Value.Types != null &&
                Info.Value.Types.Any(p => IsTypePresent(target, p.Key)))
                return (T)Value;

            if (Info.Value.TypesWithoutConstructors != null &&
                Info.Value.TypesWithoutConstructors.Any(p => IsTypePresent(target, p)))
                return (T)Value;

            throw new Exception($"Unknown type for MultiType value: {target.FullName}");
        }

        public void Set(object value)
        {
            if (value.Equals(Value))
                return;

            if (Info.Value.Types == null &&
                Info.Value.TypesWithoutConstructors == null)
                throw new NullReferenceException("Unspecified types.");

            if (Info.Value.Types != null &&
                Info.Value.Types.Any(t => IsTypePresent(value.GetType(), t.Key)))
            {
                Value = value;
                return;
            }

            if (Info.Value.TypesWithoutConstructors != null &&
                Info.Value.TypesWithoutConstructors.Any(p => IsTypePresent(value.GetType(), p)))
            {
                Value = value;
                return;
            }

            throw new Exception($"The provided object's type does not match with any of the provided types");
        }

        private bool IsTypePresent(Type target, Type lambdaType)
        {
            if (lambdaType.Equals(target))
                return true;

            bool isASubClass = target.GetTypeInfo().IsSubclassOf(lambdaType);
            return Info.Value.AllowSubTypes && isASubClass;
        }
    }
}
