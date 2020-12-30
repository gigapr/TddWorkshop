using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Coindesk.Integration.Tests
{
    public class CoindeskHttpClientTests
    {
        [Fact]
        public async Task CanRequestBitcoinPriceIndex()
        {
            var httpClient = new HttpClient { BaseAddress = new Uri("https://api.coindesk.com/v1/") };

            var client = new CoindeskHttpClient(httpClient);

            var bitcoinPriceIndex = await client.GetBitcoinPriceIndex();
            
            Assert.NotNull(bitcoinPriceIndex);
            Assert.NotNull(bitcoinPriceIndex.Time);
            Assert.NotNull(bitcoinPriceIndex.BitcoinPriceIndexes);
        }
    }
}