using FiddlerBackendSDK.Files.Client;
using FiddlerBackendSDK.Notifications.Pubnub;
using FiddlerBackendSDK.Snapshot.Client;
using Microsoft.Extensions.DependencyInjection;

namespace FiddlerBackendSDK.Snapshot;

public static class SnapshotModule
{
	public static void AddSnapshotServices(this IServiceCollection servicesCollection)
	{
		ServiceCollectionServiceExtensions.AddScoped<ISnapshotClient, SnapshotClient>(servicesCollection);
		ServiceCollectionServiceExtensions.AddScoped<IFileClient, FileClient>(servicesCollection);
		ServiceCollectionServiceExtensions.AddScoped<ISnapshotTransformer, SnapshotTransformer>(servicesCollection);
		ServiceCollectionServiceExtensions.AddScoped<IPubnubClientFactory, PubnubClientFactory>(servicesCollection);
	}
}
