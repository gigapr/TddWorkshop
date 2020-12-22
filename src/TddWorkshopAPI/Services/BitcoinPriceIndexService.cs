using System;
using System.Threading.Tasks;
using Coindesk;
using PriceIndex = TddWorkshopAPI.Domain.PriceIndex;

namespace TddWorkshopAPI.Services
{
    public class BitcoinPriceIndexService : PriceIndexService
    {
        private readonly CoindeskClient _coindeskClient;

        public BitcoinPriceIndexService(CoindeskClient coindeskClient)
        {
            _coindeskClient = coindeskClient;
        }
        
        public async Task<PriceIndex> GetPriceIndex(string currency)
        {
            var priceIndex = await _coindeskClient.GetBitcoinPriceIndex();

            var t = currency.ToLower() switch
            {
                "eur" => priceIndex.BitcoinPriceIndexes.EUR,
                "usd" => priceIndex.BitcoinPriceIndexes.USD,
                "gbp" => priceIndex.BitcoinPriceIndexes.GBP,
                _ => throw new NotSupportedException(string.Format( $"'{currency}' is not a supported currency. Supported currencies are [EUR, USD, GBP]"))
            };

            return new PriceIndex
            {
               Updated = priceIndex.Time.Updated,
               UpdatedISO = priceIndex.Time.UpdatedISO,
               UpdatedUK= priceIndex.Time.UpdatedUK,
               Code =  t.Code,
               Symbol =  t.Symbol,
               Rate = t.Rate,
               Description = t.Description
            };
        }
    }
}