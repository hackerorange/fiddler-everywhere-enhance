namespace FiddlerBackend.Contracts;

public class ValidationException : DetailsException
{
	public ValidationException(string message, string details = null)
		: base(message, details)
	{
	}
}
