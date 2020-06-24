using ImpSoft.OctopusEnergy.Api;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImpSoft.OctopusEnergy
{
    internal class PrivateClient : PublicClient, IPrivateClient
    {
        public PrivateClient(string apiKey) : base(apiKey)
        {
        }

        private int PageSize { get; } = 65000;

        public async Task<IEnumerable<Consumption>> GetElectricityConsumptionAsync(string mpan, string serialNumber,
            DateTimeOffset from, DateTimeOffset to, Interval interval = Interval.Default)
        {
            Preconditions.IsNotNullOrWhiteSpace(mpan, nameof(mpan));
            Preconditions.IsNotNullOrWhiteSpace(serialNumber, nameof(serialNumber));

            var uri = new Uri($"{BaseUrl}/v1/electricity-meter-points/{mpan}/meters/{serialNumber}/consumption/")
                .AddQueryParam(interval)
                .AddQueryParam("page_size", PageSize)
                .AddQueryParam("period_from", from)
                .AddQueryParam("period_to", to);

            return await GetCollectionAsync<Consumption>(uri);
        }

        public async Task<IEnumerable<Consumption>> GetGasConsumptionAsync(string mprn, string serialNumber,
            DateTimeOffset from, DateTimeOffset to, Interval interval = Interval.Default)
        {
            Preconditions.IsNotNullOrWhiteSpace(mprn, nameof(mprn));
            Preconditions.IsNotNullOrWhiteSpace(serialNumber, nameof(serialNumber));

            var uri = new Uri($"{BaseUrl}/v1/gas-meter-points/{mprn}/meters/{serialNumber}/consumption/")
                .AddQueryParam(interval)
                .AddQueryParam("page_size", PageSize)
                .AddQueryParam("period_from", from)
                .AddQueryParam("period_to", to);

            return await GetCollectionAsync<Consumption>(uri);
        }
    }
}