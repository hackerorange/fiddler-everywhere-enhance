using FiddlerBackendSDK.ComposerCollections.Client;

namespace FiddlerBackendSDK.ComposerCollections.Notification;

public class ComposerCollectionFolderMovedMessage : ComposerCollectionResourceMovedMessage
{
	public ComposerCollectionFolder Folder { get; set; }

	public bool IsEmpty { get; set; }
}
