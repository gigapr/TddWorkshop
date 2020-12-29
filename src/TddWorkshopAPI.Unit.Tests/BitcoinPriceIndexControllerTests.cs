using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TddWorkshopAPI.Business;
using TddWorkshopAPI.Controllers;
using Xunit;

namespace TddWorkshopAPI.Unit.Tests
{
    public class BitcoinPriceIndexControllerTests
    {
        private readonly Mock<PriceIndexService> _priceIndexService;
        private readonly BitcoinPriceIndexController _controller;

        public BitcoinPriceIndexControllerTests()
        {
            _priceIndexService = new Mock<PriceIndexService>();
            _controller = new BitcoinPriceIndexController(_priceIndexService.Object);
        }
        
        [Fact]
        public async Task Controller_returns_price_index()
        {
            const string currency  = "EUR";
            var priceIndex = new PriceIndex{ Code = "EUR" };
            
            _priceIndexService.Setup(service => service.GetPriceIndex(currency))
                             .Returns(Task.FromResult(priceIndex));
            
            var response = await _controller.Get(currency);
            
            Assert.Equal(priceIndex, response.Value);
        }
        
        [Fact]
        public async Task Controller_returns_bad_request_for_invalid_currency()
        {
            const string currency  = "SomeInvalidCurrency";
            var errorMessage = string.Format($"'{currency}' is not a supported currency. Supported currencies are [EUR, USD, GBP]");
            
            _priceIndexService.Setup(service => service.GetPriceIndex(currency))
                              .Throws(new NotSupportedException(errorMessage));
            
            var response = await _controller.Get(currency);

            var result = response.Result as BadRequestObjectResult;
            
            Assert.NotNull(result);
            Assert.Equal((int?) HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal(errorMessage, result.Value);
        }
    }
}