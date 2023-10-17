using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;
using ImpSoft.OctopusEnergy.Api.Properties;
using JetBrains.Annotations;

namespace ImpSoft.OctopusEnergy.Api;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class OctopusEnergyClient : IOctopusEnergyClient
{
    public OctopusEnergyClient(HttpClient client)
    {
        Preconditions.IsNotNull(client, nameof(client));

        Client = client;
    }

    [UsedImplicitly]
    public static string BaseUrl => "https://api.octopus.energy";

    public async Task<IEnumerable<Product>> GetProductsAsync(
        DateTimeOffset? availableAt = null, bool? isVariable = null, bool? isGreen = null, bool? isTracker = null,
        bool? isPrepay = null, bool? isBusiness = null)
    {
        var uri = ComposeGetProductsUri(availableAt, isVariable, isGreen, isTracker, isPrepay, isBusiness);
        return await GetCollectionAsync(uri, OctopusEnergyApiJsonContext.Default.PagedResultsProduct);
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

        return await GetAsync(uri, OctopusEnergyApiJsonContext.Default.ProductDetail);
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

        var result = (await GetCollectionAsync(uri, OctopusEnergyApiJsonContext.Default.PagedResultsGridSupplyPoint)).ToList();

        switch (result.Count)
        {
            case 0:
                throw new GspException(Resources.NoGspFound);
            case > 1:
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

        var result = await GetAsync(new Uri(uriString), OctopusEnergyApiJsonContext.Default.MeterPointGridSupplyPoint);

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
        DateTimeOffset? fromDateTime, DateTimeOffset? toDateTime = null, 
        ElectricityUnitRate rate = ElectricityUnitRate.Standard)
    {
        var uri = ComposeGetElectricityUnitRatesUri(productCode, tariffCode, rate, fromDateTime, toDateTime);
        return await GetCollectionAsync(uri, OctopusEnergyApiJsonContext.Default.PagedResultsCharge);
    }

    internal static Uri ComposeGetElectricityUnitRatesUri(string productCode, string tariffCode, ElectricityUnitRate rate,
        DateTimeOffset? from, DateTimeOffset? to = null)
    {
        Preconditions.IsNotNullOrWhiteSpace(productCode, nameof(productCode));
        Preconditions.IsNotNullOrWhiteSpace(tariffCode, nameof(tariffCode));

        return new Uri($"{BaseUrl}/v1/products/{productCode}/electricity-tariffs/{tariffCode}/{GetRateString()}-unit-rates/")
            .AddQueryParam("page_size", MaxTariffsPageSize)
            .AddQueryParam("period_from", from)
            .AddQueryParam("period_to", to);

        string GetRateString()
        {
            return rate switch
            {
                ElectricityUnitRate.Standard => "standard",
                ElectricityUnitRate.Day => "day",
                ElectricityUnitRate.Night => "night",
                _ => throw new ArgumentOutOfRangeException(nameof(rate))
            };
        }
    }

    public async Task<IEnumerable<Charge>> GetElectricityStandingChargesAsync(string productCode, string tariffCode,
        DateTimeOffset? fromDateTime, DateTimeOffset? toDateTime = null)
    {
        var uri = ComposeGetElectricityStandingChargesUri(productCode, tariffCode, fromDateTime, toDateTime);
        return await GetCollectionAsync(uri, OctopusEnergyApiJsonContext.Default.PagedResultsCharge);
    }

    internal static Uri ComposeGetElectricityStandingChargesUri(string productCode, string tariffCode, DateTimeOffset? from, DateTimeOffset? to = null)
    {
        Preconditions.IsNotNullOrWhiteSpace(productCode, nameof(productCode));
        Preconditions.IsNotNullOrWhiteSpace(tariffCode, nameof(tariffCode));

        return new Uri($"{BaseUrl}/v1/products/{productCode}/electricity-tariffs/{tariffCode}/standing-charges/")
            .AddQueryParam("page_size", MaxTariffsPageSize)
            .AddQueryParam("period_from", from)
            .AddQueryParam("period_to", to);
    }

    public async Task<IEnumerable<Charge>> GetGasUnitRatesAsync(string productCode, string tariffCode, DateTimeOffset? fromDateTime, DateTimeOffset? toDateTime = null)
    {
        var uri = ComposeGetGasUnitRatesUri(productCode, tariffCode, fromDateTime, toDateTime);
        return await GetCollectionAsync(uri, OctopusEnergyApiJsonContext.Default.PagedResultsCharge);
    }

    internal static Uri ComposeGetGasUnitRatesUri(string productCode, string tariffCode, DateTimeOffset? from, DateTimeOffset? to = null)
    {
        Preconditions.IsNotNullOrWhiteSpace(productCode, nameof(productCode));
        Preconditions.IsNotNullOrWhiteSpace(tariffCode, nameof(tariffCode));

        return new Uri($"{BaseUrl}/v1/products/{productCode}/gas-tariffs/{tariffCode}/standard-unit-rates/")
            .AddQueryParam("page_size", MaxTariffsPageSize)
            .AddQueryParam("period_from", from)
            .AddQueryParam("period_to", to);
    }

    public async Task<IEnumerable<Charge>> GetGasStandingChargesAsync(string productCode, string tariffCode, DateTimeOffset? fromDateTime, DateTimeOffset? toDateTime = null)
    {
        var uri = ComposeGetGasStandingChargesUri(productCode, tariffCode, fromDateTime, toDateTime);
        return await GetCollectionAsync(uri, OctopusEnergyApiJsonContext.Default.PagedResultsCharge);
    }

    internal static Uri ComposeGetGasStandingChargesUri(string productCode, string tariffCode, 
        DateTimeOffset? from, DateTimeOffset? to = null)
    {
        Preconditions.IsNotNullOrWhiteSpace(productCode, nameof(productCode));
        Preconditions.IsNotNullOrWhiteSpace(tariffCode, nameof(tariffCode));

        return new Uri($"{BaseUrl}/v1/products/{productCode}/gas-tariffs/{tariffCode}/standing-charges/")
            .AddQueryParam("page_size", MaxTariffsPageSize)
            .AddQueryParam("period_from", from)
            .AddQueryParam("period_to", to);
    }

    private static int MaxConsumptionPageSize => 25000;
    private static int MaxTariffsPageSize => 1500;

    private HttpClient Client { get; }

    public async Task<IEnumerable<Consumption>> GetElectricityConsumptionAsync(string apiKey, string mpan, string serialNumber,
        DateTimeOffset? fromDateTime, DateTimeOffset? toDateTime = null, Interval group = Interval.Default)
    {
        var uri = ComposeGetElectricityConsumptionUri(mpan, serialNumber, fromDateTime, toDateTime, group);

        return await GetCollectionAsync(uri, OctopusEnergyApiJsonContext.Default.PagedResultsConsumption, apiKey);
    }

    internal static Uri ComposeGetElectricityConsumptionUri(string mpan, string serialNumber, DateTimeOffset? from, DateTimeOffset? to, Interval interval)
    {
        Preconditions.IsNotNullOrWhiteSpace(mpan, nameof(mpan));
        Preconditions.IsNotNullOrWhiteSpace(serialNumber, nameof(serialNumber));

        return new Uri($"{BaseUrl}/v1/electricity-meter-points/{mpan}/meters/{serialNumber}/consumption/")
            .AddQueryParam(interval)
            .AddQueryParam("page_size", MaxConsumptionPageSize)
            .AddQueryParam("period_from", from)
            .AddQueryParam("period_to", to);
    }

    public async Task<IEnumerable<Consumption>> GetGasConsumptionAsync(string apiKey, string mprn, string serialNumber,
        DateTimeOffset? fromDateTime, DateTimeOffset? toDateTime = null, Interval group = Interval.Default)
    {
        var uri = ComposeGetGasConsumptionUri(mprn, serialNumber, fromDateTime, toDateTime, group);
        return await GetCollectionAsync(uri, OctopusEnergyApiJsonContext.Default.PagedResultsConsumption, apiKey);
    }

    internal static Uri ComposeGetGasConsumptionUri(string mprn, string serialNumber, DateTimeOffset? from, DateTimeOffset? to, Interval interval)
    {
        Preconditions.IsNotNullOrWhiteSpace(mprn, nameof(mprn));
        Preconditions.IsNotNullOrWhiteSpace(serialNumber, nameof(serialNumber));

        return new Uri($"{BaseUrl}/v1/gas-meter-points/{mprn}/meters/{serialNumber}/consumption/")
            .AddQueryParam(interval)
            .AddQueryParam("page_size", MaxConsumptionPageSize)
            .AddQueryParam("period_from", from)
            .AddQueryParam("period_to", to);
    }

    protected async Task<IEnumerable<TResult>> GetCollectionAsync<TResult>(Uri? uri, JsonTypeInfo<PagedResults<TResult>> typeInfo, string? apiKey = null)
    {
        var results = Enumerable.Empty<TResult>();

        var page = 0;

        while (uri != null)
        {
            var response = await GetAsync(uri, typeInfo, apiKey);

            results = results.Concat(response.Results);

            Debug.WriteLine($"Page={page}, results={response.Results.Count()}");

            uri = string.IsNullOrEmpty(response.Next) ? null : new Uri(response.Next);

            page++;
        }

        Debug.WriteLine($"Pages fetched: {page}");

        return results;
    }

    private async Task<TResult> GetAsync<TResult>(Uri uri, JsonTypeInfo<TResult> typeInfo, string? apiKey = null) where TResult : class
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

        using var httpResponse = await Client.SendAsync(request);

        httpResponse.EnsureSuccessStatusCode();

        var result = await httpResponse.Content.ReadFromJsonAsync(typeInfo) ?? throw new InvalidOperationException();

        return result;
    }
}