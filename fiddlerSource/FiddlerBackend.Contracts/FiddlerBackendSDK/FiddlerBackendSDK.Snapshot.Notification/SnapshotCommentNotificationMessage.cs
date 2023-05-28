using FiddlerBackendSDK.Notifications;
using FiddlerBackendSDK.Snapshot.Client;

namespace FiddlerBackendSDK.Snapshot.Notification;

public class SnapshotCommentNotificationMessage : NotificationMessage
{
	public SnapshotRequestComment SnapshotRequestComment { get; set; }
}
