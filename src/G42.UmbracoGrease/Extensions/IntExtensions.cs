using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Models;

namespace G42.UmbracoGrease.Extensions
{
    public static class IntExtensions
    {
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

        public static int GetPrevalueIdByValue(this int dtdId, string value)
        {
            if (string.IsNullOrEmpty(value) || value == "0")
            {
                return 0;
            }

            return GetPrevalues(dtdId).FirstOrDefault(x => x.Value.Value == value).Value.Id;
        }
        public static IDictionary<string, PreValue> GetPrevalues(this int dtdId)
        {
            return ApplicationContext.Current.Services.DataTypeService.GetPreValuesCollectionByDataTypeId(dtdId).PreValuesAsDictionary;
        }
    }
}