using FiddlerBackendSDK.AutoResponder.Client;
using Microsoft.Extensions.DependencyInjection;

namespace FiddlerBackendSDK.AutoResponder;

public static class RuleSetsModule
{
	public static void AddRuleSetsServices(this IServiceCollection servicesCollection)
	{
		ServiceCollectionServiceExtensions.AddScoped<IRuleSetsClient, RuleSetsClient>(servicesCollection);
	}
}
