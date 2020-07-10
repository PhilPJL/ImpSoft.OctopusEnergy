using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ImpSoft.OctopusEnergy.Api.Tests
{
    class FakeHttpMessageHandler<TResponse> : HttpClientHandler where TResponse : class
    {
        public FakeHttpMessageHandler(Uri expectedUri, TResponse response)
        {
            Preconditions.IsNotNull(response, nameof(response));
            AutomaticDecompression = System.Net.DecompressionMethods.All;
            ResponseObject = response;
            ExpectedRequestUri = expectedUri;
        }

        public FakeHttpMessageHandler(Uri expectedUri, string response)
        {
            Preconditions.IsNotNullOrWhiteSpace(response, nameof(response));
            AutomaticDecompression = System.Net.DecompressionMethods.All;
            ResponseString = response;
            ExpectedRequestUri = expectedUri;
        }

        public FakeHttpMessageHandler(IList<(Uri, TResponse response)> pages)
        {
            // TODO: enable faking/testing paged responses
        }

        private TResponse ResponseObject { get; }
        private string ResponseString { get; }
        public Uri ExpectedRequestUri { get; }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await Task.Yield();

            if (request.RequestUri.AbsoluteUri != ExpectedRequestUri.AbsoluteUri)
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

            httpResponse.Content.Headers.ContentType.MediaType = "application/json";

            return httpResponse;
        }
    }
}
