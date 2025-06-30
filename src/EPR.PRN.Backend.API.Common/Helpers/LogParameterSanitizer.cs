using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EPR.PRN.Backend.API.Common.Helpers
{
    public static class LogParameterSanitizer
    {
        /// <summary>
        /// Remove newline characters and other potentially problematic characters from the serialized JSON string.
        /// Use a method like String.Replace to replace newline characters (\n and \r) with an empty string.</summary>
        /// Ensure that the sanitized string is logged instead of the raw serialized JSON.<param name="param">The parameter to sanitize.</param>
        /// <returns>string value with no careriage returns or line feeds.</returns>
        public static string Sanitize(dynamic param)
        {
            string jsonString = JsonConvert.SerializeObject(param);          
            return jsonString.ReplaceLineEndings(string.Empty);
        }
    }
}
