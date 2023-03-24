using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImpSoft.OctopusEnergy.Api.Tests
{
    [TestClass]
    public class UnitRateTests
    {
        [TestMethod]
        public async Task GetElectricityUnitRatesSucceedsAsync()
        {
            var from = DateTimeOffset.UtcNow.AddDays(-2);
            var to = DateTimeOffset.UtcNow.AddDays(-1);

            var productCode = "A123Z";
            var tariffCode = "J0123456K";
            var rate = ElectricityUnitRate.Standard;

            var charges = new PagedResults<Charge>
            {
                Results = new List<Charge> {
                    new Charge {
                        ValidFrom = from,
                        ValidTo = to,
                        ValueExcludingVAT = 20m,
                        ValueIncludingVAT = 20m*1.2m
                    },
                    new Charge {
                        ValidFrom = from.AddDays(1),
                        ValidTo = to.AddDays(1),
                        ValueExcludingVAT = 30m,
                        ValueIncludingVAT = 30m*1.2m
                    },
                },
                Count = 1,
                Next = null,
                Previous = null
            };

            var uri = OctopusEnergyClient.ComposeGetElectricityUnitRatesUri(productCode, tariffCode, rate, from, to);

            var client = TestHelper.CreateClient(uri, charges);

            var charges1 = await client.GetElectricityUnitRatesAsync(productCode, tariffCode, from, to, rate);

            Assert.AreEqual(charges.Results.Count(), charges1.Count());

            var firstExpected = charges.Results.First();
            var firstActual = charges1.First();

            Assert.AreEqual(firstExpected.ValidFrom, firstActual.ValidFrom);
            Assert.AreEqual(firstExpected.ValidTo, firstActual.ValidTo);
            Assert.AreEqual(firstExpected.ValueIncludingVAT, firstActual.ValueIncludingVAT);
            Assert.AreEqual(firstExpected.ValueExcludingVAT, firstActual.ValueExcludingVAT);
        }

        [TestMethod]
        public async Task GetGasUnitRatesSucceedsAsync()
        {
            var from = DateTimeOffset.UtcNow.AddDays(-2);
            var to = DateTimeOffset.UtcNow.AddDays(-1);

            var productCode = "A123Z";
            var tariffCode = "J0123456K";

            var charges = new PagedResults<Charge>
            {
                Results = new List<Charge> {
                    new Charge {
                        ValidFrom = from,
                        ValidTo = to,
                        ValueExcludingVAT = 20m,
                        ValueIncludingVAT = 20m*1.2m
                    },
                    new Charge {
                        ValidFrom = from.AddDays(1),
                        ValidTo = to.AddDays(1),
                        ValueExcludingVAT = 30m,
                        ValueIncludingVAT = 30m*1.2m
                    },
                },
                Count = 1,
                Next = null,
                Previous = null
            };

            var uri = OctopusEnergyClient.ComposeGetGasUnitRatesUri(productCode, tariffCode, from, to);

            var client = TestHelper.CreateClient(uri, charges);

            var charges1 = await client.GetGasUnitRatesAsync(productCode, tariffCode, from, to);

            Assert.AreEqual(charges.Results.Count(), charges1.Count());

            var firstExpected = charges.Results.First();
            var firstActual = charges1.First();

            Assert.AreEqual(firstExpected.ValidFrom, firstActual.ValidFrom);
            Assert.AreEqual(firstExpected.ValidTo, firstActual.ValidTo);
            Assert.AreEqual(firstExpected.ValueIncludingVAT, firstActual.ValueIncludingVAT);
            Assert.AreEqual(firstExpected.ValueExcludingVAT, firstActual.ValueExcludingVAT);
        }

    }
}
