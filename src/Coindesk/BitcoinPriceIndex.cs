using Newtonsoft.Json;

namespace Coindesk
{
    public class BitcoinPriceIndexResponse
    {
        [JsonProperty(PropertyName = "time")]
        public PriceIndexTime Time { get; set; }
        
        [JsonProperty(PropertyName = "bpi")]
        public BitcoinPriceIndexs BitcoinPriceIndexes { get; set; }
    }

    public class PriceIndexTime
    {
        public string Updated { get; set; }
        public string UpdatedISO { get; set; }
        public string UpdatedUK { get; set; }
    }

    public class BitcoinPriceIndexs
    {
        public BitcoinPriceIndex USD { get; set; }
        public BitcoinPriceIndex GBP { get; set; }
        public BitcoinPriceIndex EUR { get; set; }
    }

    public class BitcoinPriceIndex
    {
        public string Code { get; set; }
        public string Symbol { get; set; }
        public string Rate { get; set; }
        public string Description { get; set; }
    }
}