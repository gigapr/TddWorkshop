using System;
using System.Threading.Tasks;
using Coindesk;
using Moq;
using Xunit;

namespace TddWorkshopAPI.Business.Unit.Tests
{
    public class BitcoinPriceIndexServiceTests
    {
        private readonly Mock<CoindeskClient> _coindeskClient;
        private readonly BitcoinPriceIndexService _service;

        public BitcoinPriceIndexServiceTests()
        {
            _coindeskClient = new Mock<CoindeskClient>();
            _service = new BitcoinPriceIndexService(_coindeskClient.Object);
        }
        
        [Theory]
        [InlineData("EUR")]
        [InlineData("eur")]
        [InlineData("GbP")]
        [InlineData("UsD")]
        public async Task CanGetPriceIndexForValidCurrency(string currency)
        {
            var response = ValidResponse();
            
            _coindeskClient.Setup(client => client.GetBitcoinPriceIndex())
                           .ReturnsAsync(response);

            var result = await _service.GetPriceIndex(currency);
            
            Assert.NotNull(result);
        }

        [Fact]
        public async Task ReturnDataForEuro()
        {
            var response = ValidResponse();
            
            _coindeskClient.Setup(client => client.GetBitcoinPriceIndex())
                            .ReturnsAsync(response);
            
            var result = await _service.GetPriceIndex("EUR");
            
            Assert.Equal(response.Time.Updated, result.Updated);
            Assert.Equal(response.Time.UpdatedUK, result.UpdatedUK);
            Assert.Equal(response.Time.UpdatedISO, result.UpdatedISO);
            Assert.Equal(response.BitcoinPriceIndexes.EUR.Code, result.Code);
            Assert.Equal(response.BitcoinPriceIndexes.EUR.Description, result.Description);
            Assert.Equal(response.BitcoinPriceIndexes.EUR.Rate, result.Rate);
            Assert.Equal(response.BitcoinPriceIndexes.EUR.Symbol, result.Symbol);
        }
        
        [Fact]
        public async Task ReturnDataForUSDollar()
        {
            var response = ValidResponse();
            
            _coindeskClient.Setup(client => client.GetBitcoinPriceIndex())
                .ReturnsAsync(response);
            
            var result = await _service.GetPriceIndex("USD");
            
            Assert.Equal(response.Time.Updated, result.Updated);
            Assert.Equal(response.Time.UpdatedUK, result.UpdatedUK);
            Assert.Equal(response.Time.UpdatedISO, result.UpdatedISO);
            Assert.Equal(response.BitcoinPriceIndexes.USD.Code, result.Code);
            Assert.Equal(response.BitcoinPriceIndexes.USD.Description, result.Description);
            Assert.Equal(response.BitcoinPriceIndexes.USD.Rate, result.Rate);
            Assert.Equal(response.BitcoinPriceIndexes.USD.Symbol, result.Symbol);
        }
        
        [Fact]
        public async Task ReturnDataForBritishPoundSterling()
        {
            var response = ValidResponse();
            
            _coindeskClient.Setup(client => client.GetBitcoinPriceIndex())
                .ReturnsAsync(response);
            
            var result = await _service.GetPriceIndex("gbp");
            
            Assert.Equal(response.Time.Updated, result.Updated);
            Assert.Equal(response.Time.UpdatedUK, result.UpdatedUK);
            Assert.Equal(response.Time.UpdatedISO, result.UpdatedISO);
            Assert.Equal(response.BitcoinPriceIndexes.GBP.Code, result.Code);
            Assert.Equal(response.BitcoinPriceIndexes.GBP.Description, result.Description);
            Assert.Equal(response.BitcoinPriceIndexes.GBP.Rate, result.Rate);
            Assert.Equal(response.BitcoinPriceIndexes.GBP.Symbol, result.Symbol);
        }
        
        [Fact]
        public async Task ThrowsForNotSupportedException()
        {
            var response = ValidResponse();
            
            _coindeskClient.Setup(client => client.GetBitcoinPriceIndex())
                           .ReturnsAsync(response);

            await Assert.ThrowsAsync<NotSupportedException>(async () => await _service.GetPriceIndex("InvalidCurrency"));
        }

        private static BitcoinPriceIndexResponse ValidResponse()
        {
            return new BitcoinPriceIndexResponse
            {
                Time = new PriceIndexTime
                {
                    Updated = "Dec 28, 2020 23:12:00 UTC",
                    UpdatedISO = "2020-12-28T23:12:00+00:00",
                    UpdatedUK = "Dec 28, 2020 at 23:12 GMT"
                },
                BitcoinPriceIndexes = new BitcoinPriceIndexs
                {
                    USD = new BitcoinPriceIndex
                    {
                        Code = "USD",
                        Symbol= "&#36;",
                        Rate= "26,962.1100",
                        Description= "United States Dollar",
                    },
                    GBP = new BitcoinPriceIndex 
                    {
                        Code = "GBP",
                        Symbol= "&pound;",
                        Rate= "20,041.5835",
                        Description= "British Pound Sterling",
                    },
                    EUR = new BitcoinPriceIndex
                    {
                        Code = "EUR",
                        Symbol= "&euro;",
                        Rate= "22,083.6397",
                        Description= "Euro",
                    }
                }
            };
        }
    }
}