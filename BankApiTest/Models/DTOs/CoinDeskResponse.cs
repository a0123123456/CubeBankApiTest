namespace BankApiTest.Models.DTOs
{
	public class CoinDeskResponse
	{
		public Time Time { get; set; }
		public string Disclaimer { get; set; }
		public string ChartName { get; set; }
		public Bpi Bpi { get; set; }
	}

	public class Time
	{
		public string Updated { get; set; }
		public string UpdatedISO { get; set; }
		public string Updateduk { get; set; }
	}

	public class Bpi
	{
		public CurrencyInfo USD { get; set; }
		public CurrencyInfo GBP { get; set; }
		public CurrencyInfo EUR { get; set; }
	}

	public class CurrencyInfo
	{
		public string Code { get; set; }
		public string Symbol { get; set; }
		public string Rate { get; set; }
		public string Description { get; set; }
		public float RateFloat { get; set; }
	}
}
