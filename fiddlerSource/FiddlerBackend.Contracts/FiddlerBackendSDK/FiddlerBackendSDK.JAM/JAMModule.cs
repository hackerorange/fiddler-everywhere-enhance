using Microsoft.Extensions.DependencyInjection;

namespace FiddlerBackendSDK.JAM;

public static class JAMModule
{
	public static void AddJAMServices(this IServiceCollection servicesCollection)
	{
		ServiceCollectionServiceExtensions.AddScoped<IJAMSessionsClient, JAMSessionsClient>(servicesCollection);
	}
}
