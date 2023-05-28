namespace FiddlerBackend.Contracts;

public class SaveEventDTO
{
	public FiddlerProduct Product { get; set; }

	public string ReceiverEmail { get; set; }

	public BaseUserDTO Sender { get; set; }

	public BaseUserDTO Receiver { get; set; }

	public object Payload { get; set; }
}
