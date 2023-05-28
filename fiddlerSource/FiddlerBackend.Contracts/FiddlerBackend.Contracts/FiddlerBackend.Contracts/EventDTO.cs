using System;

namespace FiddlerBackend.Contracts;

public class EventDTO
{
	public Guid Id { get; set; }

	public string Type { get; set; }

	public string Product { get; set; }

	public string SenderEmail { get; set; }

	public string ReceiverEmail { get; set; }

	public BaseUserDTO Sender { get; set; }

	public BaseUserDTO Receiver { get; set; }

	public DateTime CreatedAt { get; set; }

	public string Payload { get; set; }

	public bool IsRead { get; set; }
}
