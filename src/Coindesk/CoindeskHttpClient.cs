using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Coindesk
{
    public class CoindeskHttpClient : CoindeskClient
    {
        private readonly HttpClient _httpClient;    
    
        public CoindeskHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
    
        public async Task<BitcoinPriceIndexResponse> GetBitcoinPriceIndex()
        {
            var responseString = await _httpClient.GetStringAsync("bpi/currentprice.json");
    
            var bitcoinPriceIndex = JsonConvert.DeserializeObject<BitcoinPriceIndexResponse>(responseString);
            
            return bitcoinPriceIndex;
        }
    }
}