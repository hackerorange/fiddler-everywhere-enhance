using System;
using System.Net;

namespace FiddlerBackendSDK.Core.Http.Client;

public class HttpException : Exception, IHttpErrorException
{
	public HttpStatusCode HttpStatusCode { get; }

	public HttpException(HttpStatusCode statusCode)
		: this($"Unexpected status code: {statusCode}", statusCode)
	{
	}

	public HttpException(string message, HttpStatusCode statusCode)
		: base(message)
	{
		HttpStatusCode = statusCode;
	}
}
