using FiddlerBackendSDK.ComposerCollections.Client;

namespace FiddlerBackendSDK.ComposerCollections.Notification;

public abstract class ComposerCollectionResourceMovedMessage : ComposerCollectionNotificationMessage
{
	public ComposerCollection OldCollection { get; set; }

	public ComposerCollection NewCollection { get; set; }

	public ComposerCollectionParent OldFolder { get; set; }

	public ComposerCollectionParent NewFolder { get; set; }
}
