using FiddlerBackendSDK.ComposerCollections.Client;
using FiddlerBackendSDK.Notifications;

namespace FiddlerBackendSDK.ComposerCollections.Notification;

public class ComposerCollectionNotificationMessage : NotificationMessage
{
	public ComposerCollection ComposerCollection { get; set; }
}
