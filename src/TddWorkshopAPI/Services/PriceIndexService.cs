using System.Threading.Tasks;
using TddWorkshopAPI.Domain;

namespace TddWorkshopAPI.Services
{
    public interface PriceIndexService
    {
        Task<PriceIndexResponse> GetPriceIndex(string currency);
    }
}