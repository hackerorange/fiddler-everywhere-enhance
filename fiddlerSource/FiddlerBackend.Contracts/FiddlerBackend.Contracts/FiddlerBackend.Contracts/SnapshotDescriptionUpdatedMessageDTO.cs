namespace FiddlerBackend.Contracts;

public class SnapshotDescriptionUpdatedMessageDTO : SnapshotNotificationMessageDTO
{
	public string OldDescription { get; set; }
}
