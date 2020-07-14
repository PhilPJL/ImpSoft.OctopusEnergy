using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImpSoft.OctopusEnergy.Api
{
    public interface IOctopusEnergyClient
    {
        Task<IEnumerable<Product>> GetProductsAsync(DateTimeOffset? availableAt = null, bool? isVariable = null,
            bool? isGreen = null, bool? isTracker = null, bool? isPrepay = null, bool? isBusiness = null);
        Task<ProductDetail> GetProductAsync(string productCode, DateTimeOffset? tariffsActiveAt = null);

        Task<string> GetGridSupplyPointByPostcodeAsync(string postcode);
        Task<string> GetGridSupplyPointByMpanAsync(string mpan);

        Task<IEnumerable<Charge>> GetElectricityUnitRatesAsync(string productCode, string tariffCode,
            ElectricityUnitRate rate = ElectricityUnitRate.Standard, DateTimeOffset? from = null,
            DateTimeOffset? to = null);

        Task<IEnumerable<Charge>> GetElectricityStandingChargesAsync(string productCode, string tariffCode,
            DateTimeOffset? from = null, DateTimeOffset? to = null);

        Task<IEnumerable<Charge>> GetGasUnitRatesAsync(string productCode, string tariffCode,
            DateTimeOffset? from = null, DateTimeOffset? to = null);

        Task<IEnumerable<Charge>> GetGasStandingChargesAsync(string productCode, string tariffCode,
            DateTimeOffset? from = null, DateTimeOffset? to = null);

        Task<IEnumerable<Consumption>> GetElectricityConsumptionAsync(string apiKey, string mpan, string serialNumber,
            DateTimeOffset from, DateTimeOffset to, Interval group = Interval.Default);

        Task<IEnumerable<Consumption>> GetGasConsumptionAsync(string apiKey, string mprn, string serialNumber, DateTimeOffset from,
            DateTimeOffset to, Interval group = Interval.Default);
    }
}