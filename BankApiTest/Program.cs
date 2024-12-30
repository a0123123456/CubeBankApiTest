
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

			//1.取得組態中資料庫連線設定
			string? connectionString = builder.Configuration.GetConnectionString("BankDbContext");

			//2.註冊EF Core的DbContext
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
