
using BankApiTest.Models;
using BankApiTest.Services;
using Microsoft.EntityFrameworkCore;

namespace BankApiTest
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddHttpClient<CoinDeskService>();

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			//1.���o�պA����Ʈw�s�u�]�w
			string? connectionString = builder.Configuration.GetConnectionString("BankDbContext");

			//2.���UEF Core��DbContext
			builder.Services.AddDbContext<BankDbContext>(options => options.UseSqlServer(connectionString));


			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
