using System;

namespace FiddlerBackendSDK.Snapshot.Notification;

public class NewSnapshotVersionMessage : SnapshotNotificationMessage
{
	public Guid VersionId { get; set; }
}
