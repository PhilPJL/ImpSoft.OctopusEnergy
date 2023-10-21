using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// ReSharper disable PossibleMultipleEnumeration

namespace ImpSoft.OctopusEnergy.Api.Tests;

[TestClass]
public class UnitRateTests
{
    [TestMethod]
    public async Task GetElectricityUnitRatesSucceedsAsync()
    {
        var from = DateTimeOffset.UtcNow.AddDays(-2);
        var to = DateTimeOffset.UtcNow.AddDays(-1);

        const string productCode = "A123Z";
        const string tariffCode = "J0123456K";
        const ElectricityUnitRate rate = ElectricityUnitRate.Standard;

        var charges = new PagedResults<Charge>
        {
            Results = new List<Charge>
            {
                new()
                {
                    ValidFrom = from,
                    ValidTo = to,
                    ValueExcludingVAT = 20m,
                    ValueIncludingVAT = 20m * 1.2m
                },
                new()
                {
                    ValidFrom = from.AddDays(1),
                    ValidTo = to.AddDays(1),
                    ValueExcludingVAT = 30m,
                    ValueIncludingVAT = 30m * 1.2m
                }
            },
            Count = 1,
            Next = string.Empty,
            Previous = string.Empty
        };

        var uri = OctopusEnergyClient.ComposeGetElectricityUnitRatesUri(OctopusEnergyClient.DefaultBaseAddress,
            productCode, tariffCode, rate, from, to);

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

        const string productCode = "A123Z";
        const string tariffCode = "J0123456K";

        var charges = new PagedResults<Charge>
        {
            Results = new List<Charge>
            {
                new()
                {
                    ValidFrom = from,
                    ValidTo = to,
                    ValueExcludingVAT = 20m,
                    ValueIncludingVAT = 20m * 1.2m
                },
                new()
                {
                    ValidFrom = from.AddDays(1),
                    ValidTo = to.AddDays(1),
                    ValueExcludingVAT = 30m,
                    ValueIncludingVAT = 30m * 1.2m
                }
            },
            Count = 1,
            Next = string.Empty,
            Previous = string.Empty
        };

        var uri = OctopusEnergyClient.ComposeGetGasUnitRatesUri(OctopusEnergyClient.DefaultBaseAddress, productCode,
            tariffCode, from, to);

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