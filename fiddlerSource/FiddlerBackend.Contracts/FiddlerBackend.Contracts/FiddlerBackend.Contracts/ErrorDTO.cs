namespace FiddlerBackend.Contracts;

public class ErrorDTO
{
	public string Message { get; set; }

	public string Details { get; set; }

	public int? ErrorCode { get; set; }

	public ErrorDTO()
		: this(null, null)
	{
	}

	public ErrorDTO(string message)
		: this(message, null)
	{
	}

	public ErrorDTO(string message, string details, ErrorCode? errorCode = null)
	{
		Message = message;
		Details = details;
		ErrorCode = (int?)errorCode;
	}
}
