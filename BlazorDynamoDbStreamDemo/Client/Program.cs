using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace BlazorDynamoDbStreamDemo.Client;

public class Program
{
	public static async Task Main(string[] args)
	{
		var builder = WebAssemblyHostBuilder.CreateDefault(args);
		builder.RootComponents.Add<App>("#app");
		builder.RootComponents.Add<HeadOutlet>("head::after");

		builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
		builder.Services.AddSingleton(services => new BookService.BookServiceClient(Channel(services)));

		await builder.Build().RunAsync();
	}

	private static GrpcChannel Channel(IServiceProvider services)
	{
		var baseUri = services.GetRequiredService<NavigationManager>().BaseUri;
		var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));

		return GrpcChannel.ForAddress(baseUri, new GrpcChannelOptions
		{
			HttpClient = httpClient
		});
	}
}
