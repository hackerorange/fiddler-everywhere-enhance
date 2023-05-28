using System;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using AutoMapper;
using FiddlerBackendSDK.AutoResponder;
using FiddlerBackendSDK.ComposerCollections;
using FiddlerBackendSDK.ComposerCollections.Client;
using FiddlerBackendSDK.Core;
using FiddlerBackendSDK.Core.Http.Client;
using FiddlerBackendSDK.Core.Http.Client.RetryPolicies;
using FiddlerBackendSDK.Core.Http.Client.Validation;
using FiddlerBackendSDK.Files.Client;
using FiddlerBackendSDK.JAM;
using FiddlerBackendSDK.License;
using FiddlerBackendSDK.Mapping;
using FiddlerBackendSDK.Notifications.ChannelNaming;
using FiddlerBackendSDK.Notifications.Serialization;
using FiddlerBackendSDK.Quota;
using FiddlerBackendSDK.Snapshot;
using FiddlerBackendSDK.Snapshot.Client;
using FiddlerBackendSDK.Subscription;
using FiddlerBackendSDK.User;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Polly;

namespace FiddlerBackendSDK;

public static class BackendModule
{
	public static IServiceCollection AddFiddlerServices(this IServiceCollection serviceCollection, IBackendConfiguration backendConfiguration, ILoggerProvider loggerProvider)
	{
		ServiceCollectionServiceExtensions.AddSingleton<IBackendConfiguration>(serviceCollection, backendConfiguration);
		AddHttpClients(serviceCollection, backendConfiguration, loggerProvider);
		ServiceCollectionServiceExtensions.AddSingleton<IMapper>(serviceCollection, (Func<IServiceProvider, IMapper>)delegate(IServiceProvider provider)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Expected O, but got Unknown
			IServiceProvider provider2 = provider;
			return (IMapper)new Mapper((IConfigurationProvider)new MapperConfiguration((Action<IMapperConfigurationExpression>)delegate(IMapperConfigurationExpression cfg)
			{
				cfg.AddProfile<AutoMapping>();
				cfg.ConstructServicesUsing((Func<Type, object>)((Type type) => ActivatorUtilities.CreateInstance(provider2, type, Array.Empty<object>())));
			}));
		});
		ServiceCollectionServiceExtensions.AddScoped<ICryptoService, CryptoService>(serviceCollection);
		ServiceCollectionServiceExtensions.AddScoped<IQuotaClient, QuotaClient>(serviceCollection);
		ServiceCollectionServiceExtensions.AddScoped<ILicenseClient, LicenseClient>(serviceCollection);
		ServiceCollectionServiceExtensions.AddScoped<ISubscriptionClient, SubscriptionClient>(serviceCollection);
		ServiceCollectionServiceExtensions.AddScoped<IUserClient, UserClient>(serviceCollection);
		ServiceCollectionServiceExtensions.AddScoped<IFileCache, FileCache>(serviceCollection);
		ServiceCollectionServiceExtensions.AddScoped<ISnapshotRepository, SnapshotRepository>(serviceCollection);
		ServiceCollectionServiceExtensions.AddScoped<IMD5Calculator, MD5Calculator>(serviceCollection);
		ServiceCollectionServiceExtensions.AddScoped<IValidationExceptionFactory, ValidationExceptionFactory>(serviceCollection);
		ServiceCollectionServiceExtensions.AddScoped<INotificationMessageDeserializer, NotificationMessageDeserializer>(serviceCollection);
		ServiceCollectionServiceExtensions.AddSingleton<IUserChannelNameCreator>(serviceCollection, (IUserChannelNameCreator)new FiddlerBackendSDK.Notifications.ChannelNaming.UserChannelNameCreator(SHA256.Create()));
		ServiceCollectionServiceExtensions.AddSingleton<IEntityCache<ComposerCollectionCacheItem>, EntityCache<ComposerCollectionCacheItem>>(serviceCollection);
		FiddlerHttpModule.AddFiddlerHttpClient(serviceCollection);
		SnapshotModule.AddSnapshotServices(serviceCollection);
		RuleSetsModule.AddRuleSetsServices(serviceCollection);
		ComposerCollectionsModule.AddComposerCollectionsServices(serviceCollection);
		JAMModule.AddJAMServices(serviceCollection);
		return serviceCollection;
	}

	public static IServiceCollection AddHttpClients(this IServiceCollection serviceCollection, IBackendConfiguration backendConfiguration, ILoggerProvider loggerProvider)
	{
		ILogger logger = (ILogger)(object)NullLogger.Instance;
		if (loggerProvider != null)
		{
			logger = loggerProvider.CreateLogger(typeof(PollyRetryPolicy).Name);
		}
		ServiceCollectionServiceExtensions.AddTransient<FiddlerBackendSDK.Core.Http.Client.SignedResponseHandler>(serviceCollection);
		PollyHttpClientBuilderExtensions.AddPolicyHandler(HttpClientBuilderExtensions.AddHttpMessageHandler<FiddlerBackendSDK.Core.Http.Client.SignedResponseHandler>(HttpClientBuilderExtensions.ConfigurePrimaryHttpMessageHandler(HttpClientFactoryServiceCollectionExtensions.AddHttpClient(serviceCollection, "FiddlerBackendAPI", (Action<HttpClient>)delegate(HttpClient c)
		{
			c.BaseAddress = new Uri(backendConfiguration.ApiEndpoint);
			c.DefaultRequestHeaders.Add("Authorization", "Bearer " + backendConfiguration.BearerToken);
			c.DefaultRequestHeaders.Add("Accept-Encoding", "gzip");
			c.DefaultRequestHeaders.Add(backendConfiguration.VersionHeaderName, backendConfiguration.DefaultApiVersion);
		}), (Func<HttpMessageHandler>)(() => new HttpClientHandler
		{
			AllowAutoRedirect = false,
			UseDefaultCredentials = true,
			Proxy = backendConfiguration.Proxy,
			UseProxy = true,
			AutomaticDecompression = (DecompressionMethods.GZip | DecompressionMethods.Deflate)
		}))), (IAsyncPolicy<HttpResponseMessage>)(object)PollyRetryPolicy.CreateHttpRetryPolicy(backendConfiguration, logger));
		PollyHttpClientBuilderExtensions.AddPolicyHandler(HttpClientBuilderExtensions.ConfigurePrimaryHttpMessageHandler(HttpClientFactoryServiceCollectionExtensions.AddHttpClient(serviceCollection, "External"), (Func<HttpMessageHandler>)(() => new HttpClientHandler
		{
			AllowAutoRedirect = true,
			Proxy = backendConfiguration.Proxy,
			UseProxy = true
		})), (IAsyncPolicy<HttpResponseMessage>)(object)PollyRetryPolicy.CreateHttpRetryPolicy(backendConfiguration, logger));
		PollyHttpClientBuilderExtensions.AddPolicyHandler(HttpClientBuilderExtensions.ConfigurePrimaryHttpMessageHandler(HttpClientFactoryServiceCollectionExtensions.AddHttpClient(serviceCollection, "IdentityApi", (Action<HttpClient>)delegate(HttpClient c)
		{
			c.BaseAddress = new Uri(backendConfiguration.IdentityEndpoint);
			c.DefaultRequestHeaders.Add("Authorization", "Bearer " + backendConfiguration.BearerToken);
			c.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
		}), (Func<HttpMessageHandler>)(() => new HttpClientHandler
		{
			AllowAutoRedirect = false,
			UseDefaultCredentials = true,
			Proxy = backendConfiguration.Proxy,
			UseProxy = true,
			AutomaticDecompression = (DecompressionMethods.GZip | DecompressionMethods.Deflate)
		})), (IAsyncPolicy<HttpResponseMessage>)(object)PollyRetryPolicy.CreateHttpRetryPolicy(backendConfiguration, logger));
		return serviceCollection;
	}
}
