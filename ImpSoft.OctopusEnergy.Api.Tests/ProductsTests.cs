using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace ImpSoft.OctopusEnergy.Api.Tests
{
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
                Term = 5,

                // DualRegisterElectricityTariffs
                // Links
                // SampleConsumption
                // SampleQuotes
                // SingleRegisterElectricityTariffs
                // SingleRegisterGasTariffs
            };

            var client = TestHelper.CreateClient($"https://api.octopus.energy/v1/products/{code}/", product);

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
    }
}
