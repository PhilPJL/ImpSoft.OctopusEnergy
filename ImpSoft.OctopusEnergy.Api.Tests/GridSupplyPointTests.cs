using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ImpSoft.OctopusEnergy.Api.Tests
{
    [TestClass]
    public class GridSupplyPointTests
    {
        [TestMethod]
        public async Task GetGridSupplyPointByMpanSucceedsAsync()
        {
            var client = TestHelper.CreateClient("https://api.octopus.energy/v1/electricity-meter-points/123456789/", new MeterPointGridSupplyPoint { GroupId = "_A" });

            Assert.AreEqual("_A", await client.GetGridSupplyPointByMpanAsync("123456789"));
        }

        [TestMethod]
        public async Task GetGridSupplyPointByMpanThrowsAsync()
        {
            var client = TestHelper.CreateClient("https://api.octopus.energy/v1/electricity-meter-points/X123456789X/", new MeterPointGridSupplyPoint { GroupId = "_A" });

            await Assert.ThrowsExceptionAsync<HttpRequestException>(async () => await client.GetGridSupplyPointByMpanAsync("123456789"));
        }


        [TestMethod]
        public async Task GetGridSupplyPointByPostcodeSucceedsAsync()
        {
            var client = TestHelper.CreateClient("https://api.octopus.energy/v1/industry/grid-supply-points/?postcode=AB12 3XY", new PagedResults<GridSupplyPoint>
            {
                Count = 1,
                Results = new List<GridSupplyPoint> { new GridSupplyPoint { GroupId = "_A" } }
            });

            Assert.AreEqual("_A", await client.GetGridSupplyPointByPostcodeAsync("AB12 3XY"));
        }

        [TestMethod]
        public async Task GetGridSupplyPointByPostcodeNoResultsThrowsAsync()
        {
            var client = TestHelper.CreateClient("https://api.octopus.energy/v1/industry/grid-supply-points/?postcode=AB12 3XY", new PagedResults<GridSupplyPoint>
            {
                Count = 1,
                Results = new List<GridSupplyPoint>()
            });

            await Assert.ThrowsExceptionAsync<GspException>(async () => await client.GetGridSupplyPointByPostcodeAsync("AB12 3XY"));
        }

        [TestMethod]
        public async Task GetGridSupplyPointByPostcodeInvalidGspThrowsAsync()
        {
            var client = TestHelper.CreateClient("https://api.octopus.energy/v1/industry/grid-supply-points/?postcode=AB12 3XY", new PagedResults<GridSupplyPoint>
            {
                Count = 1,
                Results = new List<GridSupplyPoint> { new GridSupplyPoint { GroupId = "Z" } }
            });

            await Assert.ThrowsExceptionAsync<GspException>(async () => await client.GetGridSupplyPointByPostcodeAsync("AB12 3XY"));
        }

        [TestMethod]
        public async Task GetGridSupplyPointByPostcodeMoreThanOneResultThrowsAsync()
        {
            var client = TestHelper.CreateClient("https://api.octopus.energy/v1/industry/grid-supply-points/?postcode=AB12 3XY", new PagedResults<GridSupplyPoint>
            {
                Count = 2,
                Results = new List<GridSupplyPoint> { 
                    new GridSupplyPoint { GroupId = "_A" } ,
                    new GridSupplyPoint { GroupId = "_B" } ,
                }
            });

            await Assert.ThrowsExceptionAsync<GspException>(async () => await client.GetGridSupplyPointByPostcodeAsync("AB12 3XY"));
        }
    }
}
