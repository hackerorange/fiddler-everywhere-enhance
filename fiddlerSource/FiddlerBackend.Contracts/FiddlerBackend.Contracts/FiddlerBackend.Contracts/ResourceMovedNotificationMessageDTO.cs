using System;

namespace FiddlerBackend.Contracts;

public class ResourceMovedNotificationMessageDTO : NotificationMessageDTO
{
	public ResourceType ResourceType { get; set; }

	public Guid ResourceId { get; set; }
}
