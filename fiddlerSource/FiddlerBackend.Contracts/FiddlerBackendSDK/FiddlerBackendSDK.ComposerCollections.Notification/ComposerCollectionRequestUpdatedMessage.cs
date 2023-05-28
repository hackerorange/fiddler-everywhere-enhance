using System;

namespace FiddlerBackendSDK.ComposerCollections.Notification;

public class ComposerCollectionRequestUpdatedMessage : ComposerCollectionRequestMessage
{
	public string OldName { get; set; }

	public string OldHttpMethod { get; set; }

	public string OldUrl { get; set; }

	public Guid OldBodyId { get; set; }

	public Guid OldHeadersId { get; set; }

	public string OldHttpVersion { get; set; }

	public string OldParameters { get; set; }
}
