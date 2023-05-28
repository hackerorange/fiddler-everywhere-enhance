using Microsoft.Extensions.DependencyInjection;

namespace FiddlerBackendSDK.Core.Http.Client;

public static class FiddlerHttpModule
{
	public static void AddFiddlerHttpClient(this IServiceCollection servicesCollection)
	{
		ServiceCollectionServiceExtensions.AddScoped<IFileDownloader, FileDownloader>(servicesCollection);
		ServiceCollectionServiceExtensions.AddScoped<IFiddlerHttpClient, FiddlerHttpClient>(servicesCollection);
		ServiceCollectionServiceExtensions.AddScoped<IIdentityHttpClient, IdentityHttpClient>(servicesCollection);
	}
}
