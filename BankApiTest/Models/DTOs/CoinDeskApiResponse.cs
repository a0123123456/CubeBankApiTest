namespace BankApiTest.Models.DTOs
{
	public class CoinDeskApiResponse
	{
		public string UpdateTime { get; set; }  
		public List<CurrencyInfoResponse> Currencies { get; set; } 
	}

	public class CurrencyInfoResponse
	{
		public string Code { get; set; }  
		public string Name { get; set; }  
		public string Rate { get; set; } 
	}
}
