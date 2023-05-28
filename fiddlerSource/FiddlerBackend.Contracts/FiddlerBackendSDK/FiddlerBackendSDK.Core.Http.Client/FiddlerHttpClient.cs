using System.Net.Http;

namespace FiddlerBackendSDK.Core.Http.Client;

public class FiddlerHttpClient : AuthenticatedHttpClient, IFiddlerHttpClient, IAuthenticatedHttpClient
{
	public FiddlerHttpClient(IHttpClientFactory httpClientFactory, IBackendConfiguration configuration)
		: base(httpClientFactory, configuration, "FiddlerBackendAPI")
	{
	}
}
