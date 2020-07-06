using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ImpSoft.OctopusEnergy.Api.Tests
{
    [TestClass]
    public class Tests
    {
        IOctopusEnergyClient CreateClient<TResponse>(string expectedUri, TResponse response) where TResponse : class
        {
            // Do I care about disposing these?
            var httpClient = new HttpClient(new FakeHttpMessageHandler<TResponse>(new Uri(expectedUri), response));

            return new OctopusEnergyClient(httpClient);
        }

        IOctopusEnergyClient CreateClient(string expectedUri, string response)
        {
            // Do I care about disposing these?
            var httpClient = new HttpClient(new FakeHttpMessageHandler<string>(new Uri(expectedUri), response));

            return new OctopusEnergyClient(httpClient);
        }

        [TestMethod]
        public async Task GetGridSupplyPointByMpanSucceedsAsync()
        {
            var client = CreateClient("https://api.octopus.energy/v1/electricity-meter-points/123456789/", new MeterPointGridSupplyPoint { GroupId = "A" });

            Assert.AreEqual("A", await client.GetGridSupplyPointByMpanAsync("123456789"));
        }

        [TestMethod]
        public async Task GetGridSupplyPointByMpanThrowsAsync()
        {
            var client = CreateClient("https://api.octopus.energy/v1/electricity-meter-points/X123456789X/", new MeterPointGridSupplyPoint { GroupId = "A" });

            await Assert.ThrowsExceptionAsync<UriGetException>(async () => await client.GetGridSupplyPointByMpanAsync("123456789"));
        }

        [TestMethod]
        public async Task GetGridSupplyPointByPostcodeSucceedsAsync()
        {
            var client = CreateClient("https://api.octopus.energy/v1/industry/grid-supply-points/?postcode=AB12 3XY", new PagedResults<GridSupplyPoint>
            {
                Count = 1,
                Results = new List<GridSupplyPoint> { new GridSupplyPoint { GroupId = "A" } }
            });

            Assert.AreEqual("A", await client.GetGridSupplyPointByPostcodeAsync("AB12 3XY"));
        }

        [TestMethod]
        public async Task GetGridSupplyPointByPostcodeNoResultsThrowsAsync()
        {
            var client = CreateClient("https://api.octopus.energy/v1/industry/grid-supply-points/?postcode=AB12 3XY", new PagedResults<GridSupplyPoint>
            {
                Count = 1,
                Results = new List<GridSupplyPoint>()
            });

            await Assert.ThrowsExceptionAsync<GspException>(async () => await client.GetGridSupplyPointByPostcodeAsync("AB12 3XY"));
        }

        [TestMethod]
        public async Task GetGridSupplyPointByPostcodeMoreThanOneResultThrowsAsync()
        {
            var client = CreateClient("https://api.octopus.energy/v1/industry/grid-supply-points/?postcode=AB12 3XY", new PagedResults<GridSupplyPoint>
            {
                Count = 2,
                Results = new List<GridSupplyPoint> { 
                    new GridSupplyPoint { GroupId = "A" } ,
                    new GridSupplyPoint { GroupId = "B" } ,
                }
            });

            await Assert.ThrowsExceptionAsync<GspException>(async () => await client.GetGridSupplyPointByPostcodeAsync("AB12 3XY"));
        }
    }
}
