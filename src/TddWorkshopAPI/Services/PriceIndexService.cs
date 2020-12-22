using System.Threading.Tasks;
using TddWorkshopAPI.Domain;

namespace TddWorkshopAPI.Services
{
    public interface PriceIndexService
    {
        Task<PriceIndex> GetPriceIndex(string currency);
    }
}