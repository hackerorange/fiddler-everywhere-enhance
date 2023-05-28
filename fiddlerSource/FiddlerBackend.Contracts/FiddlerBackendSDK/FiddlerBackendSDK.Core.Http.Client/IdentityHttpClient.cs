using System.Net.Http;

namespace FiddlerBackendSDK.Core.Http.Client;

public class IdentityHttpClient : AuthenticatedHttpClient, IIdentityHttpClient, IAuthenticatedHttpClient
{
	public IdentityHttpClient(IHttpClientFactory httpClientFactory, IBackendConfiguration configuration)
		: base(httpClientFactory, configuration, "IdentityApi")
	{
	}
}
