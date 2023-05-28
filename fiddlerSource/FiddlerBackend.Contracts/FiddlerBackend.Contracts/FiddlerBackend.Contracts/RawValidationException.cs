using System;

namespace FiddlerBackend.Contracts;

public class RawValidationException : Exception
{
	public string ContentType { get; private set; }

	public RawValidationException(string message, string contentType)
		: base(message)
	{
		ContentType = contentType;
	}
}
