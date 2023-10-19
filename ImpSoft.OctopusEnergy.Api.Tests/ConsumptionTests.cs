using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// ReSharper disable PossibleMultipleEnumeration

namespace ImpSoft.OctopusEnergy.Api.Tests;

[TestClass]
public class ConsumptionTests
{
    [TestMethod]
    public async Task GetElectricityConsumptionSucceedsAsync()
    {
        var from = DateTimeOffset.UtcNow.AddDays(-2);
        var to = DateTimeOffset.UtcNow.AddDays(-1);

        const string mpan = "A123Z";
        const string serialNumber = "J0123456K";
        const Interval interval = Interval.Hour;

        var consumption = new PagedResults<Consumption>
        {
            Results = new List<Consumption> {
                new()
                {
                    Start = from,
                    End = from.AddMinutes(30),
                    Quantity = 0.5m
                },
                new()
                {
                    Start = from.AddMinutes(30),
                    End = from.AddMinutes(60),
                    Quantity = 0.75m
                }
            },
            Count = 1,
            Next = string.Empty,
            Previous = string.Empty
        };

        var uri = OctopusEnergyClient.ComposeGetElectricityConsumptionUri(OctopusEnergyClient.DefaultBaseAddress, mpan, serialNumber, from, to, interval);

        var client = TestHelper.CreateClient(uri, consumption);

        var consumption1 = await client.GetElectricityConsumptionAsync(mpan, serialNumber, from, to, Interval.Hour);

        Assert.AreEqual(consumption.Results.Count(), consumption1.Count());

        var firstExpected = consumption.Results.First();
         var firstActual = consumption1.First();

        Assert.AreEqual(firstExpected.Start, firstActual.Start);
        Assert.AreEqual(firstExpected.End, firstActual.End);
        Assert.AreEqual(firstExpected.Quantity, firstActual.Quantity);
    }

    [TestMethod]
    public void GetElectricityConsumptionWithNoMpanCodeThrows()
    {
        var from = DateTimeOffset.UtcNow.AddDays(-2);
        var to = DateTimeOffset.UtcNow.AddDays(-1);

        const string mpan = "";
        const string serialNumber = "J0123456K";
        const Interval interval = Interval.Hour;

        Assert.ThrowsException<ArgumentException>(() => OctopusEnergyClient.ComposeGetElectricityConsumptionUri(
            OctopusEnergyClient.DefaultBaseAddress, mpan, serialNumber, from, to, interval));
    }

    [TestMethod]
    public void GetElectricityConsumptionWithNoSerialThrows()
    {
        var from = DateTimeOffset.UtcNow.AddDays(-2);
        var to = DateTimeOffset.UtcNow.AddDays(-1);

        const string mpan = "A123Z";
        const string serialNumber = "";
        const Interval interval = Interval.Hour;

        Assert.ThrowsException<ArgumentException>(() => OctopusEnergyClient.ComposeGetElectricityConsumptionUri(
            OctopusEnergyClient.DefaultBaseAddress, mpan, serialNumber, from, to, interval));
    }

    [TestMethod]
    public async Task GetGasConsumptionSucceedsAsync()
    {
        var from = DateTimeOffset.UtcNow.AddDays(-2);
        var to = DateTimeOffset.UtcNow.AddDays(-1);

        const string mprn = "A123Z";
        const string serialNumber = "J0123456K";
        const Interval interval = Interval.Hour;

        var consumption = new PagedResults<Consumption>
        {
            Results = new List<Consumption> {
                new()
                {
                    Start = from,
                    End = from.AddMinutes(30),
                    Quantity = 0.5m
                },
                new()
                {
                    Start = from.AddMinutes(30),
                    End = from.AddMinutes(60),
                    Quantity = 0.75m
                }
            },
            Count = 1,
            Next = string.Empty,
            Previous = string.Empty
        };

        var uri = OctopusEnergyClient.ComposeGetGasConsumptionUri(OctopusEnergyClient.DefaultBaseAddress, mprn, serialNumber, from, to, interval);

        var client = TestHelper.CreateClient(uri, consumption);

        var consumption1 = await client.GetGasConsumptionAsync( mprn, serialNumber, from, to, Interval.Hour);

        Assert.AreEqual(consumption.Results.Count(), consumption1.Count());

        var firstExpected = consumption.Results.First();
        var firstActual = consumption1.First();

        Assert.AreEqual(firstExpected.Start, firstActual.Start);
        Assert.AreEqual(firstExpected.End, firstActual.End);
        Assert.AreEqual(firstExpected.Quantity, firstActual.Quantity);
    }
}