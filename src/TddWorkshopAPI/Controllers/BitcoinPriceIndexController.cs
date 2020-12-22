using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TddWorkshopAPI.Business;

namespace TddWorkshopAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BitcoinPriceIndexController : ControllerBase
    {
        private readonly PriceIndexService _priceIndexService;

        public BitcoinPriceIndexController(PriceIndexService priceIndexService)
        {
            _priceIndexService = priceIndexService;
        }

        [HttpGet]
        public async Task<ActionResult<PriceIndex>> Get([Required]string currency)
        {
            try
            {
                return await _priceIndexService.GetPriceIndex(currency);
            }
            catch (NotSupportedException exception)
            {
                return BadRequest(exception.Message);
            }
        }
    }
}
