using System.Net;

namespace FiddlerBackendSDK.Core.Http.Client.Validation;

public class ConcurrentModificationException : ClientErrorException
{
	public ConcurrentModificationException(HttpStatusCode statusCode)
		: base(statusCode)
	{
	}
}
