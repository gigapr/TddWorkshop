using System.Threading.Tasks;

namespace TddWorkshopAPI.Business
{
    public interface PriceIndexService
    {
        Task<PriceIndex> GetPriceIndex(string currency);
    }
}