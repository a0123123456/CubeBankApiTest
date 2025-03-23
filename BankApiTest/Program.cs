
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

			// 根據環境決定使用的資料庫
			if (builder.Environment.IsEnvironment("IntegrationTest"))
			{
				// 測試環境：使用 In-Memory 資料庫
				builder.Services.AddDbContext<BankDbContext>(options =>
					options.UseInMemoryDatabase("TestDb"));
			}
			else
			{
				// 正式或開發環境：使用 SQL Server
				string? connectionString = builder.Configuration.GetConnectionString("BankDbContext");

				builder.Services.AddDbContext<BankDbContext>(options =>
					options.UseSqlServer(connectionString));
			}

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
