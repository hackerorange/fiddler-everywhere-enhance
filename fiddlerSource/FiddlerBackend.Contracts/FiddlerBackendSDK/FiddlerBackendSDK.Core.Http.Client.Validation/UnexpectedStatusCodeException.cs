using System.Net;

namespace FiddlerBackendSDK.Core.Http.Client.Validation;

public class UnexpectedStatusCodeException : HttpException
{
	public string Details { get; private set; }

	public UnexpectedStatusCodeException(HttpStatusCode statusCode, string details)
		: base(statusCode)
	{
		Details = details;
	}
}
