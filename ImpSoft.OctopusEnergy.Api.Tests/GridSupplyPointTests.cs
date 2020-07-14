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
            var uri = OctopusEnergyClient.ComposeGetGridSupplyPointByMpanUri("123456789");

            var client = TestHelper.CreateClient(uri, new MeterPointGridSupplyPoint { GroupId = "_A" });

            Assert.AreEqual("_A", await client.GetGridSupplyPointByMpanAsync("123456789"));
        }

        [TestMethod]
        public async Task GetGridSupplyPointByMpanThrowsAsync()
        {
            var uri = OctopusEnergyClient.ComposeGetGridSupplyPointByMpanUri("123456789X");

            var client = TestHelper.CreateClient(uri, new MeterPointGridSupplyPoint { GroupId = "_A" });

            await Assert.ThrowsExceptionAsync<HttpRequestException>(async () => await client.GetGridSupplyPointByMpanAsync("123456789"));
        }


        [TestMethod]
        public async Task GetGridSupplyPointByPostcodeSucceedsAsync()
        {
            var uri = OctopusEnergyClient.ComposeGetGridSupplyPointByPostcodeUri("AB12 3XY");

            var client = TestHelper.CreateClient(uri, new PagedResults<GridSupplyPoint>
            {
                Count = 1,
                Results = new List<GridSupplyPoint> { new GridSupplyPoint { GroupId = "_A" } }
            });

            Assert.AreEqual("_A", await client.GetGridSupplyPointByPostcodeAsync("AB12 3XY"));
        }

        [TestMethod]
        public async Task GetGridSupplyPointByPostcodeNoResultsThrowsAsync()
        {
            var uri = OctopusEnergyClient.ComposeGetGridSupplyPointByPostcodeUri("AB12 3XY");

            var client = TestHelper.CreateClient(uri, new PagedResults<GridSupplyPoint>
            {
                Count = 1,
                Results = new List<GridSupplyPoint>()
            });

            await Assert.ThrowsExceptionAsync<GspException>(async () => await client.GetGridSupplyPointByPostcodeAsync("AB12 3XY"));
        }

        [TestMethod]
        public async Task GetGridSupplyPointByPostcodeInvalidGspThrowsAsync()
        {
            var uri = OctopusEnergyClient.ComposeGetGridSupplyPointByPostcodeUri("AB12 3XY");

            var client = TestHelper.CreateClient(uri, new PagedResults<GridSupplyPoint>
            {
                Count = 1,
                Results = new List<GridSupplyPoint> { new GridSupplyPoint { GroupId = "Z" } }
            });

            await Assert.ThrowsExceptionAsync<GspException>(async () => await client.GetGridSupplyPointByPostcodeAsync("AB12 3XY"));
        }

        [TestMethod]
        public async Task GetGridSupplyPointByPostcodeMoreThanOneResultThrowsAsync()
        {
            var uri = OctopusEnergyClient.ComposeGetGridSupplyPointByPostcodeUri("AB12 3XY");

            var client = TestHelper.CreateClient(uri, new PagedResults<GridSupplyPoint>
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
