using BankApiTest.Models.DTOs;
using Newtonsoft.Json;
using System.Globalization;

namespace BankApiTest.Services
{
	public class CoinDeskService
	{
		private readonly HttpClient _httpClient;

		public CoinDeskService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<CoinDeskResponse> GetCoinDeskData()
		{
			var response = await _httpClient.GetStringAsync("https://api.coindesk.com/v1/bpi/currentprice.json");

			var coinDeskData = JsonConvert.DeserializeObject<CoinDeskResponse>(response);

			return coinDeskData;
		}

		public async Task<CoinDeskApiResponse> GetCoinDeskApiResponse()
		{
			var coinDeskData = await GetCoinDeskData();

			DateTime updateTimeUtc = DateTime.ParseExact(
				coinDeskData.Time.Updated,
				"MMM dd, yyyy HH:mm:ss UTC",
				CultureInfo.InvariantCulture);

			TimeZoneInfo taiwanTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");
			DateTime updateTimeTST = TimeZoneInfo.ConvertTimeFromUtc(updateTimeUtc, taiwanTimeZone);

			var coinDeskApiResponse = new CoinDeskApiResponse
			{
				UpdateTime = updateTimeTST.ToString("yyyy/MM/dd HH:mm:ss"),
				Currencies = new List<CurrencyInfoResponse>
				{
					new CurrencyInfoResponse
					{
						Code = coinDeskData.Bpi.USD.Code,
						Name = "美元",
                        Rate = coinDeskData.Bpi.USD.Rate
					},
					new CurrencyInfoResponse
					{
						Code = coinDeskData.Bpi.GBP.Code,
						Name = "英鎊",
                        Rate = coinDeskData.Bpi.GBP.Rate
					},
					new CurrencyInfoResponse
					{
						Code = coinDeskData.Bpi.EUR.Code,
						Name = "歐元",
                        Rate = coinDeskData.Bpi.EUR.Rate
					}
				}
			};


			return coinDeskApiResponse;
		}
	}
}
