using System;

namespace LogSplit.Map
{
    /// <summary>
    /// A value that is converted to the concrete type
    /// </summary>
    public class ConvertedValue
    {
        /// <summary>
        /// Creates a default value that represents a value that could not be converted
        /// </summary>
        public ConvertedValue()
        {
            Converted = false;
        }

        /// <summary>
        /// Creates a oject containing a converted value and the given type
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        public ConvertedValue(object value, Type type)
        {
            Value = value;
            Type = type;
            Converted = true;
        }

        /// <summary>
        /// Gets if the value could be converted
        /// </summary>
        public bool Converted { get; }

        /// <summary>
        /// Gets the converted value
        /// </summary>
        public object Value { get; }

        /// <summary>
        /// Gets the type that the object was converted to
        /// </summary>
        public Type Type { get; }
    }
}
