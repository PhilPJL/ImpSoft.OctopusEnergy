﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// ReSharper disable PossibleMultipleEnumeration

namespace ImpSoft.OctopusEnergy.Api.Tests;

[TestClass]
public class StandingChargesTests
{
    [TestMethod]
    public async Task GetElectricityStandingChargesSucceedsAsync()
    {
        var from = DateTimeOffset.UtcNow.AddDays(-2);
        var to = DateTimeOffset.UtcNow.AddDays(-1);

        const string productCode = "A123Z";
        const string tariffCode = "J0123456K";

        var charges = new PagedResults<Charge>
        {
            Results =
            [
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
            ],
            Count = 1,
            Next = string.Empty,
            Previous = string.Empty
        };

        var uri = OctopusEnergyClient.ComposeGetElectricityStandingChargesUri(OctopusEnergyClient.DefaultBaseAddress,
            productCode, tariffCode, from, to);

        var client = TestHelper.CreateClient(uri, charges);

        var charges1 = await client.GetElectricityStandingChargesAsync(productCode, tariffCode, from, to);

        Assert.AreEqual(charges.Results.Count, charges1.Count());

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

        const string productCode = "A123Z";
        const string tariffCode = "J0123456K";

        var charges = new PagedResults<Charge>
        {
            Results =
            [
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
            ],
            Count = 1,
            Next = string.Empty,
            Previous = string.Empty
        };

        var uri = OctopusEnergyClient.ComposeGetGasStandingChargesUri(OctopusEnergyClient.DefaultBaseAddress,
            productCode, tariffCode, from, to);

        var client = TestHelper.CreateClient(uri, charges);

        var charges1 = await client.GetGasStandingChargesAsync(productCode, tariffCode, from, to);

        Assert.AreEqual(charges.Results.Count, charges1.Count());

        var firstExpected = charges.Results.First();
        var firstActual = charges1.First();

        Assert.AreEqual(firstExpected.ValidFrom, firstActual.ValidFrom);
        Assert.AreEqual(firstExpected.ValidTo, firstActual.ValidTo);
        Assert.AreEqual(firstExpected.ValueIncludingVAT, firstActual.ValueIncludingVAT);
        Assert.AreEqual(firstExpected.ValueExcludingVAT, firstActual.ValueExcludingVAT);
    }
}