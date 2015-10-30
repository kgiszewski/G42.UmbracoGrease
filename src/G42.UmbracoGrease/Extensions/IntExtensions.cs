using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Models;

namespace G42.UmbracoGrease.Extensions
{
    /// <summary>
    /// Extensions for integers.
    /// </summary>
    public static class IntExtensions
    {
        /// <summary>
        /// Converts an int to the human readable bytes (i.e. 2.00 MB, 10.12 KB, etc).
        /// </summary>
        /// <param name="len">The length.</param>
        /// <returns></returns>
        public static string ToHumanReadableBytes(this int len)
        {
            var sizes = new[] { "B", "KB", "MB", "GB" };
            var order = 0;

            while (len >= 1024 && order + 1 < sizes.Length)
            {
                order++;
                len = len / 1024;
            }

            return String.Format("{0:0.##} {1}", len, sizes[order]);
        }

        /// <summary>
        /// From an integer you can get its corresponding prevalue from the DB.
        /// </summary>
        /// <param name="dtdId">The DTD identifier.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static int GetPrevalueIdByValue(this int dtdId, string value)
        {
            if (string.IsNullOrEmpty(value) || value == "0")
            {
                return 0;
            }

            return GetPrevalues(dtdId).FirstOrDefault(x => x.Value.Value == value).Value.Id;
        }

        /// <summary>
        /// From an integer, get the prevalues associated.
        /// </summary>
        /// <param name="dtdId">The DTD identifier.</param>
        /// <returns></returns>
        public static IDictionary<string, PreValue> GetPrevalues(this int dtdId)
        {
            return ApplicationContext.Current.Services.DataTypeService.GetPreValuesCollectionByDataTypeId(dtdId).PreValuesAsDictionary;
        }

        /// <summary>
        /// Converts an integer to its ordinal value (i.e. 1st, 2nd, 3rd, etc).
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string ToOrdinal(this int input)
        {
            if (input <= 0) return input.ToString();

            switch (input % 100)
            {
                case 11:
                case 12:
                case 13:
                    return input + "th";
            }

            switch (input % 10)
            {
                case 1:
                    return input + "st";
                case 2:
                    return input + "nd";
                case 3:
                    return input + "rd";
                default:
                    return input + "th";
            }
        }
    }
}