using BankApiTest.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApiTest.IntegrationTests
{
	public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
	{
		protected override IHost CreateHost(IHostBuilder builder)
		{
			builder.UseEnvironment("IntegrationTest");
			return base.CreateHost(builder);
		}

		protected override void ConfigureWebHost(IWebHostBuilder builder)
		{
			builder.ConfigureServices(services =>
			{
				// 移除所有 BankDbContext 的註冊（含其選項）
				services.RemoveAll<DbContextOptions<BankDbContext>>();

				// 加入 In-Memory 資料庫設定
				services.AddDbContext<BankDbContext>(options =>
				{
					options.UseInMemoryDatabase("TestDb");
				});

				// 建立 ServiceProvider 並 Seed 測試資料
				using (var sp = services.BuildServiceProvider())
				{
					using (var scope = sp.CreateScope())
					{
						var scopedServices = scope.ServiceProvider;
						var db = scopedServices.GetRequiredService<BankDbContext>();
						db.Database.EnsureCreated();

						if (!db.Currencies.Any())
						{
							db.Currencies.AddRange(
								new Currency { Code = "USD", Rate = "94,927.809", RateFloat = 94927.8092m },
								new Currency { Code = "EUR", Rate = "91,070.987", RateFloat = 91070.9872m }
							);
							db.SaveChanges();
						}
					}
				}
			});
		}
	}
}
