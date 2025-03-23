using BankApiTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BankApiTest.IntegrationTests
{
	public class CurrenciesApiTests : IClassFixture<CustomWebApplicationFactory<Program>>
	{
		private readonly HttpClient _client;

		public CurrenciesApiTests(CustomWebApplicationFactory<Program> factory)
		{
			// 透過自訂工廠建立 HttpClient，模擬完整的 HTTP 請求管線
			_client = factory.CreateClient();
		}

		[Fact]
		public async Task GetCurrencies_ReturnsCurrencies()
		{
			// 發送 GET 請求到 API 端點
			var response = await _client.GetAsync("/api/Currencies/CurrenciesQuery");

			// 驗證回應狀態碼是否成功（200 OK）
			response.EnsureSuccessStatusCode();

			// 讀取回應內容並檢查是否包含 Seed 資料的值
			var responseString = await response.Content.ReadAsStringAsync();
			Assert.Contains("USD", responseString);
			Assert.Contains("EUR", responseString);
		}

		[Fact]
		public async Task AddCurrency_ReturnsCreatedCurrency()
		{
			// Arrange: 建立一個新的貨幣物件
			var newCurrency = new Currency
			{
				Code = "TWD",
				Symbol = "&twd;",
				Rate = "20,000.999",
				Description = "Taiwan",
				RateFloat = 20000.999m
			};
			var json = JsonSerializer.Serialize(newCurrency);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			// Act: 發送 POST 請求到 /api/Currencies/AddCurrencies 端點
			var response = await _client.PostAsync("/api/Currencies/AddCurrencies", content);

			// Assert: 確認回應狀態碼為 201 Created
			Assert.Equal(HttpStatusCode.Created, response.StatusCode);

			// 讀取回應內容，反序列化成 Currency 物件，並檢查資料是否正確
			var responseString = await response.Content.ReadAsStringAsync();
			var createdCurrency = JsonSerializer.Deserialize<Currency>(responseString, new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			});

			Assert.NotNull(createdCurrency);
			Assert.Equal(newCurrency.Code, createdCurrency.Code);
			Assert.Equal(newCurrency.Symbol, createdCurrency.Symbol);
			Assert.Equal(newCurrency.Rate, createdCurrency.Rate);
			Assert.Equal(newCurrency.Description, createdCurrency.Description);
			Assert.Equal(newCurrency.RateFloat, createdCurrency.RateFloat);
		}
	}
}
