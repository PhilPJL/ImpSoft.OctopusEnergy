using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using ImpSoft.OctopusEnergy.Api.Properties;
using JetBrains.Annotations;
using CommunityToolkit.Diagnostics;

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

#if NET8_0_OR_GREATER
        throw new ArgumentOutOfRangeException(nameof(gsp),
            string.Format(CultureInfo.CurrentCulture, _cfForGsp, gsp));
#else
        throw new ArgumentOutOfRangeException(nameof(gsp),
            string.Format(CultureInfo.CurrentCulture, Resources.GspNotSupported, gsp));
#endif
    }

    public static SampleQuotesByPeriod ForGsp(this Dictionary<string, SampleQuotesByPeriod> value, string gsp)
    {
        Assertions.AssertValidGsp(gsp);

        if (value.TryGetValue(gsp, out var forGsp)) return forGsp;

#if NET8_0_OR_GREATER
        throw new ArgumentOutOfRangeException(nameof(gsp),
            string.Format(CultureInfo.CurrentCulture, _cfForGsp, gsp));
#else
        throw new ArgumentOutOfRangeException(nameof(gsp),
            string.Format(CultureInfo.CurrentCulture, Resources.GspNotSupported, gsp));
#endif
    }

#if NET8_0_OR_GREATER
    private static readonly CompositeFormat _cfForGsp = CompositeFormat.Parse(Resources.GspNotSupported);
#endif

    public static void SetAuthenticationHeaderFromApiKey(this HttpClient httpClient, string apiKey)
    {
        Guard.IsNotNull(httpClient, nameof(httpClient));
        Guard.IsNotNullOrWhiteSpace(apiKey, nameof(apiKey));

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes(apiKey + ":")));
    }
}