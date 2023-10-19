using System;
using System.Net.Http;

namespace ImpSoft.OctopusEnergy.Api.Tests;

internal static class TestHelper
{
    public static IOctopusEnergyClient CreateClient<TResponse>(Uri expectedUri, TResponse response)
        where TResponse : class, new()
    {
        // Do I care about disposing these?
        var httpClient = new HttpClient(new FakeHttpMessageHandler<TResponse>(expectedUri, response));

        return new OctopusEnergyClient(httpClient);
    }

    public static IOctopusEnergyClient CreateClient<TResponse>(string expectedUri, TResponse response)
        where TResponse : class, new()
    {
        // Do I care about disposing these?
        var httpClient = new HttpClient(new FakeHttpMessageHandler<TResponse>(new Uri(expectedUri), response));

        return new OctopusEnergyClient(httpClient);
    }

    /*public static IOctopusEnergyClient CreateClient(string expectedUri, string response)
    {
        // Do I care about disposing these?
        var httpClient = new HttpClient(new FakeHttpMessageHandler<string>(new Uri(expectedUri), response));

        return new OctopusEnergyClient(httpClient);
    }*/
}