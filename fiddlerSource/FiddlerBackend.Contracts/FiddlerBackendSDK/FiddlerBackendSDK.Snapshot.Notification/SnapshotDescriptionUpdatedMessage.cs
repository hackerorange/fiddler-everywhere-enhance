namespace FiddlerBackendSDK.Snapshot.Notification;

public class SnapshotDescriptionUpdatedMessage : SnapshotNotificationMessage
{
	public string OldDescription { get; set; }
}
