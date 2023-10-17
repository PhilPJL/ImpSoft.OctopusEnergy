using System;
using System.Collections.Generic;
using System.Globalization;
using ImpSoft.OctopusEnergy.Api.Properties;
using JetBrains.Annotations;

namespace ImpSoft.OctopusEnergy.Api;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public static class Extensions
{
    public static string ToIso8601(this DateTimeOffset value)
    {
        return $"{value.ToUniversalTime():s}Z";
    }

    public static string AsString(this Interval value)
    {
        return value switch
        {
            Interval.Hour => "hour",
            Interval.Day => "day",
            Interval.Week => "week",
            Interval.Month => "month",
            Interval.Quarter => "quarter",
            Interval.Default => "",
            _ => ""
        };
    }

    public static TariffsByPeriod ForGsp(this Dictionary<string, TariffsByPeriod> value, string gsp)
    {
        Assertions.AssertValidGsp(gsp);

        if (value.TryGetValue(gsp, out var forGsp)) return forGsp;

        throw new ArgumentOutOfRangeException(nameof(gsp),
            string.Format(CultureInfo.CurrentCulture, Resources.GspNotSupported, gsp));
    }

    public static SampleQuotesByPeriod ForGsp(this Dictionary<string, SampleQuotesByPeriod> value, string gsp)
    {
        Assertions.AssertValidGsp(gsp);

        if (value.TryGetValue(gsp, out var forGsp)) return forGsp;

        throw new ArgumentOutOfRangeException(nameof(gsp),
            string.Format(CultureInfo.CurrentCulture, Resources.GspNotSupported, gsp));
    }
}