using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace ImpSoft.OctopusEnergy.Api;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public interface IOctopusEnergyClient
{
    /// <summary>
    /// Return a list of energy products.
    /// </summary>
    /// <param name="availableAt">Show products available for new agreements on the given datetime. 
    /// Defaults to current datetime, effectively showing products that are currently available.</param>
    /// <param name="isVariable">Show only variable products.</param>
    /// <param name="isGreen">Show only green products.</param>
    /// <param name="isTracker">Show only tracker products.</param>
    /// <param name="isPrepay">Show only pre-pay products.</param>
    /// <param name="isBusiness">Show only business products.</param>
    /// <returns></returns>
    Task<IEnumerable<Product>> GetProductsAsync(DateTimeOffset? availableAt = null, bool? isVariable = null,
        bool? isGreen = null, bool? isTracker = null, bool? isPrepay = null, bool? isBusiness = null);


    /// <summary>
    /// Retrieve the details of a product (including all its tariffs) for a particular point in time.
    /// </summary>
    /// <param name="productCode">The code of the product to be retrieved, for example VAR-17-01-11.</param>
    /// <param name="tariffsActiveAt">The point in time in which to show the active charges. Defaults to current datetime.</param>
    /// <returns></returns>
    Task<ProductDetail> GetProductAsync(string productCode, DateTimeOffset? tariffsActiveAt = null);


    Task<string> GetGridSupplyPointByPostcodeAsync(string postcode);
    Task<string> GetGridSupplyPointByMpanAsync(string mpan);

    /// <summary>
    /// Retrieve electricity unit rates
    /// </summary>
    /// <param name="productCode">The code of the product to be retrieved, for example VAR-17-01-11.</param>
    /// <param name="tariffCode">The code of the tariff to be retrieved, for example E-1R-VAR-17-01-11-A.</param>
    /// <param name="fromDateTime">Start date and time. </param>
    /// <param name="toDateTime">End date and time. Defaults to no specified end date.</param>
    /// <param name="rate"></param>
    /// <returns></returns>
    Task<IEnumerable<Charge>> GetElectricityUnitRatesAsync(string productCode, string tariffCode,
        DateTimeOffset? fromDateTime, DateTimeOffset? toDateTime = null,
        ElectricityUnitRate rate = ElectricityUnitRate.Standard);

    Task<IEnumerable<Charge>> GetElectricityStandingChargesAsync(string productCode, string tariffCode,
        DateTimeOffset? fromDateTime, DateTimeOffset? toDateTime = null);

    Task<IEnumerable<Charge>> GetGasUnitRatesAsync(string productCode, string tariffCode,
        DateTimeOffset? fromDateTime, DateTimeOffset? toDateTime = null);

    Task<IEnumerable<Charge>> GetGasStandingChargesAsync(string productCode, string tariffCode,
        DateTimeOffset? fromDateTime, DateTimeOffset? toDateTime = null);

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Requires authentication
    /// </remarks>
    /// <param name="mpan"></param>
    /// <param name="serialNumber"></param>
    /// <param name="fromDateTime"></param>
    /// <param name="toDateTime"></param>
    /// <param name="group"></param>
    /// <returns></returns>
    Task<IEnumerable<Consumption>> GetElectricityConsumptionAsync(string mpan, string serialNumber,
        DateTimeOffset? fromDateTime, DateTimeOffset? toDateTime = null,
        Interval group = Interval.Default);

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Requires authentication
    /// </remarks>
    /// <param name="mprn"></param>
    /// <param name="serialNumber"></param>
    /// <param name="fromDateTime"></param>
    /// <param name="toDateTime"></param>
    /// <param name="group"></param>
    /// <returns></returns>
    Task<IEnumerable<Consumption>> GetGasConsumptionAsync(string mprn, string serialNumber,
        DateTimeOffset? fromDateTime, DateTimeOffset? toDateTime = null, 
        Interval group = Interval.Default);
}