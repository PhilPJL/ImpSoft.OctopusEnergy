using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
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
            Response = response;
            ExpectedRequestUri = expectedUri;
        }

        public FakeHttpMessageHandler(Uri expectedUri, string response)
        {
            Preconditions.IsNotNullOrWhiteSpace(response, nameof(response));
            AutomaticDecompression = System.Net.DecompressionMethods.All;
            ResponseString = response;
            ExpectedRequestUri = expectedUri;
        }

        private TResponse Response { get; }
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

            if (Response != null)
            {
                return new HttpResponseMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(Response))
                };
            }

            return new HttpResponseMessage
            {
                Content = new StringContent(ResponseString)
            };
        }
    }
}
