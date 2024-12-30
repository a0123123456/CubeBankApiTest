using BankApiTest.Models.DTOs;
using BankApiTest.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankApiTest.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CoinDeskController : ControllerBase
	{
		private readonly CoinDeskService _coinDeskService;

		public CoinDeskController(CoinDeskService coinDeskService)
		{
			_coinDeskService = coinDeskService;
		}

        [HttpGet("original")]
        public async Task<ActionResult<CoinDeskResponse>> GetOriginalPrice()
        {
            var coinDeskData = await _coinDeskService.GetCoinDeskData();
            return Ok(coinDeskData); 
        }

		[HttpGet("current")]
		public async Task<ActionResult<CoinDeskApiResponse>> GetCurrentPrice()
		{
			var coinDeskData = await _coinDeskService.GetCoinDeskApiResponse();
			return Ok(coinDeskData);
		}
	}
}
