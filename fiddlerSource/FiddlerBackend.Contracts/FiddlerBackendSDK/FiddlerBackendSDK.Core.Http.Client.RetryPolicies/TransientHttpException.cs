using System.Net;

namespace FiddlerBackendSDK.Core.Http.Client.RetryPolicies;

public class TransientHttpException : HttpException
{
	public TransientHttpException(HttpStatusCode statusCode)
		: base(statusCode)
	{
	}
}
