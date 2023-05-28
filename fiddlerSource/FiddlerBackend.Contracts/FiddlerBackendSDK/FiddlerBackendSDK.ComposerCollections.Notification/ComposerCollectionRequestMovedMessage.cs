using FiddlerBackendSDK.ComposerCollections.Client;

namespace FiddlerBackendSDK.ComposerCollections.Notification;

public class ComposerCollectionRequestMovedMessage : ComposerCollectionResourceMovedMessage
{
	public ComposerCollectionRequest Request { get; set; }
}
