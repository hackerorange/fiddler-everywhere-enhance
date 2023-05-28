using System;
using Newtonsoft.Json;

namespace FiddlerBackend.Contracts;

public class NotificationMessageDTO
{
	public Guid Id { get; set; } = Guid.NewGuid();


	public virtual FiddlerProduct Product { get; }

	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


	public string Operation { get; set; }

	public long UserNotificationCounter { get; set; }

	public BaseUserDTO Sender { get; set; }

	[JsonProperty(/*Could not decode attribute arguments.*/)]
	public BaseUserDTO Receiver { get; set; }
}
