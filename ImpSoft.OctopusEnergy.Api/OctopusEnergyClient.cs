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
using CommunityToolkit.Diagnostics;
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

        if(client.BaseAddress == null)
        {
            client.BaseAddress = DefaultBaseAddress;
        }
    }

    [UsedImplicitly]
    public static Uri DefaultBaseAddress => new("https://api.octopus.energy");

    public async Task<IEnumerable<Product>> GetProductsAsync(
        DateTimeOffset? availableAt = null, bool? isVariable = null, bool? isGreen = null, bool? isTracker = null,
        bool? isPrepay = null, bool? isBusiness = null)
    {
        Guard.IsNotNull(Client.BaseAddress);
        var uri = ComposeGetProductsUri(Client.BaseAddress, availableAt, isVariable, isGreen, isTracker, isPrepay, isBusiness);
        return await GetCollectionAsync(uri, OctopusEnergyApiJsonContext.Default.PagedResultsProduct);
    }

    internal static Uri ComposeGetProductsUri(Uri baseUri, DateTimeOffset? availableAt, bool? isVariable, bool? isGreen, bool? isTracker, bool? isPrepay, bool? isBusiness)
    {
        return new Uri(baseUri, $"/v1/products/")
            .AddQueryParam("available_at", availableAt)
            .AddQueryParam("is_variable", isVariable)
            .AddQueryParam("is_green", isGreen)
            .AddQueryParam("is_tracker", isTracker)
            .AddQueryParam("is_prepay", isPrepay)
            .AddQueryParam("is_business", isBusiness);
    }

    public async Task<ProductDetail> GetProductAsync(string productCode, DateTimeOffset? tariffsActiveAt = null)
    {
        Guard.IsNotNull(Client.BaseAddress);
        var uri = ComposeGetProductUri(Client.BaseAddress, productCode, tariffsActiveAt);

        return await GetAsync(uri, OctopusEnergyApiJsonContext.Default.ProductDetail);
    }

    internal static Uri ComposeGetProductUri(Uri baseUri, string productCode, DateTimeOffset? tariffsActiveAt)
    {
        Preconditions.IsNotNullOrWhiteSpace(productCode, nameof(productCode));

        return new Uri(baseUri, $"/v1/products/{productCode}/")
            .AddQueryParam("tariffs_active_at ", tariffsActiveAt);
    }

    public async Task<string> GetGridSupplyPointByPostcodeAsync(string postcode)
    {
        Guard.IsNotNull(Client.BaseAddress);
        var uri = ComposeGetGridSupplyPointByPostcodeUri(Client.BaseAddress, postcode);

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

    internal static Uri ComposeGetGridSupplyPointByPostcodeUri(Uri baseUri, string postcode)
    {
        Preconditions.IsNotNullOrWhiteSpace(postcode, nameof(postcode));

        return new Uri(baseUri, $"/v1/industry/grid-supply-points/")
            .AddQueryParam("postcode", postcode);
    }

    public async Task<string> GetGridSupplyPointByMpanAsync(string mpan)
    {
        Guard.IsNotNull(Client.BaseAddress);
        var uriString = ComposeGetGridSupplyPointByMpanUri(Client.BaseAddress, mpan);

        var result = await GetAsync(uriString, OctopusEnergyApiJsonContext.Default.MeterPointGridSupplyPoint);

        var gsp = result.GroupId;

        Assertions.AssertValidGsp(gsp);

        return gsp;
    }

    internal static Uri ComposeGetGridSupplyPointByMpanUri(Uri baseUri, string mpan)
    {
        Guard.IsNotNullOrWhiteSpace(mpan, nameof(mpan));

        return new Uri(baseUri, $"/v1/electricity-meter-points/{mpan}/");
    }

    public async Task<IEnumerable<Charge>> GetElectricityUnitRatesAsync(string productCode, string tariffCode,
        DateTimeOffset? fromDateTime, DateTimeOffset? toDateTime = null, 
        ElectricityUnitRate rate = ElectricityUnitRate.Standard)
    {
        Guard.IsNotNull(Client.BaseAddress);
        var uri = ComposeGetElectricityUnitRatesUri(Client.BaseAddress,productCode, tariffCode, rate, fromDateTime, toDateTime);
        return await GetCollectionAsync(uri, OctopusEnergyApiJsonContext.Default.PagedResultsCharge);
    }

    internal static Uri ComposeGetElectricityUnitRatesUri(Uri baseUri, string productCode, string tariffCode, ElectricityUnitRate rate,
        DateTimeOffset? from, DateTimeOffset? to = null)
    {
        Preconditions.IsNotNullOrWhiteSpace(productCode, nameof(productCode));
        Preconditions.IsNotNullOrWhiteSpace(tariffCode, nameof(tariffCode));

        return new Uri(baseUri, $"/v1/products/{productCode}/electricity-tariffs/{tariffCode}/{GetRateString()}-unit-rates/")
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
        Guard.IsNotNull(Client.BaseAddress);
        var uri = ComposeGetElectricityStandingChargesUri(Client.BaseAddress, productCode, tariffCode, fromDateTime, toDateTime);
        return await GetCollectionAsync(uri, OctopusEnergyApiJsonContext.Default.PagedResultsCharge);
    }

    internal static Uri ComposeGetElectricityStandingChargesUri(Uri baseUri, string productCode, string tariffCode, DateTimeOffset? from, DateTimeOffset? to = null)
    {
        Preconditions.IsNotNullOrWhiteSpace(productCode, nameof(productCode));
        Preconditions.IsNotNullOrWhiteSpace(tariffCode, nameof(tariffCode));

        return new Uri(baseUri, $"/v1/products/{productCode}/electricity-tariffs/{tariffCode}/standing-charges/")
            .AddQueryParam("page_size", MaxTariffsPageSize)
            .AddQueryParam("period_from", from)
            .AddQueryParam("period_to", to);
    }

    public async Task<IEnumerable<Charge>> GetGasUnitRatesAsync(string productCode, string tariffCode, DateTimeOffset? fromDateTime, DateTimeOffset? toDateTime = null)
    {
        Guard.IsNotNull(Client.BaseAddress);
        var uri = ComposeGetGasUnitRatesUri(Client.BaseAddress, productCode, tariffCode, fromDateTime, toDateTime);
        return await GetCollectionAsync(uri, OctopusEnergyApiJsonContext.Default.PagedResultsCharge);
    }

    internal static Uri ComposeGetGasUnitRatesUri(Uri baseUri, string productCode, string tariffCode, DateTimeOffset? from, DateTimeOffset? to = null)
    {
        Preconditions.IsNotNullOrWhiteSpace(productCode, nameof(productCode));
        Preconditions.IsNotNullOrWhiteSpace(tariffCode, nameof(tariffCode));

        return new Uri(baseUri, $"/v1/products/{productCode}/gas-tariffs/{tariffCode}/standard-unit-rates/")
            .AddQueryParam("page_size", MaxTariffsPageSize)
            .AddQueryParam("period_from", from)
            .AddQueryParam("period_to", to);
    }

    public async Task<IEnumerable<Charge>> GetGasStandingChargesAsync(string productCode, string tariffCode, DateTimeOffset? fromDateTime, DateTimeOffset? toDateTime = null)
    {
        Guard.IsNotNull(Client.BaseAddress);
        var uri = ComposeGetGasStandingChargesUri(Client.BaseAddress, productCode, tariffCode, fromDateTime, toDateTime);
        return await GetCollectionAsync(uri, OctopusEnergyApiJsonContext.Default.PagedResultsCharge);
    }

    internal static Uri ComposeGetGasStandingChargesUri(Uri baseUri, string productCode, string tariffCode, 
        DateTimeOffset? from, DateTimeOffset? to = null)
    {
        Preconditions.IsNotNullOrWhiteSpace(productCode, nameof(productCode));
        Preconditions.IsNotNullOrWhiteSpace(tariffCode, nameof(tariffCode));

        return new Uri(baseUri, $"/v1/products/{productCode}/gas-tariffs/{tariffCode}/standing-charges/")
            .AddQueryParam("page_size", MaxTariffsPageSize)
            .AddQueryParam("period_from", from)
            .AddQueryParam("period_to", to);
    }

    private static int MaxConsumptionPageSize => 25000;
    private static int MaxTariffsPageSize => 1500;

    private HttpClient Client { get; }

    public async Task<IEnumerable<Consumption>> GetElectricityConsumptionAsync(string mpan, string serialNumber,
        DateTimeOffset? fromDateTime, DateTimeOffset? toDateTime = null, Interval group = Interval.Default)
    {
        Guard.IsNotNull(Client.BaseAddress);
        var uri = ComposeGetElectricityConsumptionUri(Client.BaseAddress, mpan, serialNumber, fromDateTime, toDateTime, group);
        return await GetCollectionAsync(uri, OctopusEnergyApiJsonContext.Default.PagedResultsConsumption);
    }

    internal static Uri ComposeGetElectricityConsumptionUri(Uri baseUri, string mpan, string serialNumber, DateTimeOffset? from, DateTimeOffset? to, Interval interval)
    {
        Preconditions.IsNotNullOrWhiteSpace(mpan, nameof(mpan));
        Preconditions.IsNotNullOrWhiteSpace(serialNumber, nameof(serialNumber));

        return new Uri(baseUri, $"v1/electricity-meter-points/{mpan}/meters/{serialNumber}/consumption/")
            .AddQueryParam(interval)
            .AddQueryParam("page_size", MaxConsumptionPageSize)
            .AddQueryParam("period_from", from)
            .AddQueryParam("period_to", to);
    }

    public async Task<IEnumerable<Consumption>> GetGasConsumptionAsync(string mprn, string serialNumber,
        DateTimeOffset? fromDateTime, DateTimeOffset? toDateTime = null, Interval group = Interval.Default)
    {
        Guard.IsNotNull(Client.BaseAddress);
        var uri = ComposeGetGasConsumptionUri(Client.BaseAddress, mprn, serialNumber, fromDateTime, toDateTime, group);
        return await GetCollectionAsync(uri, OctopusEnergyApiJsonContext.Default.PagedResultsConsumption);
    }

    internal static Uri ComposeGetGasConsumptionUri(Uri baseUri, string mprn, string serialNumber, DateTimeOffset? from, DateTimeOffset? to, Interval interval)
    {
        Preconditions.IsNotNullOrWhiteSpace(mprn, nameof(mprn));
        Preconditions.IsNotNullOrWhiteSpace(serialNumber, nameof(serialNumber));

        return new Uri(baseUri, $"v1/gas-meter-points/{mprn}/meters/{serialNumber}/consumption/")
            .AddQueryParam(interval)
            .AddQueryParam("page_size", MaxConsumptionPageSize)
            .AddQueryParam("period_from", from)
            .AddQueryParam("period_to", to);
    }

    protected async Task<IEnumerable<TResult>> GetCollectionAsync<TResult>(Uri? uri, JsonTypeInfo<PagedResults<TResult>> typeInfo)
    {
        var results = Enumerable.Empty<TResult>();

        var page = 0;

        while (uri != null)
        {
            var response = await GetAsync(uri, typeInfo);

            results = results.Concat(response.Results);

            Debug.WriteLine($"Page={page}, results={response.Results.Count()}");

            uri = string.IsNullOrEmpty(response.Next) ? null : new Uri(response.Next);

            page++;
        }

        Debug.WriteLine($"Pages fetched: {page}");

        return results;
    }

    private async Task<TResult> GetAsync<TResult>(Uri uri, JsonTypeInfo<TResult> typeInfo) where TResult : class
    {
        var result = await Client.GetFromJsonAsync<TResult>(uri, typeInfo);

        Guard.IsNotNull(result);

        return result;
    }
}