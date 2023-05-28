using System;
using FiddlerBackend.Contracts;

namespace FiddlerBackendSDK.Notifications;

public class NotificationMessage
{
	public DateTime CreatedAt { get; set; }

	public Guid Id { get; set; }

	public string Operation { get; set; }

	public int UserNotificationCounter { get; set; }

	public BaseUserDTO Sender { get; set; }

	public BaseUserDTO Receiver { get; set; }

	public FiddlerProduct Product { get; set; }
}
