using BankApiTest.Models;
using BankApiTest.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace BankApiTest.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CurrenciesController : ControllerBase
	{
		private readonly BankDbContext _context;
		

		public CurrenciesController(BankDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Currency>>> GetCurrencies()
		{
			return await _context.Currencies.OrderBy(c => c.Code).ToListAsync();
		}

		[HttpPost]
		public async Task<ActionResult<Currency>> AddCurrency(Currency currency)
		{
			_context.Currencies.Add(currency);
			await _context.SaveChangesAsync();
			return CreatedAtAction(nameof(GetCurrencies), new { id = currency.Id }, currency);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateCurrency(int id, Currency currency)
		{
			if (id != currency.Id) return BadRequest();

			_context.Entry(currency).State = EntityState.Modified;
			await _context.SaveChangesAsync();
			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCurrency(int id)
		{
			var currency = await _context.Currencies.FindAsync(id);
			if (currency == null) return NotFound();

			_context.Currencies.Remove(currency);
			await _context.SaveChangesAsync();
			return NoContent();
		}

	
	}
}
