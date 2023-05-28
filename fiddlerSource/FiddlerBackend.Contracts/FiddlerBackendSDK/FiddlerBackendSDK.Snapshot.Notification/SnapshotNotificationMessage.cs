using FiddlerBackendSDK.Notifications;
using FiddlerBackendSDK.Snapshot.Client;

namespace FiddlerBackendSDK.Snapshot.Notification;

public class SnapshotNotificationMessage : NotificationMessage
{
	public BaseSnapshotMetadata Snapshot { get; set; }
}
