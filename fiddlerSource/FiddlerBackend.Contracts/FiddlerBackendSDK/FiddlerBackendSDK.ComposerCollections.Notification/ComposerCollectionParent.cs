using System;

namespace FiddlerBackendSDK.ComposerCollections.Notification;

public class ComposerCollectionParent
{
	public Guid Id { get; set; }

	public bool IsRootCollection { get; set; }

	public string Name { get; set; }
}
