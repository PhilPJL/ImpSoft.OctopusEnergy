using System;
using System.Collections.Generic;
using System.Globalization;
using ImpSoft.OctopusEnergy.Api.Properties;

namespace ImpSoft.OctopusEnergy.Api
{
    public static class Extensions
    {
        public static string ToIso8601(this DateTimeOffset value)
        {
            return $"{value.ToUniversalTime():s}Z";
        }

        public static string AsString(this Interval value)
        {
            switch (value)
            {
                case Interval.Hour: return "hour";
                case Interval.Day: return "day";
                case Interval.Week: return "week";
                case Interval.Month: return "month";
                case Interval.Quarter: return "quarter";
                case Interval.Default: return "";
                default: return "";
            }
        }

        public static TariffsByPeriod ForGsp(this Dictionary<string, TariffsByPeriod> value, string gsp)
        {
            if (gsp.StartsWith("_") && gsp.Length > 1) gsp = gsp.Substring(1);

            if (value.ContainsKey(gsp)) return value[gsp];

            if (value.ContainsKey("_" + gsp)) return value["_" + gsp];

            throw new ArgumentOutOfRangeException(
                string.Format(CultureInfo.CurrentCulture, Resources.GspNotSupported, gsp), nameof(gsp));
        }

        public static SampleQuotesByPeriod ForGsp(this Dictionary<string, SampleQuotesByPeriod> value, string gsp)
        {
            if (gsp.StartsWith("_") && gsp.Length > 1) gsp = gsp.Substring(1);

            if (value.ContainsKey(gsp)) return value[gsp];

            if (value.ContainsKey("_" + gsp)) return value["_" + gsp];

            throw new ArgumentOutOfRangeException(
                string.Format(CultureInfo.CurrentCulture, Resources.GspNotSupported, gsp), nameof(gsp));
        }

        internal static string StripAsyncSuffix(this string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return name;

            return name.EndsWith("Async", StringComparison.OrdinalIgnoreCase)
                ? name.Substring(0, name.Length - 5)
                : name;
        }
    }
}