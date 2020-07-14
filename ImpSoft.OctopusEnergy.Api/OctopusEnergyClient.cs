using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using ImpSoft.OctopusEnergy.Api.Properties;

namespace ImpSoft.OctopusEnergy.Api
{
    public class OctopusEnergyClient : IOctopusEnergyClient
    {
        public OctopusEnergyClient(HttpClient client)
        {
            Preconditions.IsNotNull(client, nameof(client));

            Client = client;
        }

        public static string BaseUrl { get; } = "https://api.octopus.energy";

        public async Task<IEnumerable<Product>> GetProductsAsync(
            DateTimeOffset? availableAt = null, bool? isVariable = null, bool? isGreen = null, bool? isTracker = null,
            bool? isPrepay = null, bool? isBusiness = null)
        {
            var uri = ComposeGetProductsUri(availableAt, isVariable, isGreen, isTracker, isPrepay, isBusiness);

            return await GetCollectionAsync<Product>(uri);
        }

        internal static Uri ComposeGetProductsUri(DateTimeOffset? availableAt, bool? isVariable, bool? isGreen, bool? isTracker, bool? isPrepay, bool? isBusiness)
        {
            return new Uri($"{BaseUrl}/v1/products/")
                .AddQueryParam("available_at", availableAt)
                .AddQueryParam("is_variable", isVariable)
                .AddQueryParam("is_green", isGreen)
                .AddQueryParam("is_tracker", isTracker)
                .AddQueryParam("is_prepay", isPrepay)
                .AddQueryParam("is_business", isBusiness);
        }

        public async Task<ProductDetail> GetProductAsync(string productCode, DateTimeOffset? tariffsActiveAt = null)
        {
            var uri = ComposeGetProductUri(productCode, tariffsActiveAt);

            return await GetAsync<ProductDetail>(uri);
        }

        internal static Uri ComposeGetProductUri(string productCode, DateTimeOffset? tariffsActiveAt)
        {
            Preconditions.IsNotNullOrWhiteSpace(productCode, nameof(productCode));

            return new Uri($"{BaseUrl}/v1/products/{productCode}/")
                .AddQueryParam("tariffs_active_at ", tariffsActiveAt);
        }

        public async Task<string> GetGridSupplyPointByPostcodeAsync(string postcode)
        {
            var uri = ComposeGetGridSupplyPointByPostcodeUri(postcode);

            var result = await GetCollectionAsync<GridSupplyPoint>(uri);

            Debug.Assert(result != null);

            if (!result.Any())
            {
                throw new GspException(Resources.NoGspFound);
            }

            if (result.Count() > 1)
            {
                throw new GspException(Resources.MultipleGspFound);
            }

            var gsp = result.Single().GroupId;

            Assertions.AssertValidGsp(gsp);

            return gsp;
        }

        internal static Uri ComposeGetGridSupplyPointByPostcodeUri(string postcode)
        {
            Preconditions.IsNotNullOrWhiteSpace(postcode, nameof(postcode));

            return new Uri($"{BaseUrl}/v1/industry/grid-supply-points/")
                .AddQueryParam("postcode", postcode);
        }

        public async Task<string> GetGridSupplyPointByMpanAsync(string mpan)
        {
            var uriString = ComposeGetGridSupplyPointByMpanUri(mpan);

            var result = await GetAsync<MeterPointGridSupplyPoint>(new Uri(uriString));

            Debug.Assert(result != null);

            var gsp = result.GroupId;

            Assertions.AssertValidGsp(gsp);

            return gsp;
        }

        internal static string ComposeGetGridSupplyPointByMpanUri(string mpan)
        {
            Preconditions.IsNotNullOrWhiteSpace(mpan, nameof(mpan));

            return $"{BaseUrl}/v1/electricity-meter-points/{mpan}/";
        }

        public async Task<IEnumerable<Charge>> GetElectricityUnitRatesAsync(string productCode, string tariffCode,
            ElectricityUnitRate rate = ElectricityUnitRate.Standard, DateTimeOffset? from = null,
            DateTimeOffset? to = null)
        {
            return await GetCollectionAsync<Charge>(ComposeGetElectricityUnitRatesUri(productCode, tariffCode, rate, from, to));
        }

        internal static Uri ComposeGetElectricityUnitRatesUri(string productCode, string tariffCode, ElectricityUnitRate rate, DateTimeOffset? from, DateTimeOffset? to)
        {
            Preconditions.IsNotNullOrWhiteSpace(productCode, nameof(productCode));
            Preconditions.IsNotNullOrWhiteSpace(tariffCode, nameof(tariffCode));

            return new Uri($"{BaseUrl}/v1/products/{productCode}/electricity-tariffs/{tariffCode}/{GetRateString()}-unit-rates/")
                .AddQueryParam("period_from", from)
                .AddQueryParam("period_to", to);

            string GetRateString()
            {
                switch (rate)
                {
                    case ElectricityUnitRate.Standard: return "standard";
                    case ElectricityUnitRate.Day: return "day";
                    case ElectricityUnitRate.Night: return "night";
                    default: throw new ArgumentOutOfRangeException(nameof(rate));
                }
            }
        }

        public async Task<IEnumerable<Charge>> GetElectricityStandingChargesAsync(string productCode, string tariffCode,
            DateTimeOffset? from = null, DateTimeOffset? to = null)
        {
            var uri = ComposeGetElectricityStandingChargesUri(productCode, tariffCode, from, to);

            return await GetCollectionAsync<Charge>(uri);
        }

        internal static Uri ComposeGetElectricityStandingChargesUri(string productCode, string tariffCode, DateTimeOffset? from, DateTimeOffset? to)
        {
            Preconditions.IsNotNullOrWhiteSpace(productCode, nameof(productCode));
            Preconditions.IsNotNullOrWhiteSpace(tariffCode, nameof(tariffCode));

            return new Uri($"{BaseUrl}/v1/products/{productCode}/electricity-tariffs/{tariffCode}/standing-charges/")
                .AddQueryParam("period_from", from)
                .AddQueryParam("period_to", to);
        }

        public async Task<IEnumerable<Charge>> GetGasUnitRatesAsync(string productCode, string tariffCode,
            DateTimeOffset? from = null, DateTimeOffset? to = null)
        {
            var uri = ComposeGetGasUnitRatesUri(productCode, tariffCode, from, to);

            return await GetCollectionAsync<Charge>(uri);
        }

        internal static Uri ComposeGetGasUnitRatesUri(string productCode, string tariffCode, DateTimeOffset? from, DateTimeOffset? to)
        {
            Preconditions.IsNotNullOrWhiteSpace(productCode, nameof(productCode));
            Preconditions.IsNotNullOrWhiteSpace(tariffCode, nameof(tariffCode));

            return new Uri($"{BaseUrl}/v1/products/{productCode}/gas-tariffs/{tariffCode}/standard-unit-rates/")
                .AddQueryParam("period_from", from)
                .AddQueryParam("period_to", to);
        }

        public async Task<IEnumerable<Charge>> GetGasStandingChargesAsync(string productCode, string tariffCode,
            DateTimeOffset? from = null, DateTimeOffset? to = null)
        {
            var uri = ComposeGetGasStandingChargesUri(productCode, tariffCode, from, to);

            return await GetCollectionAsync<Charge>(uri);
        }

        internal static Uri ComposeGetGasStandingChargesUri(string productCode, string tariffCode, DateTimeOffset? from, DateTimeOffset? to)
        {
            Preconditions.IsNotNullOrWhiteSpace(productCode, nameof(productCode));
            Preconditions.IsNotNullOrWhiteSpace(tariffCode, nameof(tariffCode));

            return new Uri($"{BaseUrl}/v1/products/{productCode}/gas-tariffs/{tariffCode}/standing-charges/")
                .AddQueryParam("period_from", from)
                .AddQueryParam("period_to", to);
        }

        private static int PageSize { get; } = 65000;
        public HttpClient Client { get; }

        public async Task<IEnumerable<Consumption>> GetElectricityConsumptionAsync(string apiKey, string mpan, string serialNumber,
            DateTimeOffset from, DateTimeOffset to, Interval interval = Interval.Default)
        {
            var uri = ComposeGetElectricityConsumptionUri(mpan, serialNumber, from, to, interval);

            return await GetCollectionAsync<Consumption>(uri, apiKey);
        }

        internal static Uri ComposeGetElectricityConsumptionUri(string mpan, string serialNumber, DateTimeOffset from, DateTimeOffset to, Interval interval)
        {
            Preconditions.IsNotNullOrWhiteSpace(mpan, nameof(mpan));
            Preconditions.IsNotNullOrWhiteSpace(serialNumber, nameof(serialNumber));

            return new Uri($"{BaseUrl}/v1/electricity-meter-points/{mpan}/meters/{serialNumber}/consumption/")
                .AddQueryParam(interval)
                .AddQueryParam("page_size", PageSize)
                .AddQueryParam("period_from", from)
                .AddQueryParam("period_to", to);
        }

        public async Task<IEnumerable<Consumption>> GetGasConsumptionAsync(string apiKey, string mprn, string serialNumber,
            DateTimeOffset from, DateTimeOffset to, Interval interval = Interval.Default)
        {
            var uri = ComposeGetGasConsumptionUri(mprn, serialNumber, from, to, interval);

            return await GetCollectionAsync<Consumption>(uri, apiKey);
        }

        internal static Uri ComposeGetGasConsumptionUri(string mprn, string serialNumber, DateTimeOffset from, DateTimeOffset to, Interval interval)
        {
            Preconditions.IsNotNullOrWhiteSpace(mprn, nameof(mprn));
            Preconditions.IsNotNullOrWhiteSpace(serialNumber, nameof(serialNumber));

            return new Uri($"{BaseUrl}/v1/gas-meter-points/{mprn}/meters/{serialNumber}/consumption/")
                .AddQueryParam(interval)
                .AddQueryParam("page_size", PageSize)
                .AddQueryParam("period_from", from)
                .AddQueryParam("period_to", to);
        }

        protected async Task<IEnumerable<TResult>> GetCollectionAsync<TResult>(Uri uri, string apiKey = null)
        {
            var results = Enumerable.Empty<TResult>();

            var pages = 0;

            while (uri != null)
            {
                pages++;

                var response = await GetAsync<PagedResults<TResult>>(uri, apiKey);

                results = results.Concat(response.Results ?? Enumerable.Empty<TResult>());

                uri = string.IsNullOrEmpty(response.Next) ? null : new Uri(response.Next);
            }

            Debug.WriteLine($"Pages fetched: {pages}");

            return results;
        }

        private async Task<TResult> GetAsync<TResult>(Uri uri, string apiKey = null)
        {
            Debug.WriteLine(uri.ToString());

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = uri
            };

            // TODO: if the api key was configured in the HttpClientHandler then this method could be reduced to:
            // await Client.GetFromJsonAsync<TResult>(uri);

            if (!string.IsNullOrEmpty(apiKey))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes(apiKey + ":")));
            }

            using (var httpResponse = await Client.SendAsync(request))
            {
                httpResponse.EnsureSuccessStatusCode();

                return await httpResponse.Content.ReadFromJsonAsync<TResult>();
            }
        }
    }
}