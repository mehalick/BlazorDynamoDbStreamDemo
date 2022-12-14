using Amazon;
using Amazon.DynamoDBv2;
using BlazorDynamoDbStreamDemo.Server.Services;

namespace BlazorDynamoDbStreamDemo.Server;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		builder.Services.AddRazorPages();
		builder.Services.AddGrpc();

		builder.Services.AddSingleton(_ =>
			new AmazonDynamoDBClient(
				builder.Configuration["AWS:AccessKeyId"],
				builder.Configuration["AWS:SecretAccessKey"],
				RegionEndpoint.USEast1));

		var app = builder.Build();

		if (app.Environment.IsDevelopment())
		{
			app.UseWebAssemblyDebugging();
		}
		else
		{
			app.UseExceptionHandler("/Error");
			app.UseHsts();
		}

		app.UseHttpsRedirection();

		app.UseBlazorFrameworkFiles();
		app.UseStaticFiles();

		app.UseRouting();
		app.UseGrpcWeb();

		app.MapRazorPages();
		app.MapFallbackToFile("index.html");
		app.MapGrpcService<BookService>().EnableGrpcWeb();

		app.Run();
	}
}
