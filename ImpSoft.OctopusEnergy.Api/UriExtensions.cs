using System;
using System.Globalization;

namespace ImpSoft.OctopusEnergy.Api;

public static class UriExtensions
{
    public static Uri AddQueryParam(this Uri uri, Interval interval)
    {
        return interval != Interval.Default ? uri.AddQueryParam("group_by", interval.AsString()) : uri;
    }

    public static Uri AddQueryParam(this Uri uri, string name, DateTimeOffset? value)
    {
        return value == null ? uri : uri.AddQueryParam(name, value.Value.ToIso8601());
    }

    public static Uri AddQueryParam(this Uri uri, string name, bool? value)
    {
        return value == null ? uri : uri.AddQueryParam(name, value.Value ? "true" : "false");
    }

    public static Uri AddQueryParam(this Uri uri, string name, int value)
    {
        return uri.AddQueryParam(name, value.ToString(CultureInfo.InvariantCulture));
    }

    public static Uri AddQueryParam(this Uri uri, string name, string value)
    {
        var baseUri = new UriBuilder(uri);

        var param = $"{name}={value}";

        if (baseUri.Query.Length > 1)
        {
#if NET7_0_OR_GREATER
            var sub = baseUri.Query[1..];
#else
            var sub = baseUri.Query.Substring(1);
#endif
            baseUri.Query = sub + "&" + param;
        }
        else
        {
            baseUri.Query = param;
        }

        return baseUri.Uri;
    }
}