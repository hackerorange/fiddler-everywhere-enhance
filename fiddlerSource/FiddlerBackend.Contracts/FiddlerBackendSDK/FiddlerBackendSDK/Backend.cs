using System;
using System.Threading.Tasks;
using AutoMapper;
using FiddlerBackendSDK.ComposerCollections.Client;
using FiddlerBackendSDK.Core;
using FiddlerBackendSDK.Core.Http.Client;
using FiddlerBackendSDK.Notifications;
using FiddlerBackendSDK.Notifications.ChannelNaming;
using FiddlerBackendSDK.Notifications.Pubnub;
using FiddlerBackendSDK.Notifications.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FiddlerBackendSDK;

public class Backend : IBackend
{
	private readonly IBackendConfiguration configuration;

	private readonly IServiceProvider serviceProvider;

	public Backend(IBackendConfiguration configuration)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		this.configuration = configuration;
		serviceProvider = (IServiceProvider)ServiceCollectionContainerBuilderExtensions.BuildServiceProvider(BackendModule.AddFiddlerServices(LoggingServiceCollectionExtensions.AddLogging((IServiceCollection)new ServiceCollection()), this.configuration, configuration.LoggerProvider));
		if (configuration.LoggerProvider != null)
		{
			ServiceProviderServiceExtensions.GetService<ILoggerFactory>(serviceProvider).AddProvider(configuration.LoggerProvider);
		}
	}

	public T GetService<T>()
	{
		return ServiceProviderServiceExtensions.GetService<T>(serviceProvider);
	}

	public async Task<IObservable<NotificationMessage>> GetNotificationObservable(string uniqueClientId, string clientEmail, bool waitForSubscriptionConfirmation = false)
	{
		return new NotificationObservable(await GetService<IPubnubClientFactory>().CreateAsync(uniqueClientId, waitForSubscriptionConfirmation), GetService<INotificationMessageDeserializer>(), GetService<IUserChannelNameCreator>(), GetService<IMapper>(), GetService<IEntityCache<ComposerCollectionCacheItem>>(), clientEmail);
	}

	public string UpdateToken(string idToken)
	{
		string bearerToken = configuration.BearerToken;
		configuration.BearerToken = idToken;
		IFiddlerHttpClient service = GetService<IFiddlerHttpClient>();
		IIdentityHttpClient service2 = GetService<IIdentityHttpClient>();
		service.ReloadToken();
		service2.ReloadToken();
		return bearerToken;
	}
}
