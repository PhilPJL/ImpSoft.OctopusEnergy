using Microsoft.VisualStudio.TestTools.UnitTesting;
// ReSharper disable PossibleMultipleEnumeration

namespace ImpSoft.OctopusEnergy.Api.Tests;

[TestClass]
public class GspTests
{
    [TestMethod]
    public void IsValidGsp()
    {
        for(char c = 'A'; c < 'O'; c++)
        {
            Assertions.AssertValidGsp($"_{c}");
        }
    }

    [TestMethod]
    public void IsInvalidGsp()
    {
        Assert.ThrowsException<GspException>(() => Assertions.AssertValidGsp($"_O"), "The GSP '_O' is not in the range _A to _N");
        Assert.ThrowsException<GspException>(() => Assertions.AssertValidGsp($"_a"), "The GSP '_a' is not in the range _A to _N");
        Assert.ThrowsException<GspException>(() => Assertions.AssertValidGsp($"A"), "The GSP 'A' is not in the range _A to _N");
        Assert.ThrowsException<GspException>(() => Assertions.AssertValidGsp($"O"), "The GSP 'O' is not in the range _A to _N");
    }
}
