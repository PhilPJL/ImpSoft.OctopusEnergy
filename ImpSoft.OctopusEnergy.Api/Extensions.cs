﻿using System;
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
            Assertions.AssertValidGsp(gsp);

            if (value.ContainsKey(gsp)) return value[gsp];

            throw new ArgumentOutOfRangeException(
                string.Format(CultureInfo.CurrentCulture, Resources.GspNotSupported, gsp), nameof(gsp));
        }

        public static SampleQuotesByPeriod ForGsp(this Dictionary<string, SampleQuotesByPeriod> value, string gsp)
        {
            Assertions.AssertValidGsp(gsp);

            if (value.ContainsKey(gsp)) return value[gsp];

            throw new ArgumentOutOfRangeException(
                string.Format(CultureInfo.CurrentCulture, Resources.GspNotSupported, gsp), nameof(gsp));
        }
    }
}