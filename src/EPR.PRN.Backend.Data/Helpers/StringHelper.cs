using System.ComponentModel.DataAnnotations;
using System.Reflection;
using AutoMapper.Internal;

namespace EPR.PRN.Backend.Data.Helpers
{
    public static class StringHelper
    {
        public static string TruncateString(this string input, int maxLength)
        {
            if (string.IsNullOrEmpty(input) || input.Length <= maxLength)
                return input;
            if (maxLength < 0)
                return input;
            if (maxLength == 0)
                return "";
            return maxLength <= 3
                ? input.AsSpan(0, maxLength).ToString()
                : string.Concat(input.AsSpan(0, maxLength - 3), "...");
        }

        public static List<string> TruncateStringsBasedOnMaxLengthAttributes<T>(
            this T obj,
            List<string>? excludeProperties = null
        )
            where T : class
        {
            var ret = new List<string>();
            foreach (var p in typeof(T).GetProperties())
            {
                if (
                    (excludeProperties == null || !excludeProperties.Contains(p.Name))
                    && p.IsPublic()
                    && p.CanBeSet()
                    && p.GetValue(obj) is string s
                    && p.GetCustomAttribute<MaxLengthAttribute>() is MaxLengthAttribute maxAttribute
                )
                {
                    var truncated = TruncateString(s, maxAttribute.Length);
                    if (truncated != s)
                    {
                        p.SetValue(obj, truncated);
                        ret.Add(p.Name);
                    }
                }
            }
            return ret;
        }
    }
}
