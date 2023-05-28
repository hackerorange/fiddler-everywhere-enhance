using System.Net;
using FiddlerBackend.Contracts;

namespace FiddlerBackendSDK.Core.Http.Client.Validation;

public class ClientErrorException : HttpException
{
	public ErrorDTO Error { get; private set; }

	public ClientErrorException(HttpStatusCode statusCode, ErrorDTO error = null)
		: base(error?.Message, statusCode)
	{
		Error = error;
	}
}
