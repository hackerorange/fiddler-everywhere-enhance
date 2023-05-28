namespace FiddlerBackendSDK.Snapshot.Notification;

public class SnapshotNameUpdatedMessage : SnapshotNotificationMessage
{
	public string OldName { get; set; }
}
