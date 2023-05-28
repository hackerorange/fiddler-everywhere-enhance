using System.Net;

namespace FiddlerBackendSDK.Core.Http.Client;

public interface IHttpErrorException
{
	HttpStatusCode HttpStatusCode { get; }
}
