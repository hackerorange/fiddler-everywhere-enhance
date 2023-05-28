using FiddlerBackendSDK.ComposerCollections.Client;

namespace FiddlerBackendSDK.ComposerCollections.Notification;

public class ComposerCollectionFolderMessage : ComposerCollectionNotificationMessage
{
	public ComposerCollectionFolder Folder { get; set; }

	public bool IsEmpty { get; set; }
}
