namespace EPR.Accreditation.API.Helpers
{
    using AutoMapper;

    public static class ExtensionMethods
    {
        public static IMappingExpression<TSource, TDestination> MapOnlyNonDefault<TSource, TDestination>(this IMappingExpression<TSource, TDestination> mappingExpression)
        {
            mappingExpression.ForAllMembers(opt => opt.Condition((src, dest, sourceProp, destinationProp, res) =>
            {
                return sourceProp != null && !sourceProp.IsDefaultValue();
            }));

            return mappingExpression;
        }

        public static bool IsDefaultValue(this object value)
        {
            if (value == null)
                return true;

            Type type = value.GetType();

            // if it's a boolean value, then we'll have to allow it through,
            // otherwise false never gets mapped
            if (type == typeof(bool))
                return false;

            object defaultValue = type.IsValueType ? Activator.CreateInstance(type) : null;
            return value.Equals(defaultValue);
        }

        /// <summary>
        /// Creates a list of strings with one element
        /// </summary>
        /// <param name="value">String value to be added to a list</param>
        /// <returns>List with on element for the row answer</returns>
        public static List<string> ToListSingle(this string value)
        {
            return new List<string> { value };
        }

        /// <summary>
        /// Creates a list of strings with one element
        /// </summary>
        /// <param name="value">Decimal value to be converted and added to a list of strings</param>
        /// <returns>List with on element for the row answer</returns>
        public static List<string> ToListSingle(this decimal value)
        {
            return new List<string> { value.ToString() };
        }

        /// <summary>
        /// Creates a list of strings with one element
        /// </summary>
        /// <param name="value">Nullable decimal value to be converted and added to a list of strings</param>
        /// <returns>List with on element for the row answer</returns>
        public static List<string> ToListSingle(this decimal? value)
        {
            return new List<string> { value.ToString() };
        }
    }
}
