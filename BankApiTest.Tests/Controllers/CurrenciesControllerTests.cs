using BankApiTest.Controllers;
using BankApiTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApiTest.Tests.Controllers
{
	public class CurrenciesControllerTests
	{
		private readonly DbContextOptions<BankDbContext> _options;

		// 在測試初始化時使用 In-Memory 資料庫
		public CurrenciesControllerTests()
		{
			_options = new DbContextOptionsBuilder<BankDbContext>()
				.UseInMemoryDatabase(databaseName: "CurrencyDb")
				.Options;
		}

		// 測試 GET /api/currencies
		[Fact]
		public async Task GetCurrencies_ShouldReturnCurrencies()
		{
			// Arrange
			using (var context = new BankDbContext(_options))
			{
				context.Currencies.Add(new Currency { Code = "USD", Rate = "94,927.809", RateFloat = 94927.8092m });
				context.Currencies.Add(new Currency { Code = "EUR", Rate = "91,070.987", RateFloat = 91070.9872m });
				context.Currencies.Add(new Currency { Code = "GBP", Rate = "75,703.029", RateFloat = 75703.0293m });
				await context.SaveChangesAsync();
			}

			// Act
			using (var context = new BankDbContext(_options))
			{
				var controller = new CurrenciesController(context);
				var result = await controller.GetCurrencies();

				// Assert
				var okResult = Assert.IsType<ActionResult<IEnumerable<Currency>>>(result);
				var currencies = Assert.IsType<List<Currency>>(okResult.Value);
				Assert.Equal(3, currencies.Count);
			}
		}

		// 測試 POST /api/currencies
		[Fact]
		public async Task AddCurrency_ShouldAddCurrency()
		{
			// Arrange
			var newCurrency = new Currency { Code = "AUD", Rate = "70,000.00", RateFloat = 70000.00m };

			using (var context = new BankDbContext(_options))
			{
				var controller = new CurrenciesController(context);
				var result = await controller.AddCurrency(newCurrency);

				// Assert
				var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
				var addedCurrency = Assert.IsType<Currency>(createdAtActionResult.Value);
				Assert.Equal(newCurrency.Code, addedCurrency.Code);
				Assert.Equal(newCurrency.Rate, addedCurrency.Rate);
				Assert.Equal(newCurrency.RateFloat, addedCurrency.RateFloat);
			}
		}

		// 測試 PUT /api/currencies/{id}
		[Fact]
		public async Task UpdateCurrency_ShouldUpdateCurrency()
		{
			// Arrange
			var newCurrency = new Currency { Code = "USD", Rate = "94,927.809", RateFloat = 94927.8092m };
			using (var context = new BankDbContext(_options))
			{
				context.Currencies.Add(newCurrency);
				await context.SaveChangesAsync();
			}

			newCurrency.Rate = "95,000.00";

			using (var context = new BankDbContext(_options))
			{
				var controller = new CurrenciesController(context);
				var result = await controller.UpdateCurrency(newCurrency.Id, newCurrency);

				// Assert
				Assert.IsType<NoContentResult>(result);
				var updatedCurrency = await context.Currencies.FindAsync(newCurrency.Id);
				Assert.Equal("95,000.00", updatedCurrency?.Rate);
			}
		}

		// 測試 DELETE /api/currencies/{id}
		[Fact]
		public async Task DeleteCurrency_ShouldRemoveCurrency()
		{
			// Arrange
			var newCurrency = new Currency { Code = "USD", Rate = "94,927.809", RateFloat = 94927.8092m };
			using (var context = new BankDbContext(_options))
			{
				context.Currencies.Add(newCurrency);
				await context.SaveChangesAsync();
			}

			// Act
			using (var context = new BankDbContext(_options))
			{
				var controller = new CurrenciesController(context);
				var result = await controller.DeleteCurrency(newCurrency.Id);

				// Assert
				Assert.IsType<NoContentResult>(result);
				var deletedCurrency = await context.Currencies.FindAsync(newCurrency.Id);
				Assert.Null(deletedCurrency);
			}
		}
	}
}
