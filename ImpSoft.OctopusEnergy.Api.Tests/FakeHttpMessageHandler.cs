using CommunityToolkit.Diagnostics;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ImpSoft.OctopusEnergy.Api.Tests;

internal class FakeHttpMessageHandler<TResponse> : HttpClientHandler where TResponse : class, new()
{
    public FakeHttpMessageHandler(Uri expectedUri, TResponse response)
    {
        Guard.IsNotNull(response);
        Guard.IsNotNull(expectedUri);

#if NET48_OR_GREATER
        AutomaticDecompression = System.Net.DecompressionMethods.None;
#else
        AutomaticDecompression = System.Net.DecompressionMethods.All;
#endif
        ResponseObject = response;
        ExpectedRequestUri = expectedUri;

        ResponseString = string.Empty;
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public Uri ExpectedRequestUri { get; }
    private TResponse? ResponseObject { get; }
    private string ResponseString { get; }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        await Task.Yield();

        if (request.RequestUri?.AbsoluteUri != ExpectedRequestUri.AbsoluteUri)
        {
            return new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
        }

        Debug.WriteLine(ExpectedRequestUri.AbsoluteUri);

        var httpResponse = ResponseObject != null
            ? new HttpResponseMessage { Content = new StringContent(JsonSerializer.Serialize(ResponseObject)) }
            : new HttpResponseMessage { Content = new StringContent(ResponseString) };

        if (httpResponse.Content.Headers.ContentType != null)
        {
            httpResponse.Content.Headers.ContentType.MediaType = "application/json";
        }
        else
        {
            throw new InvalidOperationException(
                $"{nameof(httpResponse.Content.Headers.ContentType)} is unexpectedly null.");
        }

        return httpResponse;
    }
}