using System.Net;
using FiddlerBackend.Contracts;

namespace FiddlerBackendSDK.Core.Http.Client.Validation;

public class UnauthorizedClientErrorException : ClientErrorException
{
	public string ReasonPhrase { get; private set; }

	public UnauthorizedClientErrorException(string reasonPhrase)
		: base(HttpStatusCode.Unauthorized, new ErrorDTO
		{
			Message = "The user is not authorized to make the request.",
			Details = "The requested operation could not be authorized, please try logging in again!"
		})
	{
		ReasonPhrase = reasonPhrase;
	}
}
