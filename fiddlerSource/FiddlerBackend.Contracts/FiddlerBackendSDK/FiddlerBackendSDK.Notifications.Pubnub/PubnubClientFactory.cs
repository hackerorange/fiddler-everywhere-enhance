using System;
using System.Net;
using System.Threading.Tasks;
using FiddlerBackendSDK.Core.Http.Client;
using FiddlerBackendSDK.Core.Http.Client.Validation;

namespace FiddlerBackendSDK.Notifications.Pubnub;

public class PubnubClientFactory : IPubnubClientFactory
{
	private readonly IFiddlerHttpClient httpClient;

	private readonly IValidationExceptionFactory exceptionFactory;

	private readonly IBackendConfiguration backendConfiguration;

	public PubnubClientFactory(IFiddlerHttpClient httpClient, IValidationExceptionFactory exceptionFactory, IBackendConfiguration backendConfiguration)
	{
		this.httpClient = httpClient;
		this.exceptionFactory = exceptionFactory;
		this.backendConfiguration = backendConfiguration;
	}

	public async Task<IPubnubClient> CreateAsync(string uniqueClientId, bool waitForSubscriptionConfirmation = false)
	{
		return new PubnubClient(await GetPushNotificationsSubscribeKeyAsync(), uniqueClientId, backendConfiguration.Proxy, waitForSubscriptionConfirmation);
	}

	private async Task<string> GetPushNotificationsSubscribeKeyAsync()
	{
		string requestUri = "push-notifications-configuration";
		IFiddlerHttpStatusCodeValidator statusCodeValidator = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(exceptionFactory).WithSuccessCode(HttpStatusCode.OK).WithErrorCode(HttpStatusCode.Unauthorized).Create();
		PushNotificationsConfigurationResponseDTO obj = await httpClient.GetAsync<PushNotificationsConfigurationResponseDTO>(requestUri, statusCodeValidator);
		if (string.IsNullOrWhiteSpace(obj.PubnubSubscribeKey))
		{
			throw new ArgumentException("The received PubnubSubscribeKey is empty", "PubnubSubscribeKey");
		}
		return obj.PubnubSubscribeKey;
	}
}
