using System.Threading.Tasks;

namespace Coindesk
{
    public interface CoindeskClient
    {
        Task<BitcoinPriceIndexResponse> GetBitcoinPriceIndex();
    }
}