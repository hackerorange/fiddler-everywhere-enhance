namespace FiddlerBackend.Contracts;

public class SnapshotNameUpdatedMessageDTO : SnapshotNotificationMessageDTO
{
	public string OldName { get; set; }
}
