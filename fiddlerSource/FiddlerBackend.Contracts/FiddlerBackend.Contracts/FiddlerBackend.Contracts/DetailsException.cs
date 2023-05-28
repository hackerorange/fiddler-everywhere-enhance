using System;

namespace FiddlerBackend.Contracts;

public class DetailsException : Exception
{
	public string Details { get; set; }

	public DetailsException(string message, string details)
		: base(message)
	{
		Details = details;
	}

	public override string ToString()
	{
		return base.ToString() + "\nDetails: " + (Details ?? string.Empty);
	}
}
