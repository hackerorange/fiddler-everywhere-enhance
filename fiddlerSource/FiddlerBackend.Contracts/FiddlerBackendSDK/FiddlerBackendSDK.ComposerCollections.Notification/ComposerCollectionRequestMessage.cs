using FiddlerBackendSDK.ComposerCollections.Client;

namespace FiddlerBackendSDK.ComposerCollections.Notification;

public class ComposerCollectionRequestMessage : ComposerCollectionNotificationMessage
{
	public ComposerCollectionRequest Request { get; set; }
}
