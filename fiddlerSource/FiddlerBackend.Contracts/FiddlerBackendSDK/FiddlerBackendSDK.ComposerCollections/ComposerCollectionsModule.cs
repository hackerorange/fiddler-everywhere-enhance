using FiddlerBackendSDK.ComposerCollections.Client;
using Microsoft.Extensions.DependencyInjection;

namespace FiddlerBackendSDK.ComposerCollections;

public static class ComposerCollectionsModule
{
	public static void AddComposerCollectionsServices(this IServiceCollection serviceCollection)
	{
		ServiceCollectionServiceExtensions.AddScoped<IComposerCollectionClient, ComposerCollectionClient>(serviceCollection);
	}
}
