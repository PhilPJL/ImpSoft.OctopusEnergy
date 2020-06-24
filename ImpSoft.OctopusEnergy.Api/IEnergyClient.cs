using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImpSoft.OctopusEnergy.Api
{
    public interface IPublicClient
    {
        string BaseUrl { get; }

        Task<IEnumerable<Product>> GetProductsAsync(DateTimeOffset? availableAt = null, bool? isVariable = null,
            bool? isGreen = null, bool? isTracker = null, bool? isPrepay = null, bool? isBusiness = null);

        Task<ProductDetail> GetProductAsync(string productCode, DateTimeOffset? tariffsActiveAt = null);
        Task<IEnumerable<GridSupplyPoint>> GetGridSupplyPointsAsync(string postcode = null);
        Task<MeterPointGridSupplyPoint> GetGridSupplyPointAsync(string mpan);

        Task<IEnumerable<Charge>> GetElectricityUnitRatesAsync(string productCode, string tariffCode,
            ElectricityUnitRate rate = ElectricityUnitRate.Standard, DateTimeOffset? from = null,
            DateTimeOffset? to = null);

        Task<IEnumerable<Charge>> GetElectricityUnitRatesAsync(Tariff tariff,
            ElectricityUnitRate rate = ElectricityUnitRate.Standard, DateTimeOffset? from = null,
            DateTimeOffset? to = null);

        Task<IEnumerable<Charge>> GetElectricityStandingChargesAsync(string productCode, string tariffCode,
            DateTimeOffset? from = null, DateTimeOffset? to = null);

        Task<IEnumerable<Charge>> GetElectricityStandingChargesAsync(Tariff tariff, DateTimeOffset? from = null,
            DateTimeOffset? to = null);

        Task<IEnumerable<Charge>> GetGasUnitRatesAsync(string productCode, string tariffCode, DateTimeOffset? from = null,
            DateTimeOffset? to = null);

        Task<IEnumerable<Charge>> GetGasStandingChargesAsync(string productCode, string tariffCode,
            DateTimeOffset? from = null, DateTimeOffset? to = null);
    }

    public interface IPrivateClient : IPublicClient
    {
        Task<IEnumerable<Consumption>> GetElectricityConsumptionAsync(string mpan, string serialNumber,
            DateTimeOffset from, DateTimeOffset to, Interval group = Interval.Default);

        Task<IEnumerable<Consumption>> GetGasConsumptionAsync(string mprn, string serialNumber, DateTimeOffset from,
            DateTimeOffset to, Interval group = Interval.Default);
    }
}