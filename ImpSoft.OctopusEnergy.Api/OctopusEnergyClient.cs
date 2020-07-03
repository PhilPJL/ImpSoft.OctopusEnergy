using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ImpSoft.OctopusEnergy.Api.Properties;
using Newtonsoft.Json;

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
            var uri = new Uri($"{BaseUrl}/v1/products/")
                .AddQueryParam("available_at", availableAt)
                .AddQueryParam("is_variable", isVariable)
                .AddQueryParam("is_green", isGreen)
                .AddQueryParam("is_tracker", isTracker)
                .AddQueryParam("is_prepay", isPrepay)
                .AddQueryParam("is_business", isBusiness);

            return await GetCollectionAsync<Product>(uri);
        }

        public async Task<ProductDetail> GetProductAsync(string productCode, DateTimeOffset? tariffsActiveAt = null)
        {
            Preconditions.IsNotNullOrWhiteSpace(productCode, nameof(productCode));

            var uri = new Uri($"{BaseUrl}/v1/products/{productCode}/")
                .AddQueryParam("tariffs_active_at ", tariffsActiveAt);

            return await GetAsync<ProductDetail>(uri);
        }

        public async Task<string> GetGridSupplyPointByPostcodeAsync(string postcode)
        {
            Preconditions.IsNotNullOrWhiteSpace(postcode, nameof(postcode));

            var uri = new Uri($"{BaseUrl}/v1/industry/grid-supply-points/")
                .AddQueryParam("postcode", postcode);

            var result = await GetCollectionAsync<GridSupplyPoint>(uri);

            if (result == null || !result.Any())
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

        public async Task<string> GetGridSupplyPointByMpanAsync(string mpan)
        {
            Preconditions.IsNotNullOrWhiteSpace(mpan, nameof(mpan));

            var uriString = $"{BaseUrl}/v1/electricity-meter-points/{mpan}/";

            var result = await GetAsync<MeterPointGridSupplyPoint>(new Uri(uriString));

            if (result == null)
            {
                throw new GspException(Resources.NoGspFound);
            }

            var gsp = result.GroupId;

            Assertions.AssertValidGsp(gsp);

            return gsp;
        }

        public IEnumerable<GridSupplyPointInfo> GetGridSupplyPoints()
        {
            return new List<GridSupplyPointInfo>
            {
                new GridSupplyPointInfo{ GroupId = "_A", AreaId = "10", Area = "East England" },
                new GridSupplyPointInfo{ GroupId = "_B", AreaId = "11", Area = "East Midlands" },
                new GridSupplyPointInfo{ GroupId = "_C", AreaId = "12", Area = "London" },
                new GridSupplyPointInfo{ GroupId = "_D", AreaId = "13", Area = "North Wales, Merseyside and Cheshire" },
                new GridSupplyPointInfo{ GroupId = "_E", AreaId = "14", Area = "West Midlands" },
                new GridSupplyPointInfo{ GroupId = "_F", AreaId = "15", Area = "North East England" },
                new GridSupplyPointInfo{ GroupId = "_G", AreaId = "16", Area = "North West England" },
                new GridSupplyPointInfo{ GroupId = "_H", AreaId = "17", Area = "North Scotland" },
                new GridSupplyPointInfo{ GroupId = "_I", AreaId = "18", Area = "South Scotland" },
                new GridSupplyPointInfo{ GroupId = "_J", AreaId = "19", Area = "South East England" },
                new GridSupplyPointInfo{ GroupId = "_K", AreaId = "20", Area = "Southern England" },
                new GridSupplyPointInfo{ GroupId = "_L", AreaId = "21", Area = "South Wales" },
                new GridSupplyPointInfo{ GroupId = "_M", AreaId = "22", Area = "South West England" },
                new GridSupplyPointInfo{ GroupId = "_N", AreaId = "23", Area = "Yorkshire" },
            };
        }

        public async Task<IEnumerable<Charge>> GetElectricityUnitRatesAsync(string productCode, string tariffCode,
            ElectricityUnitRate rate = ElectricityUnitRate.Standard, DateTimeOffset? from = null,
            DateTimeOffset? to = null)
        {
            Preconditions.IsNotNullOrWhiteSpace(productCode, nameof(productCode));
            Preconditions.IsNotNullOrWhiteSpace(tariffCode, nameof(tariffCode));

            var uri = new Uri(
                    $"{BaseUrl}/v1/products/{productCode}/electricity-tariffs/{tariffCode}/{GetRateString()}-unit-rates/")
                .AddQueryParam("period_from", from)
                .AddQueryParam("period_to", to);

            return await GetCollectionAsync<Charge>(uri);

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

        public async Task<IEnumerable<Charge>> GetElectricityUnitRatesAsync(Tariff tariff,
            ElectricityUnitRate rate = ElectricityUnitRate.Standard, DateTimeOffset? from = null,
            DateTimeOffset? to = null)
        {
            Preconditions.IsNotNull(tariff, nameof(tariff));

            var link = tariff.Links.SingleOrDefault(l => l.Rel == GetRateString());

            if (link == null)
                return Enumerable.Empty<Charge>();

            var uri = new Uri(link.HRef)
                .AddQueryParam("period_from", from)
                .AddQueryParam("period_to", to);

            return await GetCollectionAsync<Charge>(uri);

            string GetRateString()
            {
                switch (rate)
                {
                    case ElectricityUnitRate.Standard: return "standard_unit_rates";
                    case ElectricityUnitRate.Day: return "day_unit_rates";
                    case ElectricityUnitRate.Night: return "night_unit_rates";
                    default: throw new ArgumentOutOfRangeException(nameof(rate));
                }
            }
        }

        public async Task<IEnumerable<Charge>> GetElectricityStandingChargesAsync(string productCode, string tariffCode,
            DateTimeOffset? from = null, DateTimeOffset? to = null)
        {
            Preconditions.IsNotNullOrWhiteSpace(productCode, nameof(productCode));
            Preconditions.IsNotNullOrWhiteSpace(tariffCode, nameof(tariffCode));

            var uri = new Uri($"{BaseUrl}/v1/products/{productCode}/electricity-tariffs/{tariffCode}/standing-charges/")
                .AddQueryParam("period_from", from)
                .AddQueryParam("period_to", to);

            return await GetCollectionAsync<Charge>(uri);
        }

        public async Task<IEnumerable<Charge>> GetGasUnitRatesAsync(string productCode, string tariffCode,
            DateTimeOffset? from = null, DateTimeOffset? to = null)
        {
            Preconditions.IsNotNullOrWhiteSpace(productCode, nameof(productCode));
            Preconditions.IsNotNullOrWhiteSpace(tariffCode, nameof(tariffCode));

            var uri = new Uri($"{BaseUrl}/v1/products/{productCode}/gas-tariffs/{tariffCode}/standard-unit-rates/")
                .AddQueryParam("period_from", from)
                .AddQueryParam("period_to", to);

            return await GetCollectionAsync<Charge>(uri);
        }

        public async Task<IEnumerable<Charge>> GetGasStandingChargesAsync(string productCode, string tariffCode,
            DateTimeOffset? from = null, DateTimeOffset? to = null)
        {
            Preconditions.IsNotNullOrWhiteSpace(productCode, nameof(productCode));
            Preconditions.IsNotNullOrWhiteSpace(tariffCode, nameof(tariffCode));

            var uri = new Uri($"{BaseUrl}/v1/products/{productCode}/gas-tariffs/{tariffCode}/standing-charges/")
                .AddQueryParam("period_from", from)
                .AddQueryParam("period_to", to);

            return await GetCollectionAsync<Charge>(uri);
        }

        public async Task<IEnumerable<Charge>> GetElectricityStandingChargesAsync(Tariff tariff,
            DateTimeOffset? from = null, DateTimeOffset? to = null)
        {
            Preconditions.IsNotNull(tariff, nameof(tariff));

            var link = tariff.Links.SingleOrDefault(l => l.Rel == "standing_charges");

            if (link == null)
                return Enumerable.Empty<Charge>();

            var uri = new Uri(link.HRef)
                .AddQueryParam("period_from", from)
                .AddQueryParam("period_to", to);

            return await GetCollectionAsync<Charge>(uri);
        }

        private int PageSize { get; } = 65000;
        public HttpClient Client { get; }

        public async Task<IEnumerable<Consumption>> GetElectricityConsumptionAsync(string apiKey, string mpan, string serialNumber,
            DateTimeOffset from, DateTimeOffset to, Interval interval = Interval.Default)
        {
            Preconditions.IsNotNullOrWhiteSpace(mpan, nameof(mpan));
            Preconditions.IsNotNullOrWhiteSpace(serialNumber, nameof(serialNumber));

            var uri = new Uri($"{BaseUrl}/v1/electricity-meter-points/{mpan}/meters/{serialNumber}/consumption/")
                .AddQueryParam(interval)
                .AddQueryParam("page_size", PageSize)
                .AddQueryParam("period_from", from)
                .AddQueryParam("period_to", to);

            return await GetCollectionAsync<Consumption>(uri, apiKey);
        }

        public async Task<IEnumerable<Consumption>> GetGasConsumptionAsync(string apiKey, string mprn, string serialNumber,
            DateTimeOffset from, DateTimeOffset to, Interval interval = Interval.Default)
        {
            Preconditions.IsNotNullOrWhiteSpace(mprn, nameof(mprn));
            Preconditions.IsNotNullOrWhiteSpace(serialNumber, nameof(serialNumber));

            var uri = new Uri($"{BaseUrl}/v1/gas-meter-points/{mprn}/meters/{serialNumber}/consumption/")
                .AddQueryParam(interval)
                .AddQueryParam("page_size", PageSize)
                .AddQueryParam("period_from", from)
                .AddQueryParam("period_to", to);

            return await GetCollectionAsync<Consumption>(uri, apiKey);
        }

        protected async Task<IEnumerable<TResult>> GetCollectionAsync<TResult>(Uri uri, string apiKey = null,
            [CallerMemberName] string caller = null)
        {
            var results = Enumerable.Empty<TResult>();

            var pages = 0;

            while (uri != null)
            {
                pages++;

                var response = await GetAsync<PagedResults<TResult>>(uri, apiKey, caller);

                results = results.Concat(response.Results ?? Enumerable.Empty<TResult>());

                uri = string.IsNullOrEmpty(response.Next) ? null : new Uri(response.Next);
            }

            Debug.WriteLine($"Pages fetched: {pages}");

            return results;
        }

        private async Task<TResult> GetAsync<TResult>(Uri uri, string apiKey = null, [CallerMemberName] string caller = null)
        {
            Debug.WriteLine(uri.ToString());

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = uri
            };

            if (!string.IsNullOrEmpty(apiKey))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes(apiKey + ":")));
            }

            using (var httpResponse = await Client.SendAsync(request))
            {
                if (!httpResponse.IsSuccessStatusCode)
                    throw new UriGetException(GetErrorMessage(httpResponse), uri);

                return JsonConvert.DeserializeObject<TResult>(await httpResponse.Content.ReadAsStringAsync());
            }

            string GetErrorMessage(HttpResponseMessage response)
            {
                caller = caller?.StripAsyncSuffix() ?? Resources.UnknownMethod;

                return string.Format(CultureInfo.CurrentCulture,
                    Resources.HttpRequestFailed, caller, response.StatusCode, response.ReasonPhrase);
            }
        }
    }
}