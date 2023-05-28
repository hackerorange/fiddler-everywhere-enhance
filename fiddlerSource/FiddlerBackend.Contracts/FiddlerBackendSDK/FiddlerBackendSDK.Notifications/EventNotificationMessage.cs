using System;

namespace FiddlerBackendSDK.Notifications;

public class EventNotificationMessage
{
	public bool IsRead { get; set; }

	public Guid EventId { get; set; }

	public NotificationMessage Message { get; set; }
}
