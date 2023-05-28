using System.Collections.Generic;
using FiddlerBackendSDK.Notifications;

namespace FiddlerBackendSDK.User;

public class UserNotifications
{
	public int TotalCount { get; set; }

	public int TotalUnreadCount { get; set; }

	public IList<EventNotificationMessage> Notifications { get; set; }

	public IList<string> NonDeserializablePayloads { get; set; }
}
