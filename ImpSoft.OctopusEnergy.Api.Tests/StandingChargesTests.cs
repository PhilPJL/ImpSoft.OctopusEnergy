using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImpSoft.OctopusEnergy.Api.Tests
{
    [TestClass]
    public class StandingChargesTests
    {
        [TestMethod]
        public async Task GetElectricityStandingChargesSucceedsAsync()
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

            var uri = OctopusEnergyClient.ComposeGetElectricityStandingChargesUri(productCode, tariffCode, from, to);

            var client = TestHelper.CreateClient(uri, charges);

            var charges1 = await client.GetElectricityStandingChargesAsync(productCode, tariffCode, from, to);

            Assert.AreEqual(charges.Results.Count(), charges1.Count());

            var firstExpected = charges.Results.First();
            var firstActual = charges1.First();

            Assert.AreEqual(firstExpected.ValidFrom, firstActual.ValidFrom);
            Assert.AreEqual(firstExpected.ValidFromUTC, firstActual.ValidFromUTC);
            Assert.AreEqual(firstExpected.ValidTo, firstActual.ValidTo);
            Assert.AreEqual(firstExpected.ValidToUTC, firstActual.ValidToUTC);
            Assert.AreEqual(firstExpected.ValueIncludingVAT, firstActual.ValueIncludingVAT);
            Assert.AreEqual(firstExpected.ValueExcludingVAT, firstActual.ValueExcludingVAT);
        }

        [TestMethod]
        public async Task GetGasStandingChargesSucceedsAsync()
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

            var uri = OctopusEnergyClient.ComposeGetGasStandingChargesUri(productCode, tariffCode, from, to);

            var client = TestHelper.CreateClient(uri, charges);

            var charges1 = await client.GetGasStandingChargesAsync(productCode, tariffCode, from, to);

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
