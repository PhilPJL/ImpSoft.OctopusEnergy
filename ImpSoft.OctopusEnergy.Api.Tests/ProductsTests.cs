using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImpSoft.OctopusEnergy.Api.Tests;

[TestClass]
public class ProductsTests
{
    [TestMethod]
    public async Task GetProductByProductCodeSucceedsAsync()
    {
        const string code = "P123Z";

        var product = new ProductDetail
        {
            ActiveAt = DateTimeOffset.Now,
            AvailableFrom = DateTimeOffset.Now.AddHours(-10),
            AvailableTo = DateTimeOffset.Now.AddYears(1),
            Brand = "Brand",
            Code = code,
            Description = "Description",
            DisplayName = "Display name",
            FullName = "Full name",
            IsBusiness = true,
            IsGreen = true,
            IsPrepay = true,
            IsRestricted = true,
            IsTracker = true,
            IsVariable = true,
            Term = 5

            // DualRegisterElectricityTariffs
            // Links
            // SampleConsumption
            // SampleQuotes
            // SingleRegisterElectricityTariffs
            // SingleRegisterGasTariffs
        };

        var uri = OctopusEnergyClient.ComposeGetProductUri(OctopusEnergyClient.DefaultBaseAddress, code, null);

        var client = TestHelper.CreateClient(uri, product);

        var product1 = await client.GetProductAsync(code);

        // TODO? implement ProductDetail IEquatable?

        Assert.AreEqual(product.ActiveAt, product1.ActiveAt);
        Assert.AreEqual(product.AvailableFrom, product1.AvailableFrom);
        Assert.AreEqual(product.AvailableTo, product1.AvailableTo);
        Assert.AreEqual(product.Brand, product1.Brand);
        Assert.AreEqual(product.Code, product1.Code);
        Assert.AreEqual(product.Description, product1.Description);
        Assert.AreEqual(product.DisplayName, product1.DisplayName);
        Assert.AreEqual(product.FullName, product1.FullName);
        Assert.AreEqual(product.IsBusiness, product1.IsBusiness);
        Assert.AreEqual(product.IsGreen, product1.IsGreen);
        Assert.AreEqual(product.IsPrepay, product1.IsPrepay);
        Assert.AreEqual(product.IsRestricted, product1.IsRestricted);
        Assert.AreEqual(product.IsTracker, product1.IsTracker);
        Assert.AreEqual(product.IsVariable, product1.IsVariable);
        Assert.AreEqual(product.Term, product1.Term);
    }

    [TestMethod]
    public async Task GetProductsSucceedsAsync()
    {
        const string code = "P123Z";

        var products = new PagedResults<Product>
        {
            Results = new List<Product>{ 
                new()
                {
                    AvailableFrom = DateTimeOffset.Now.AddHours(-10),
                    AvailableTo = DateTimeOffset.Now.AddYears(1),
                    Brand = "Brand",
                    Code = code,
                    Description = "Description",
                    DisplayName = "Display name",
                    FullName = "Full name",
                    IsBusiness = true,
                    IsGreen = true,
                    IsPrepay = true,
                    IsRestricted = true,
                    IsTracker = true,
                    IsVariable = true,
                    Term = 5,
                    Direction = "Out"

                    // Links
                }
            },
            Count = 1,
            Next = string.Empty,
            Previous = string.Empty
        };

        var uri = OctopusEnergyClient.ComposeGetProductsUri(OctopusEnergyClient.DefaultBaseAddress, null, null, null, null, null, null);

        var client = TestHelper.CreateClient(uri, products);

        var products1 = await client.GetProductsAsync();

        var expectedProduct = products.Results.First();
        var actualProduct = products1.First();

        Assert.AreEqual(expectedProduct.AvailableFrom, actualProduct.AvailableFrom);
        Assert.AreEqual(expectedProduct.AvailableTo, actualProduct.AvailableTo);
        Assert.AreEqual(expectedProduct.Brand, actualProduct.Brand);
        Assert.AreEqual(expectedProduct.Code, actualProduct.Code);
        Assert.AreEqual(expectedProduct.Description, actualProduct.Description);
        Assert.AreEqual(expectedProduct.DisplayName, actualProduct.DisplayName);
        Assert.AreEqual(expectedProduct.FullName, actualProduct.FullName);
        Assert.AreEqual(expectedProduct.IsBusiness, actualProduct.IsBusiness);
        Assert.AreEqual(expectedProduct.IsGreen, actualProduct.IsGreen);
        Assert.AreEqual(expectedProduct.IsPrepay, actualProduct.IsPrepay);
        Assert.AreEqual(expectedProduct.IsRestricted, actualProduct.IsRestricted);
        Assert.AreEqual(expectedProduct.IsTracker, actualProduct.IsTracker);
        Assert.AreEqual(expectedProduct.IsVariable, actualProduct.IsVariable);
        Assert.AreEqual(expectedProduct.Term, actualProduct.Term);
    }

    [TestMethod]
    public void GetProductWithNullProductCodeThrows()
    {
        Assert.ThrowsException<ArgumentException>(() => OctopusEnergyClient.ComposeGetProductUri(OctopusEnergyClient.DefaultBaseAddress, string.Empty, null));
    }

    [TestMethod]
    public void GetProductWithEmptyProductCodeThrows()
    {
        Assert.ThrowsException<ArgumentException>(() => OctopusEnergyClient.ComposeGetProductUri(OctopusEnergyClient.DefaultBaseAddress, "    ", null));
    }
}