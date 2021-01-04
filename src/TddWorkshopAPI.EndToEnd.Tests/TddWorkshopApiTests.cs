using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Coindesk;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Xunit;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TddWorkshopAPI.Domain;
using Xunit;
using Xunit.Abstractions;

namespace TddWorkshopAPI.EndToEnd.Tests
{
    public class TddWorkshopApiTests : IDisposable
    {
        private readonly StubCoindesk _stubCoindesk;
        private readonly IHostBuilder _hostBuilder;

        public TddWorkshopApiTests(ITestOutputHelper testOutputHelper)
        {
            var loggerFactory = new LoggerFactory();
            
            loggerFactory.AddProvider(new XunitLoggerProvider(testOutputHelper));
            
            var logger = loggerFactory.CreateLogger<TddWorkshopApiTests>();
            
            _stubCoindesk = new StubCoindesk(logger);
            
            _hostBuilder = new HostBuilder().ConfigureWebHost(webHost =>
            {
                webHost.UseTestServer();
                webHost.ConfigureAppConfiguration((_, configuration) =>
                    configuration.AddInMemoryCollection(
                        new Dictionary<string, string>
                        {
                            ["coindeskAPiURL"] = $"http://localhost:{_stubCoindesk.Port}/"
                        }));
                webHost.UseStartup<Startup>();
            });
        }
        
        [Fact]
        public async Task CanGETBitCoinPriceIndex()
        {
            const string currency = "EUR";
            
            var coindeskResponse = ValidResponse();
            
            _stubCoindesk.OnRequestReturns(coindeskResponse);
            
            var host = await _hostBuilder.StartAsync();

            var client = host.GetTestClient();

            var response = await client.GetAsync($"/BitcoinPriceIndex?currency={currency}");

            var responseString = await response.Content.ReadAsStringAsync();
            
            var priceIndexResponse = JsonConvert.DeserializeObject<PriceIndexResponse>(responseString);

            Assert.Equal(currency, priceIndexResponse.Code);
            Assert.Equal(coindeskResponse.BitcoinPriceIndexes.EUR.Description, priceIndexResponse.Description);
            Assert.Equal(coindeskResponse.BitcoinPriceIndexes.EUR.Rate, priceIndexResponse.Rate);
            Assert.Equal(coindeskResponse.BitcoinPriceIndexes.EUR.Symbol, priceIndexResponse.Symbol);
            Assert.Equal(coindeskResponse.Time.Updated, priceIndexResponse.Updated);
            Assert.Equal(coindeskResponse.Time.UpdatedUK, priceIndexResponse.UpdatedUK);
            Assert.Equal(coindeskResponse.Time.UpdatedISO, priceIndexResponse.UpdatedISO);
        }
        
        public void Dispose()
        {
            _stubCoindesk?.Dispose();
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